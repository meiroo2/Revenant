using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AimCursor : MonoBehaviour
{
    // Member Variables
    private RectTransform m_rect;

    public GameObject m_bullet;
    public GameObject m_gun;

    // Constructors
    private void Awake()
    {
        m_rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        m_rect.position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(m_bullet);
            bullet.transform.SetPositionAndRotation(m_gun.transform.position, m_gun.transform.rotation);
        }
    }
}
