using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fading : MonoBehaviour
{
    [SerializeField]
    Image m_black;
    
    float a = 1;
    Color alpha;
    public int m_value { get; set; } = 0;

    [SerializeField]
    float m_changeAlpha = 0.001f;

    private void Update()
    {
        switch(m_value)
        {
            case 0: // fade in
                a -= m_changeAlpha;
                alpha = new Color(0, 0, 0, a);
                m_black.color = alpha;
                if (a <= 0)
                {
                    m_value++;
                }
                break;
            case 1: // press key
                if (Input.anyKeyDown)
                {
                    m_value++;
                    Time.timeScale = 0;
                }
                break;
            case 2: // fade out
                a += m_changeAlpha;
                alpha = new Color(0, 0, 0, a);
                m_black.color = alpha;
                if (a >= 1)
                {
                    m_value++;
                    Time.timeScale = 1;
                    SceneManager.LoadScene(0);
                }
                break;
        }
    }

}
