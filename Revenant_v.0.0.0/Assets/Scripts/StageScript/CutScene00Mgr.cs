using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutScene00Mgr : MonoBehaviour
{
    private float m_Timer = 5f;

    private int m_Phase = 0;
    public SpriteRenderer m_BlackBox;

    public string m_NextSceneName;

    private Color m_BlackBoxColor;

    private void Awake()
    {
        m_BlackBoxColor = new Color(1, 1, 1, 1);
        m_BlackBox.color = m_BlackBoxColor;
    }

    private void Update()
    {
        switch (m_Phase)
        {
            case 0:
                if (m_BlackBoxColor.a >= 0f)
                {
                    m_BlackBoxColor.a -= Time.deltaTime * 0.5f;
                    m_BlackBox.color = m_BlackBoxColor;
                }
                else
                    m_Phase++;
                break;

            case 1:
                m_Timer -= Time.deltaTime;
                if (m_Timer <= 0f)
                    m_Phase++;
                break;

            case 2:
                m_BlackBoxColor.a += Time.deltaTime * 0.5f;
                if (m_BlackBoxColor.a >= 1f)
                    SceneManager.LoadScene(m_NextSceneName);
                break;
        }
    }
}
