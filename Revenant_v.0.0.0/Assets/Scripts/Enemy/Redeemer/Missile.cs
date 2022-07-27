using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class Missile : MonoBehaviour
{
    // Member Variables
    private Transform m_PlayerTransform;
    private Transform m_MissileTransform;
    
    private Rigidbody2D m_Rigid;
    private BoxCollider2D m_Col;

    private Coroutine m_Coroutine;
    private Vector2 velocity;

    // Constructors
    private void Awake()
    {
        m_Rigid = GetComponent<Rigidbody2D>();
        m_Col = GetComponent<BoxCollider2D>();

        m_MissileTransform = transform;
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_PlayerTransform = instance.GetComponentInChildren<Player_Manager>().m_Player.p_Player_RealPos;

        m_Coroutine = StartCoroutine(MissileStart());
    }
    
    


    // Functions
    private IEnumerator MissileStart()
    {
        while (true)
        {
            m_Rigid.velocity = transform.right * 1.5f;

            if (m_PlayerTransform.position.y < m_MissileTransform.position.y)
                break;
            
            yield return new WaitForFixedUpdate();
        }
        
        m_Rigid.gravityScale = 0f;
        velocity = m_Rigid.velocity;

        float a = 1;
        while (true)
        {
            a -= Time.deltaTime * 0.5f;
            m_Rigid.velocity =  m_Rigid.velocity * a;

            if (a <= 0.8f)
            {
                m_Rigid.velocity = Vector2.zero;
                break;
            }

            yield return new WaitForFixedUpdate();
        }
        Debug.Log("Enmd");
        m_Coroutine = StartCoroutine(MissileRotate());
    }

    private IEnumerator MissileRotate()
    {
        Debug.Log("asd");

        m_MissileTransform.rotation = Quaternion.Euler(0,0,GetPlayerLookAngle());
        
        while (true)
        {
            m_Rigid.velocity = transform.right * 5f;

            yield return new WaitForFixedUpdate();
        }
        
    }

    private float GetPlayerLookAngle()
    {
        Vector2 playerPos = m_PlayerTransform.position;
        Vector2 pos = transform.position;
        Vector2 distance = new Vector2(playerPos.x - pos.x, playerPos.y - pos.y);

        return Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
    }
}