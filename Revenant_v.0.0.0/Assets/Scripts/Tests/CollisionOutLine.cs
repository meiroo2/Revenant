using System;
using Unity.VisualScripting;
using UnityEngine;


public class CollisionOutLine : MonoBehaviour
{
    private SpriteOutline m_Outline;

    private void Awake()
    {
        m_Outline = GetComponentInParent<SpriteOutline>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        m_Outline.outlineSize = 1;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        m_Outline.outlineSize = 0;
    }
}