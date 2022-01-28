using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AimCursor : MonoBehaviour
{
    // Member Variables
    private RectTransform m_rect;

    // Constructors
    private void Awake()
    {
        m_rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        m_rect.position = Input.mousePosition;
    }
}
