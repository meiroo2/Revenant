using System;
using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;


public class ScreenCaptureMgr : MonoBehaviour
{
    public float m_LerpedPos = 3.56f;
    public float m_Speed;
    public AnimationCurve m_Curve;
    public Material m_LeftMat;
    public Material m_RightMat;
    
    private Image m_LImg;
    private Image m_RImg;
    
    public Camera m_Cam;
    private CameraMgr m_CamMgr;
    
    private int width = 1920;
    private int height = 1080;

    private Texture2D m_ScreenShotTex;
    private Rect m_ScreenRect;
    private RenderTexture m_CamRenderTex;

    private BulletTimeMgr m_BulletTimeMgr;
    
    private Coroutine TearCoroutine;
    private static readonly int Rotate = Shader.PropertyToID("_Rotate");
    private static readonly int PivotX = Shader.PropertyToID("_PivotX");
    private static readonly int PivotY = Shader.PropertyToID("_PivotY");

    private float m_TimeSliceAngle;

    private void Awake()
    {
        m_Cam = Camera.main;
        m_CamMgr = m_Cam.GetComponent<CameraMgr>();
        
        if (m_Cam.TryGetComponent(out ScreenCaptureCanvas scCanvas))
        {
            var CanvasOnCam = m_Cam.GetComponentInChildren<ScreenCaptureCanvas>();
            m_LImg = CanvasOnCam.m_LImg;
            m_RImg = CanvasOnCam.m_RImg;
        
            m_ScreenShotTex = new Texture2D(width, height, TextureFormat.ARGB32, false);
            m_ScreenRect = new Rect(0, 0, width, height);
        }
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_BulletTimeMgr = instance.GetComponentInChildren<BulletTimeMgr>();
    }

    public void Capture(float _xPos, float _yPos, float _timeSliceAngle)
    {
        m_TimeSliceAngle = _timeSliceAngle;
        m_CamRenderTex = new RenderTexture(width, height, 24);
        
        m_Cam.targetTexture = m_CamRenderTex;
        m_Cam.Render();
        
        RenderTexture.active = m_CamRenderTex;
        m_ScreenShotTex.ReadPixels(m_ScreenRect,0,0);
        m_ScreenShotTex.Apply();

        m_LImg.sprite = Sprite.Create(m_ScreenShotTex, new Rect(0, 0, width, height),
            new Vector2(0.5f, 0.5f));
        m_RImg.sprite = m_LImg.sprite;
        
        RenderTexture.active = null;
        m_Cam.targetTexture = null;
        Destroy(m_CamRenderTex);

        Color color = Color.white;

        m_LImg.material = m_LeftMat;
        m_RImg.material = m_RightMat;
        
        // 쉐이더 0.785가 왼쪽위 오른아래
        // OBJ 60.7도가 왼쪽위 오른아래
        float angle = _timeSliceAngle * 0.01293f;
        
        m_LImg.material.SetFloat(Rotate, angle);
        m_RImg.material.SetFloat(Rotate, angle);

        float pivotXVal = m_Cam.transform.position.x - _xPos;
        //float pivot = -0.8f;
       // Debug.Log(pivotXVal.ToString());
        m_LImg.material.SetFloat(PivotX, pivotXVal / 8f);
        m_RImg.material.SetFloat(PivotX, pivotXVal / 8f);

        float pivotYVal = -(m_CamMgr.p_YValue / 4.5f);
        m_LImg.material.SetFloat(PivotY, pivotYVal);
        m_RImg.material.SetFloat(PivotY, pivotYVal);
        
        
        m_LImg.color = color;
        m_RImg.color = color;


        m_LImg.rectTransform.anchoredPosition = Vector2.zero;
        m_RImg.rectTransform.anchoredPosition = Vector2.zero;

        if (!ReferenceEquals(TearCoroutine, null))
            StopCoroutine(TearCoroutine);
        TearCoroutine = StartCoroutine(MoveImg());
    }

    private IEnumerator MoveImg()
    {
        /*
        m_BulletTimeMgr.ChangeTimeScale(0f);
        
        float IncreaseSpeed = 1f;
        float forLValue = 0f;
        float forRValue = 0f;

        float deltaTime = 0f;
        float Timer = 0f;
        
        while (true)
        {
            deltaTime = Time.unscaledDeltaTime;
            
            m_LImg.rectTransform.anchoredPosition = new Vector2(forLValue, 0f);
            m_RImg.rectTransform.anchoredPosition = new Vector2(forRValue, 0f);

            forLValue -= deltaTime * 0.5f;
            forRValue += deltaTime * 0.5f;

            if (forRValue > 0.05f)
                break;

            yield return null;
        }
        
        yield return new WaitForSecondsRealtime(0.5f);
        m_BulletTimeMgr.ChangeTimeScale(1f);
        
        while (true)
        {
            deltaTime = Time.unscaledDeltaTime;
            
            m_LImg.rectTransform.anchoredPosition = new Vector2(forLValue, 0f);
            m_RImg.rectTransform.anchoredPosition = new Vector2(forRValue, 0f);

            forLValue -= deltaTime  * IncreaseSpeed;
            forRValue +=  deltaTime  * IncreaseSpeed;
            IncreaseSpeed *= 1.3f;

            Timer += deltaTime;
            if (Timer > 3f)
            {
                break;
            }
            
            yield return null;
        }
        */
        //m_BulletTimeMgr.ChangeTimeScale(0f);
        float deltaTime = 0f;
        float forLValue = 0f;
        float forRValue = 0f;

        float Timer = 0f;
        float evaluated = 0f;

        float LerpVal = 0f;
        Vector2 LOriginPos = m_LImg.rectTransform.anchoredPosition;
        Vector2 LLerpPos = LOriginPos;
        LLerpPos.x -= m_LerpedPos;

        Vector2 ROriginPos = m_RImg.rectTransform.anchoredPosition;
        Vector2 RLerpPos = ROriginPos;
        RLerpPos.x += m_LerpedPos;

        while (true)
        {
            deltaTime = Time.unscaledDeltaTime;
            Timer += deltaTime;
            
            evaluated = m_Curve.Evaluate(Timer);
            Debug.Log(evaluated);
            LerpVal = evaluated;
            
            m_LImg.rectTransform.anchoredPosition = Vector2.Lerp(LOriginPos, LLerpPos, LerpVal);
            m_RImg.rectTransform.anchoredPosition = Vector2.Lerp(ROriginPos, RLerpPos, LerpVal);
            
            
            

           
            
            if (LerpVal > 1f)
            {
                break;
            }
            yield return null;
        }
        
        yield break;
    }
}