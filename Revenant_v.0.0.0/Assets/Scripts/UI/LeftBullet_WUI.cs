using System;
using System.Collections;
using UnityEngine;


public class LeftBullet_WUI : WorldUI
{
    // Visible Member Variables
    public float m_LerpSpeed = 1f;
    public Animator m_Animator;
    
    
    // Member Variables
    private Transform m_OriginParentTransform;
    private Vector2 m_LeftGap;
    private Vector2 m_RightGap;
    private Vector2 m_DestPos;
    
    
    private Coroutine m_MoveCoroutine = null;
    private bool m_IsLerping = false;
    private bool m_IsLeft = true;
    
    
    // Constructors
    private void Awake()
    {
        m_OriginParentTransform = transform.parent;
        m_LeftGap = transform.localPosition;
        m_RightGap = new Vector2(-m_LeftGap.x, m_LeftGap.y);
        transform.SetParent(null);
    }

    // Updates
    private void Update()
    {
        if (m_IsLerping)
            return;

        m_DestPos = m_IsLeft ? new Vector2(m_OriginParentTransform.position.x + m_LeftGap.x,
            m_OriginParentTransform.position.y + m_LeftGap.y) : 
            new Vector2(m_OriginParentTransform.position.x + m_RightGap.x, 
                m_OriginParentTransform.position.y + m_RightGap.y);
        
        transform.position = m_DestPos;
    }
    
    // Functions
    public void SetLeftBulletUI(int _leftBullet)
    {
        //Debug.Log(_leftBullet);
        switch (_leftBullet)
        {
            case 0:
                m_Animator.Play("BlankAnim", -1, 0f);
                break;
            
            case 1:
                m_Animator.Play("2Anim", -1, 0f);
                break;
            
            case 2:
                m_Animator.Play("3Anim", -1, 0f);
                break;
            
            case 3:
                m_Animator.Play("4Anim", -1, 0f);
                break;
            
            case 4:
                m_Animator.Play("5Anim", -1, 0f);
                break;
            
            case 5:
                m_Animator.Play("6Anim", -1, 0f);
                break;
            
            case 6:
                m_Animator.Play("7Anim", -1, 0f);
                break;
            
            case 7:
                m_Animator.Play("FullAnim", -1, 0f);
                break;
            
            case 8:
                m_Animator.Play("FullAnim", -1, 0f);
                break;
        }
    }
    
    public void MovetoLeftSide(bool _toLeft)
    {
        m_IsLeft = _toLeft;
        
        if (!ReferenceEquals(m_MoveCoroutine, null))
        {
            StopCoroutine(m_MoveCoroutine);
            m_MoveCoroutine = null;
        }
        
        m_MoveCoroutine = StartCoroutine(CalMove(_toLeft));
    }

    private IEnumerator CalMove(bool _toLeft)
    {
        m_IsLerping = true;
        
        Vector2 originTransformPos;
        if (_toLeft)
        {
            while (true)
            {
                originTransformPos = m_OriginParentTransform.position;
                m_DestPos = new Vector2(originTransformPos.x + m_LeftGap.x, originTransformPos.y + m_LeftGap.y);
                
                transform.position = Vector2.Lerp(transform.position, m_DestPos, 
                    Time.deltaTime * m_LerpSpeed);

                if (Vector2.Distance(transform.position, m_DestPos) < 0.01f)
                {
                    break;
                }
                
                yield return null;
            }
        }
        else
        {
            while (true)
            {
                originTransformPos = m_OriginParentTransform.position;
                m_DestPos = new Vector2(originTransformPos.x + m_RightGap.x, originTransformPos.y + m_RightGap.y);
                
                transform.position = Vector2.Lerp(transform.position, m_DestPos, 
                    Time.deltaTime * m_LerpSpeed);
            
                if (Vector2.Distance(transform.position, m_DestPos) < 0.01f)
                {
                    break;
                }
                
                yield return null;
            }
        }

        m_IsLerping = false;
        
        yield break;
    }
}