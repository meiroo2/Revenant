using System;
using System.Collections;
using System.Security.Cryptography;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneChangeMgr : MonoBehaviour
{
    // Visible Member Variables
    public Image m_BlackImg;
    public float m_BlackStopTime = 1f;
    public float m_WhitenerSpeed = 2f;
    
    
    // Member Variables
    private Canvas m_MainCanvas = null;
    private Color m_Color;
    private Image m_InstantiatedImg = null;

    private Coroutine m_ColorCoroutine;
    
    private bool m_LastSceneSmoothRequest = false;

    
    // Constructor
    private void Awake()
    {
        Debug.Log("SceneChangeMgr Awake");
        CheckSmoothSceneChange();
    }

    
    /// <summary>
    /// SceneChangeMgr이 OnSceneloaded시 호출되는 함수입니다.
    /// 이전 신에서 스무스 로드 요청이 bool값으로 있었을 경우 함수를 호출합니다.
    /// </summary>
    public void CheckSmoothSceneChange()
    {
        m_MainCanvas = null;

        Debug.Log("SceneChangeMgr OnSceneLoaded");

        if (!ReferenceEquals(m_ColorCoroutine, null))
        {
            StopCoroutine(m_ColorCoroutine);
            m_ColorCoroutine = null;
        }
        
        GameObject mainCanvasObj = GameObject.FindGameObjectWithTag("MainCanvas");

        if (ReferenceEquals(mainCanvasObj, null))
        {
            Debug.Log("ERR : SceneChangeMgr에서 MainCanvas 발견 못함");
        }
        else
        {
            if (mainCanvasObj.TryGetComponent(out Canvas canvas))
            {
                m_MainCanvas = canvas;
                
                
                if (m_LastSceneSmoothRequest)
                {
                    LoadSceneWithSmooth();
                }
            }
        }

        m_LastSceneSmoothRequest = false;
    }

    /// <summary>
    /// 원하는 Idx의 신으로 스무스한 신 전환을 시작합니다.
    /// </summary>
    /// <param name="_idx">신 Idx</param>
    /// <param name="_speed">전환 스피드</param>
    public void InitSceneEndWithSmooth(int _idx, float _speed)
    {
        if (ReferenceEquals(m_MainCanvas, null))
        {
            SceneManager.LoadScene(_idx);
            return;
        }
        
        if (!ReferenceEquals(m_ColorCoroutine, null))
        {
            StopCoroutine(m_ColorCoroutine);
            m_ColorCoroutine = null;
        }

        m_InstantiatedImg = GameObject.Instantiate(m_BlackImg.gameObject, 
            m_MainCanvas.transform).GetComponent<Image>();
        m_InstantiatedImg.transform.localPosition = Vector3.zero;
        
        m_ColorCoroutine = StartCoroutine(CalSceneChange(_idx, _speed));
    }

    
    /// <summary>
    /// 화면 까매짐을 시작하는 IEnumerator입니다.
    /// </summary>
    /// <param name="_idx"></param>
    /// <param name="_speed"></param>
    /// <returns></returns>
    private IEnumerator CalSceneChange(int _idx, float _speed)
    {
        m_Color = Color.black;
        m_Color.a = 0f;
        while (true)
        {
            m_InstantiatedImg.color = m_Color;
            m_Color.a += Time.unscaledDeltaTime * _speed;

            if (m_Color.a >= 1f)
                break;
            
            yield return null;
        }
        
        m_Color.a = 1f;
        m_InstantiatedImg.color = m_Color;

        yield return new WaitForSecondsRealtime(m_BlackStopTime);
        SceneManager.LoadScene(_idx);

        Canvas mainCanvas = GetMainCanvas();
        Debug.Log(m_InstantiatedImg);
        if (ReferenceEquals(mainCanvas, null))
        {
            SceneManager.LoadScene(_idx);
            yield break;
        }
        else
        {
            m_LastSceneSmoothRequest = true;
        }
        
        yield break;
    }

    private void LoadSceneWithSmooth()
    {
        if (ReferenceEquals(m_MainCanvas, null))
            return;

        m_InstantiatedImg = null;

        m_InstantiatedImg = GameObject.Instantiate(m_BlackImg, m_MainCanvas.transform);
        m_InstantiatedImg.transform.localPosition = Vector3.zero;
        
        
        if(!ReferenceEquals( m_ColorCoroutine, null))
        {
            StopCoroutine( m_ColorCoroutine);
            m_ColorCoroutine = null;
        }

        m_ColorCoroutine = StartCoroutine(CalSmoothLoad(m_WhitenerSpeed));
    }

    private IEnumerator CalSmoothLoad(float _speed)
    {
        m_Color = Color.black;
        m_Color.a = 1f;
        while (true)
        {
            m_InstantiatedImg.color = m_Color;
            m_Color.a -= Time.unscaledDeltaTime;

            yield return null;
        }

        yield break;
    }
    
    private Canvas GetMainCanvas()
    {
        GameObject mainCanvasObj = GameObject.FindGameObjectWithTag("MainCanvas");

        if (ReferenceEquals(mainCanvasObj, null))
        {
            Debug.Log("ERR : SceneChangeMgr에서 MainCanvas 발견 못함");
        }
        else
        {
            if (mainCanvasObj.TryGetComponent(out Canvas canvas))
            {
                return canvas;
            }
        }

        return null;
    }
}