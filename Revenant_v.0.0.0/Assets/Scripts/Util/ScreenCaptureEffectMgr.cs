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


public class ScreenCaptureEffectMgr : MonoBehaviour
{
    public float p_BlackWaitTime = 1f;
    public float p_BlackFadeSpeed = 2f;
    
    public float p_LineXScale = 3f;
    public float p_LineScaleSpeed = 2f;

    public float p_LineMatSpeed = 1f;
    
    public float m_InitLerpedPos = 3.56f;
    
    public float m_AdjustAngle = 0.01293f;
    public float m_AdjustX = 8f;
    public float m_AdjustY = 4.5f;
    public AnimationCurve m_Curve;
    public Material m_LeftMat;
    public Material m_RightMat;

    [HideInInspector] public Action m_MoveImgEndAction = null;
    
    private AR_ScreenCapture m_ScreenCaptureCanvas;
    
    private SpriteRenderer m_LImg;
    private SpriteRenderer m_RImg;
    
    private Camera m_Cam;
    private CameraMgr m_CamMgr;
    
    private int width = 1920;
    private int height = 1080;

    private Texture2D m_ScreenShotTex;
    private Rect m_ScreenRect;
    private RenderTexture m_CamRenderTex;

    private BulletTimeMgr m_BulletTimeMgr;
    
    private Coroutine m_TearCoroutine = null;
    private Coroutine m_EffectCoroutine = null;
    private Coroutine m_BlackImgCoroutine = null;
    
    private static readonly int Rotate = Shader.PropertyToID("_Rotate");
    private static readonly int PivotX = Shader.PropertyToID("_PivotX");
    private static readonly int PivotY = Shader.PropertyToID("_PivotY");

    private float m_TimeSliceAngle;
    private readonly int BossCutTime = Shader.PropertyToID("_BossCutTime");

    private void Awake()
    {
        m_Cam = Camera.main;
        m_CamMgr = m_Cam.GetComponent<CameraMgr>();
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();

        m_ScreenCaptureCanvas = instance.m_ScreenCaptureCanvas;
        m_BulletTimeMgr = instance.p_BulletTimeMgr.GetComponent<BulletTimeMgr>();
        
        var CanvasOnCam = instance.m_ScreenCaptureCanvas;
        m_LImg = CanvasOnCam.m_LImg;
        m_RImg = CanvasOnCam.m_RImg;
        
        m_ScreenShotTex = new Texture2D(width, height, TextureFormat.ARGB32, false);
        m_ScreenRect = new Rect(0, 0, width, height);
        
       
        m_BulletTimeMgr = instance.GetComponentInChildren<BulletTimeMgr>();
    }

    /*
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Vector2 sans = GameMgr.GetInstance().p_PlayerMgr.GetPlayer()
                .GetPlayerCenterPos();
            
            Capture(sans.x, sans.y, 30f);
        }
    }
    */

    public void Capture(float _xPos, float _yPos, float _timeSliceAngle)
    {
        m_TimeSliceAngle = _timeSliceAngle;
        m_CamRenderTex = new RenderTexture(width, height, 24);
        
        m_Cam.targetTexture = m_CamRenderTex;
        m_Cam.Render();
        
        RenderTexture.active = m_CamRenderTex;
        m_ScreenShotTex.ReadPixels(m_ScreenRect,0,0);
        m_ScreenShotTex.Apply();
        m_ScreenShotTex.filterMode = FilterMode.Point;
        Sprite CapturedSprite = Sprite.Create(m_ScreenShotTex, new Rect(0, 0, width, height),
            new Vector2(0.5f, 0.5f));

        m_LImg.sprite = CapturedSprite;
        m_RImg.sprite = CapturedSprite;
        
        RenderTexture.active = null;
        m_Cam.targetTexture = null;
        Destroy(m_CamRenderTex);

        Color color = Color.white;

        m_LImg.material = m_LeftMat;
        m_RImg.material = m_RightMat;
        
        // 쉐이더 0.785가 왼쪽위 오른아래
        // OBJ 60.7도가 왼쪽위 오른아래
        float angle = _timeSliceAngle * m_AdjustAngle;
        
        m_LImg.material.SetFloat(Rotate, angle);
        m_RImg.material.SetFloat(Rotate, angle);

        float pivotXVal = m_Cam.transform.position.x - _xPos;
        m_LImg.material.SetFloat(PivotX, pivotXVal / m_AdjustX);
        m_RImg.material.SetFloat(PivotX, pivotXVal / m_AdjustX);

        float pivotYVal = -(m_Cam.transform.position.y - _yPos);
        m_LImg.material.SetFloat(PivotY, pivotYVal / m_AdjustY);
        m_RImg.material.SetFloat(PivotY, pivotYVal / m_AdjustY);
        
        
        m_LImg.color = color;
        m_RImg.color = color;


        m_LImg.transform.localPosition = Vector2.zero;
        m_RImg.transform.localPosition = Vector2.zero;

        
        if (!ReferenceEquals(m_TearCoroutine, null))
            StopCoroutine(m_TearCoroutine);
        m_TearCoroutine = StartCoroutine(MoveImg());
        
        if(!ReferenceEquals(m_EffectCoroutine, null))
            StopCoroutine(m_EffectCoroutine);
        m_EffectCoroutine = StartCoroutine(LineEffect(new Vector2(_xPos, _yPos), _timeSliceAngle));
        
        if(!ReferenceEquals(m_BlackImgCoroutine, null))
            StopCoroutine(m_BlackImgCoroutine);
        m_BlackImgCoroutine = StartCoroutine(BlackImgFadeOut());
    }

