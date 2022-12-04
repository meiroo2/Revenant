using System;
using System.Collections;
using UnityEngine;



public class ShieldMatTest : MonoBehaviour, IHotBox
{
    public BoxCollider2D m_Collider;
    public SpriteRenderer m_Renderer;
    public GameObject m_ParentObj { get; set; }
    public int m_hotBoxType { get; set; }
    public bool m_isEnemys { get; set; }
    public HitBoxPoint m_HitBoxInfo { get; set; }

    private Vector2 m_LeftBottom;
    private Vector2 m_RightTop;
    private static readonly int HitTime = Shader.PropertyToID("_Hit_Time");
    private static readonly int HitXY = Shader.PropertyToID("_Hit_XY");

    private Coroutine m_Coroutine = null;
    
    private void Awake()
    {
        // 좌하단과 우상단 계산 완료
        m_LeftBottom = new Vector2(transform.position.x  - (m_Collider.size.x / 2f),
            transform.position.y - (m_Collider.size.y / 2f));
        
        m_RightTop = m_LeftBottom;
        m_RightTop.x += m_Collider.size.x;
        m_RightTop.y += m_Collider.size.y;
        
        m_Renderer.material.SetFloat(HitTime, 
            1f);
    }

    public int HitHotBox(IHotBoxParam _param)
    {
        m_Renderer.material.SetVector(HitXY, 
            GetShaderPos(_param.m_contactPoint));

        if (!ReferenceEquals(m_Coroutine, null))
        {
            StopCoroutine(m_Coroutine);
            m_Coroutine = null;
            
        }
        
        m_Coroutine = StartCoroutine(SANSMAT());
        
        return 1;
    }

    private IEnumerator SANSMAT()
    {
        float shaderVal = 0f;

        while (true)
        {
            m_Renderer.material.SetFloat(HitTime, shaderVal);
            shaderVal += Time.deltaTime * 1f;

            if (shaderVal > 1f)
            {
                shaderVal = 1f;
                m_Renderer.material.SetFloat(HitTime, shaderVal);
                break;
            }
            yield return null;
        }

        yield break;
    }
    
    private Vector2 GetShaderPos(Vector2 _worldHit)
    {
        Vector2 returnPos;

        returnPos = _worldHit - m_LeftBottom;

        if (returnPos.x > 1f)
        {
            returnPos.x = 1f;
        }
        else if (returnPos.x < 0f)
        {
            returnPos.x = 0f;
        }


        if (returnPos.y > 1f)
        {
            returnPos.y = 1f;
        }
        else if (returnPos.y < 0f)
        {
            returnPos.y = 0f;
        }
        
        return returnPos;
    }
}