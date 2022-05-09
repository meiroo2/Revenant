using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NuKimPyo : MonoBehaviour
{
    private Image m_Image;
    private int m_Phase = 0;

    public float m_Speed = 0.1f;

    private float m_Timer = 1f;

    private void Awake()
    {
        m_Image = GetComponent<Image>();
    }

    void Update()
    {
        switch (m_Phase)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.H))
                {
                    m_Image.fillAmount = 0;
                    m_Phase = 1;
                }

                break;
            case 1:
                m_Image.fillAmount += Time.deltaTime * m_Speed;
                if (m_Image.fillAmount == 1)
                {
                    m_Phase = 2;
                }
                break;

            case 2:
                m_Timer -= Time.deltaTime;
                if(m_Timer <= 0f)
                {
                    m_Phase = 0;
                    m_Timer = 1f;
                    m_Image.fillAmount = 0;
                }
                break;
        }
    }
}
