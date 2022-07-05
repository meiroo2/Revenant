using System;
using System.Collections;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;


public class IconObj : MonoBehaviour
{
    // Member Variables
    private SpriteRenderer m_Renderer;
    private Transform m_Transform;
    private Coroutine m_Coroutine;

    private Vector2 m_CenterPos;
    private float m_AddYValue;


    // Constructors
    private void Awake()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
    }
    
    
    // Functions
    public void SetIconObj(Transform _toAttach, Sprite _sprite)
    {
        m_Renderer.sprite = _sprite;
        m_Transform = _toAttach;

        if (m_Coroutine != null)
        {
            StopCoroutine(m_Coroutine);
        }
        
        m_Coroutine = StartCoroutine(DoMovenAppear());
    }
    private IEnumerator DoMovenAppear()
    {
        var transformPos = m_Transform.position;
        
        m_CenterPos = transformPos;
        transform.position = new Vector2(transformPos.x, transformPos.y + m_AddYValue);

        m_AddYValue += Time.deltaTime * 0.5f;
        yield return null;
    }
}