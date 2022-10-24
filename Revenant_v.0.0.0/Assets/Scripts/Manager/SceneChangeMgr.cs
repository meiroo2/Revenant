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
    
    
    // Member Variables
    private Canvas m_MainCanvas = null;
    private Color m_Color;
    private Image m_InstantiatedImg = null;

    private Coroutine m_ColorCoroutine;

    private float m_Speed = 0f;
    private bool m_LastSceneSmoothRequest = false;

    // Constructor
    private void Awake()
    {
        Debug.Log("SceneChangeMgr Awake");
    }

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
        
        SceneManager.LoadScene(_idx);
        m_Color.a = 1f;
        
        Canvas mainCanvas = GetMainCanvas();
        Debug.Log(m_InstantiatedImg);
        if (ReferenceEquals(mainCanvas, null))
        {
            SceneManager.LoadScene(_idx);
            yield break;
        }
        else
        {
            m_Speed = _speed;
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

        m_ColorCoroutine = StartCoroutine(CalSmoothLoad(m_Speed));
    }

    private IEnumerator CalSmoothLoad(float _speed)
    {
        m_Color = Color.black;
        m_Color.a = 1f;
        while (true)
        {
            m_InstantiatedImg.color = m_Color;
            m_Color.a -= Time.unscaledDeltaTime;

            /*
            if (m_Color.a <= 0f)
            { 
                Debug.Log("Sans");
                break;
            }
            */
            
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