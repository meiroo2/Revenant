using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class Wdn : MonoBehaviour
{
    public GameObject m_WhiteBox;
    public GameObject m_WdnEffect;
    public ScriptUIMgr m_ScriptUIMgr;
    public SpriteRenderer m_BlackSquare;
    public float SpeedVal = 2f;

    public int Phase = 0;

    public GameObject Mark00;
    public GameObject Mark01;
    public GameObject Mark02;

    private Color m_BlackColor;

    private float m_Timer = 0.5f;

    private Animator m_Animator;

    // Start is called before the first frame update
    void Start()
    {
        m_BlackColor = new Color(0, 0, 0, 0);
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            SceneManager.LoadScene(0);
        }

        switch (Phase)
        {
            case 0:
                transform.Translate(Vector2.left * Time.deltaTime * SpeedVal);
                if (Mark00.transform.position.x >= transform.position.x)
                {
                    Phase++;
                    m_BlackColor.a = 1;
                    m_BlackSquare.color = m_BlackColor;
                    m_Timer = 0.7f;
                }
                break;

            case 1:

                m_Timer -= Time.deltaTime;
                if(m_Timer <= 0f)
                {
                    Phase++;
                    m_Timer = 0.5f;
                }
                break;

            case 2:
                transform.position = new Vector2(Mark01.transform.position.x, transform.position.y);
                m_Animator.SetInteger("Phase", 1);
                m_BlackColor.a = 0;
                m_BlackSquare.color = m_BlackColor;
                Phase++;
                break;

            case 3:
                m_Timer -= Time.deltaTime;

                    Phase++;
                    m_BlackColor.a = 1;
                    m_BlackSquare.color = m_BlackColor;
                    m_Timer = 1.05f;
  
                break;

            case 4:
                m_Timer -= Time.deltaTime;
                if(m_Timer <= 0f)
                {
                    transform.position = new Vector2(Mark02.transform.position.x, transform.position.y);
                    transform.localScale = new Vector2(1, 1);
                    m_BlackColor.a = 0;
                    m_BlackSquare.color = m_BlackColor;
                    Phase++;
                }
                break;

            case 5:
                m_Animator.SetInteger("Phase", 2);
                Phase++;
                break;

            case 6:
                m_ScriptUIMgr.NextScript(0, true);
                m_WdnEffect.SetActive(true);
                Phase++;
                break;

            case 7:
                if(m_ScriptUIMgr.m_isPlaying == false)
                {
                    Phase++;
                }
                break;

            case 8:
                m_WhiteBox.SetActive(true);
                Phase++;
                m_Timer = 3f;
                break;

            case 9:
                m_Timer -= Time.deltaTime;
                if (m_Timer <= 0f)
                    Phase++;
                break;

            case 10:
                SceneManager.LoadScene("CutScene2");
                break;
        }

    }
}
