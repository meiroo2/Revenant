using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Dynamic : MonoBehaviour
{
    public bool UsingUp = true;
    
    public float m_CurScaleX = 1f;
    public float m_LargeScaleX = 2f;
    
    public float m_MainSpeed = 1f;
    public float m_ExtraSpeed = 1f;
    
    public RectTransform m_Transform;

    public RectTransform m_LPos;
    public RectTransform m_RPos;
    
    public float m_FadeSpeed = 1f;
    public Image m_Image;
    public Text m_Txt;

    public float yVal;
    public float ySpeed;
    public RectTransform Back;
    public Text AlertTxt;
    
    private float m_OriginY;

    private Coroutine m_Coroutine;
    
    private void Awake()
    {
        m_OriginY = m_Transform.localScale.y;
        m_Image.color = new Color(1, 1, 1, 0);
        m_Txt.color = new Color(0, 0, 0, 0);

        AlertTxt.color = new Color(0, 0, 0, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if(!ReferenceEquals(m_Coroutine, null))
                StopCoroutine(m_Coroutine);
            
            m_Coroutine = StartCoroutine(ReturnScale());
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if(!ReferenceEquals(m_Coroutine, null))
                StopCoroutine(m_Coroutine);
            
            m_Coroutine = StartCoroutine(LargeScale());
        }
    }

    private IEnumerator LargeScale()
    {
        Vector2 sans;
        float extra = 0f;
        
        Color color = new Color(1, 1, 1, 0);
        m_Image.color = color;
        m_Txt.color = color;

        Color colorALert = new Color(0, 0, 0, 0);
        
        while (true)
        {
            extra += Time.deltaTime * m_ExtraSpeed;
            
            sans = m_Transform.localScale;
            sans = Vector2.Lerp(sans, new Vector2(m_LargeScaleX, m_OriginY), 
                Time.deltaTime * (m_MainSpeed + extra));
                
            m_Transform.localScale = sans;

            if (UsingUp && Back.localPosition.y - yVal < 0)
            {
                Back.localPosition =
                    new Vector2(Back.localPosition.x, Back.localPosition.y + (Time.deltaTime * ySpeed));

                colorALert.a += Time.deltaTime * m_FadeSpeed;
                AlertTxt.transform.position = Back.position;
                AlertTxt.color = colorALert;
            }
               
            if (Mathf.Abs(m_Transform.localScale.x - m_LargeScaleX) < 0.5f)
            {
                m_Image.rectTransform.position = m_LPos.position;
                m_Txt.rectTransform.position = m_RPos.position;
                m_Image.color = color;
                m_Txt.color = new Color(0, 0, 0, m_Image.color.a);

                color.a += Time.deltaTime * m_FadeSpeed;
            }
            
            if (Mathf.Abs(m_Transform.localScale.x -m_LargeScaleX) < 0.05f)
            {
                break;
            }
            
            yield return null;
        }
        
        m_Image.rectTransform.position = m_LPos.position;
        m_Txt.rectTransform.position = m_RPos.position;

        while (true)
        {
            if (color.a > 1)
                break;
            
            m_Image.color = color;
            m_Txt.color = new Color(0, 0, 0, m_Image.color.a);

            color.a += Time.deltaTime * m_FadeSpeed;
            
            yield return null;
        }

        colorALert.a = 1;
        
        while (true)
        {
            if (!UsingUp)
                break;
            
            
            if (Back.localPosition.y > 0)
            {
                Back.position =
                    new Vector2(Back.position.x, Back.position.y - (Time.deltaTime * ySpeed));

    
                    colorALert.a -= Time.deltaTime * m_FadeSpeed;
                    AlertTxt.transform.position = Back.position;
                    AlertTxt.color = colorALert;

            }
            else
            {
                break;
            }

            yield return null;
        }

        Back.localPosition = Vector2.zero;
    }
    
    
    private IEnumerator ReturnScale()
    {
        Vector2 sans;
        float extra = 0f;

        Color color = new Color(1, 1, 1, 0);
        m_Image.color = color;
        m_Txt.color = color;
        
        while (true)
        {
            extra += Time.deltaTime * m_ExtraSpeed;
            
            sans = m_Transform.localScale;
            sans = Vector2.Lerp(sans, new Vector2(m_CurScaleX, m_OriginY),
                Time.deltaTime * (m_MainSpeed + extra));
                
            m_Transform.localScale = sans;

            
            
            if (Mathf.Abs(m_Transform.localScale.x - m_CurScaleX) < 0.05f)
            {
                break;
            }
            
            yield return null;
        }
    }
}
