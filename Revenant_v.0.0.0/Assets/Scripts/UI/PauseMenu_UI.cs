using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;


public class PauseMenu_UI : MonoBehaviour
{
    public float p_Speed = 1f;
    public Image[] p_ImgArr;
    
    private bool m_IsOn = false;
    private float[] m_AlphaValArr;

    private Dictionary<Image, float> m_ImageDic = new Dictionary<Image, float>();
    private Color m_ClearWhiteColor;

    private Coroutine[] m_CoroutineArr;

    private void Awake()
    {
        m_CoroutineArr = new Coroutine[7];
        for (int i = 0; i < m_CoroutineArr.Length; i++)
        {
            m_CoroutineArr[i] = null;
        }
        
        m_ClearWhiteColor = Color.white;
        m_ClearWhiteColor.a = 0f;
        
        for (int i = 0; i < p_ImgArr.Length; i++)
        {
            m_ImageDic.Add(p_ImgArr[i], p_ImgArr[i].color.a);
        }

        foreach (var VARIABLE in m_ImageDic)
        {
            VARIABLE.Key.gameObject.SetActive(false);
        }
    }
    
    [Button]
    public void ActivatePauseMenu(bool _isOn)
    {
        m_IsOn = _isOn;

        var imgArr = m_ImageDic.Keys.ToArray();
        
        for (int i = 0; i < imgArr.Length; i++)
        {
            if (!ReferenceEquals(m_CoroutineArr[i], null))
            {
                StopCoroutine(m_CoroutineArr[i]);
                m_CoroutineArr[i] = null;
            }
                
            imgArr[i].gameObject.SetActive(true);
            m_CoroutineArr[i] = StartCoroutine(AlphaFade(m_IsOn, imgArr[i],
                m_ImageDic[imgArr[i]]));
        }
    }

    private IEnumerator AlphaFade(bool _toAppear, Image _image, float _originAlpha)
    {
        Color startColor = m_ClearWhiteColor;
        Color endColor = m_ClearWhiteColor;

        if (_toAppear)
        {
            endColor.a = _originAlpha;
        }
        else
        {
            startColor.a = _originAlpha;
            endColor.a = 0f;
        }

        float lerpVal = 0f;

        while (true)
        {
            if (lerpVal >= 1f)
            {
                lerpVal = 1f;
                _image.color = Color.Lerp(startColor, endColor, lerpVal);
                break;
            }
            
            _image.color = Color.Lerp(startColor, endColor, lerpVal);
            lerpVal += Time.deltaTime * p_Speed;
            
            yield return null;
        }

        if(!_toAppear)
            _image.gameObject.SetActive(true);
        
        yield break;
    }
}