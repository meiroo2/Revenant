using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    private Player m_Player;
    private Vector2 m_OriginPos;

    public SpriteRenderer m_BlackSPrite;

    private Color m_BlackColor;

    private int m_isBulletTime = 0;

    private void Start()
    {
        m_BlackColor = new Color(0, 0, 0, 0);
        m_OriginPos = transform.position;
        m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            transform.position = m_OriginPos;
            m_isBulletTime = 0;
            Time.timeScale = 1f;
        }

        if((Vector2.Distance(m_Player.transform.position, transform.position) <= 0.8f) && m_isBulletTime == 0)
        {
            m_isBulletTime = 1;
            Time.timeScale = 0f;
            
        }

        if (Input.GetKeyDown(KeyCode.Space) && m_isBulletTime == 1)
        {
            Time.timeScale = 1f;
            m_isBulletTime = 2;
        }

        if (m_isBulletTime == 1)
        {
            if(m_BlackColor.a <= 0.8f)
            {
                m_BlackColor.a += Time.unscaledDeltaTime * 1.5f;
                m_BlackSPrite.color = m_BlackColor;
            }
        }
        else if(m_isBulletTime == 2)
        {
            if (m_BlackColor.a >= 0f)
                m_BlackColor.a -= Time.unscaledDeltaTime * 1.5f;
            m_BlackSPrite.color = m_BlackColor;
        }

        if (m_isBulletTime == 1)
        {
            if (Vector2.Distance(m_Player.transform.position, transform.position) >= 0.3f){
                transform.Translate(Vector2.left * Time.unscaledDeltaTime * 0.3f);
            }
        }
        else
            transform.Translate(Vector2.left * Time.deltaTime * 6f);
    }

    private void FixedUpdate()
    {
        
    }
}
