using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Visible Member Variables


    // Member Variables
    private SpriteRenderer m_Renderer;
    private IHotBox m_BulletHitHotBox;
    public HitSFXMaker m_HitSFXMaker;

    private bool m_isPlayers;
    private float m_MaxDeadTimer = 3f;
    private int m_Damage;
    private float m_Speed;
    private int m_StunValue;
    private int m_IsRightHeaded;
    private int m_AimedObjID = -1;

    private bool m_ShouldDestroy = false;
    
    
    // Constructor
    private void Awake()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        StopCoroutine(StartBulletDeadTimer());
        StartCoroutine(StartBulletDeadTimer());

        m_IsRightHeaded = (int)transform.localScale.x;
        m_BulletHitHotBox = null;
        m_ShouldDestroy = false;
    }

    public void InitBullet(bool _isPlayers, float _Speed, int _damage)  // For Enemy
    {
        if (_isPlayers == true)
            return;

        m_isPlayers = false;
        m_Speed = _Speed;
        m_Damage = _damage;
    }

    public void InitBullet(bool _isPlayers, float _Speed, int _damage, int _aimID)  // For Player
    {
        if (_isPlayers == false)
            return;

        m_isPlayers = true;
        m_Speed = _Speed;
        m_Damage = _damage;
        m_AimedObjID = _aimID;
    }

    
    // Updates
    private void Update()
    {
        transform.Translate(Vector2.right * (m_IsRightHeaded * Time.deltaTime * m_Speed));
    }

    
    // Functions
    private void OnTriggerEnter2D(Collider2D _col)
    {
        m_BulletHitHotBox = _col.GetComponent<IHotBox>();
        
        if (m_isPlayers)
        {
            CalculateBulletForPlayer(ref _col);
        }
        else
        {
            CalculateBulletForEnemy(ref _col);
        }
        
        if(m_ShouldDestroy)
            gameObject.SetActive(false);
    }

    private IEnumerator StartBulletDeadTimer()
    {
        yield return new WaitForSeconds(m_MaxDeadTimer);
        gameObject.SetActive(false);
    }

    private void CalculateBulletForEnemy(ref Collider2D col)
    {
        switch (m_BulletHitHotBox.m_hotBoxType)
        {
            case 0:
                // 적은 조준하지 않음(그냥 쏨)
                break;
            
            case 1:
                Debug.Log("적 총알 충돌 : " + col.name + " " + col.gameObject.GetInstanceID());

                m_BulletHitHotBox.HitHotBox(new IHotBoxParam(m_Damage, m_StunValue, transform.position, WeaponType.BULLET));
                m_HitSFXMaker.EnableNewObj(UnityEngine.Random.Range(1, 3), transform.position, transform.rotation,
                    (m_IsRightHeaded > 0) ? true : false);
                m_ShouldDestroy = true;
                break;
        }
    }

    private void CalculateBulletForPlayer(ref Collider2D col)
    {
        switch (m_BulletHitHotBox.m_hotBoxType)
        {
            case 0: // 조준한 오브젝트(적 등)
                if (m_AimedObjID != col.gameObject.GetInstanceID())
                {
                    Debug.Log("지나감 : " + col.name + " " + col.gameObject.GetInstanceID());
                    break;
                }

                Debug.Log("충돌 : " + col.name + " " + col.gameObject.GetInstanceID());
                m_BulletHitHotBox.HitHotBox(new IHotBoxParam(m_Damage, m_StunValue, transform.position, WeaponType.BULLET));
                m_HitSFXMaker.EnableNewObj(UnityEngine.Random.Range(1, 3), transform.position, transform.rotation,
                    (m_IsRightHeaded > 0) ? true : false);
                m_ShouldDestroy = true;
                break;

            case 1: // 반드시 충돌해야 하는 오브젝트(벽 등)
                Debug.Log("반드시 충돌 : " + col.name + " " + col.gameObject.GetInstanceID());

                m_BulletHitHotBox.HitHotBox(new IHotBoxParam(m_Damage, m_StunValue, transform.position, WeaponType.BULLET));
                m_HitSFXMaker.EnableNewObj(UnityEngine.Random.Range(1, 3), transform.position, transform.rotation,
                    (m_IsRightHeaded > 0) ? true : false);
                m_ShouldDestroy = true;
                break;
        }
    }
}