    private IEnumerator BlackImgFadeOut()
    {
        Color blackColor = Color.black;
        Color endColor = blackColor;
        endColor.a = 0f;
        
        SpriteRenderer blackSprite = m_ScreenCaptureCanvas.m_BlackBackSprite;
        blackSprite.gameObject.SetActive(true);
        blackSprite.color = Color.black;

        float lerpVal = 0f;

        yield return new WaitForSecondsRealtime(p_BlackWaitTime);
        
        while (true)
        {
            blackSprite.color = Color.Lerp(blackColor, endColor, lerpVal);
            lerpVal += Time.unscaledDeltaTime;

            if (lerpVal > 1f)
            {
                lerpVal = 1f;
                blackSprite.color = Color.Lerp(blackColor, endColor, lerpVal);
                break;
            }
            yield return null;
        }

        blackSprite.gameObject.SetActive(false);
        yield break;
    }
    
    private IEnumerator LineEffect(Vector2 _linePos, float _angle)
    {
        Color blueColor = Color.blue;
        Color whiteColor = Color.white;
        
       
        SpriteRenderer lineSprite = m_ScreenCaptureCanvas.m_LineSpriteForEffect;
        Transform lineTransform = lineSprite.transform;
        
        // 머터리얼 초기화
        lineSprite.material.SetFloat(BossCutTime, 0f);
        
        Vector2 lineStartScale = m_ScreenCaptureCanvas.m_LineOriginScale;
        lineStartScale.x *= p_LineXScale;
        Vector2 lineEndScale = m_ScreenCaptureCanvas.m_LineOriginScale;
   
        
        lineSprite.gameObject.SetActive(true);

        lineTransform.localScale = lineStartScale;
        lineTransform.position = _linePos;
        lineTransform.rotation = Quaternion.Euler(0f,0f,_angle);
        
        
        float lerpVal = 0f;
        // Scale First
        while (true)
        {
            yield return null;
            
            if (lerpVal < 1f)
            {
                lerpVal += Time.unscaledDeltaTime * p_LineScaleSpeed;
                lineTransform.localScale = Vector2.Lerp(lineStartScale, lineEndScale, lerpVal);
            }
            else
            {
                lerpVal = 1f;
                lineTransform.localScale = Vector2.Lerp(lineStartScale, lineEndScale, lerpVal);
                break;
            }
        }

        lerpVal = 0f;
        // Mat Last
        while (true)
        {
            yield return null;
            
            if (lerpVal < 1f)
            {
                lerpVal += Time.unscaledDeltaTime * p_LineMatSpeed;
                lineSprite.material.SetFloat(BossCutTime, lerpVal);
            }
            else
            {
                lerpVal = 1f;
                lineSprite.material.SetFloat(BossCutTime, lerpVal);
                break;
            }
        }

        yield break;
    }
    
    private IEnumerator MoveImg()
    {
        m_BulletTimeMgr.ChangeTimeScale(0f);
        
        float deltaTime = 0f;
        float forLValue = 0f;
        float forRValue = 0f;

        float Timer = 0f;
        float evaluated = 0f;

        float LerpVal = 0f;
        Vector2 LOriginPos = m_LImg.transform.localPosition;
        Vector2 LLerpPos = LOriginPos;
        LLerpPos.x -= m_InitLerpedPos;

        Vector2 ROriginPos = m_RImg.transform.localPosition;
        Vector2 RLerpPos = ROriginPos;
        RLerpPos.x += m_InitLerpedPos;

        while (true)
        {
            deltaTime = Time.unscaledDeltaTime;
            Timer += deltaTime;
            
            evaluated = m_Curve.Evaluate(Timer);
            LerpVal = evaluated;
            
            m_LImg.transform.localPosition = Vector2.Lerp(LOriginPos, LLerpPos, LerpVal);
            m_RImg.transform.localPosition = Vector2.Lerp(ROriginPos, RLerpPos, LerpVal);
            
            
            if (LerpVal > 1f)
            {
                break;
            }
            yield return null;
        }
        m_MoveImgEndAction?.Invoke();
        m_BulletTimeMgr.ChangeTimeScale(1f);
        yield break;
    }
}