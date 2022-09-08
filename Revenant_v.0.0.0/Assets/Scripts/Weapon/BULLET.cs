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

    private BulletParam m_BulletParam;

    private bool m_ShouldDestroy = false;
    
    
    // Constructor
    private void Awake()
    {
        m_Renderer = GetComponentInChildren<SpriteRenderer>();
    }

    
    private void OnEnable()
    {
        StopCoroutine(StartBulletDeadTimer());
        StartCoroutine(StartBulletDeadTimer());
        
        m_BulletHitHotBox = null;
        m_ShouldDestroy = false;
    }

    
    public void InitBullet(BulletParam _param)
    {
        m_BulletParam = (BulletParam)_param.DeepCopy();
        transform.SetPositionAndRotation(m_BulletParam.m_Position, m_BulletParam.m_Rotation);
        transform.localScale = new Vector3(transform.localScale.x * (m_BulletParam.m_IsRightHeaded ? 1 : -1), 1, 1);
        m_Renderer.sprite = m_BulletParam.m_BulletSprite;
    }


    // Updates
    private void Update()
    {
        transform.Translate(Vector2.right * ((m_BulletParam.m_IsRightHeaded ? 1 : -1) * 
                                             Time.deltaTime * m_BulletParam.m_Speed));
    }

    
    // Functions
    private void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.TryGetComponent(out IHotBox hotBox))
        {
            if (m_BulletParam.m_IsPlayers)
            {
                m_BulletHitHotBox = hotBox;
                CalculateBulletForPlayer(ref _col);
            }
            else
            {
                CalculateBulletForEnemy(ref hotBox);
            }
        }

        if(m_ShouldDestroy)
            gameObject.SetActive(false);
    }

    private IEnumerator StartBulletDeadTimer()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    private void CalculateBulletForEnemy(ref IHotBox hotBox)
    {
        if (hotBox.m_isEnemys)
            return;
        
        hotBox.HitHotBox(new IHotBoxParam(m_BulletParam.m_Damage, m_BulletParam.m_StunValue,
            transform.position, WeaponType.BULLET));

        switch (hotBox.m_hotBoxType)
        {
            case 0:
                m_HitSFXMaker.EnableNewObj(0,transform.position);
                m_ShouldDestroy = true;
                break;
            
            case 1:
                m_HitSFXMaker.EnableNewObj(0,transform.position);
                m_ShouldDestroy = true;
                break;
            
            case 2:
                break;
        }
    }

    private void CalculateBulletForPlayer(ref Collider2D col)
    {
        switch (m_BulletHitHotBox.m_hotBoxType)
        {
            case 0: // 조준한 오브젝트(적 등)
                if (m_BulletParam.m_AimedObjID != col.gameObject.GetInstanceID())
                {
                    //Debug.Log("지나감 : " + col.name + " " + col.gameObject.GetInstanceID());
                    break;
                }

                //Debug.Log("충돌 : " + col.name + " " + col.gameObject.GetInstanceID());
                m_BulletHitHotBox.HitHotBox(new IHotBoxParam(m_BulletParam.m_Damage, m_BulletParam.m_StunValue, transform.position, WeaponType.BULLET));
                
                int hitPoint = 0;
                if (m_BulletHitHotBox.m_HitBoxInfo == HitBoxPoint.HEAD)
                    hitPoint = 1;
                else if (m_BulletHitHotBox.m_HitBoxInfo == HitBoxPoint.BODY)
                    hitPoint = 0;
                
                m_HitSFXMaker.EnableNewObj(0,transform.position);
                
                m_ShouldDestroy = true;
                break;

            case 1: // 반드시 충돌해야 하는 오브젝트(벽 등)
                //Debug.Log("반드시 충돌 : " + col.name + " " + col.gameObject.GetInstanceID());

                m_BulletHitHotBox.HitHotBox(new IHotBoxParam(m_BulletParam.m_Damage, m_BulletParam.m_StunValue, transform.position, WeaponType.BULLET));
                m_HitSFXMaker.EnableNewObj(0,transform.position);
                m_ShouldDestroy = true;
                break;
        }
    }
}

public class BulletParam
{
    public bool m_IsPlayers;
    public Sprite m_BulletSprite;
    public bool m_IsRightHeaded;
    public Vector2 m_Position;
    public Quaternion m_Rotation;
    public int m_Damage;
    public float m_Speed;
    public int m_StunValue;
    public int m_AimedObjID;

    public BulletParam(bool _isPlayers, Sprite _sprite, bool _isRight, Vector2 _position,
        Quaternion _rotation, int _damage, float _speed, int _stunValue = 0, int _objID = -1)
    {
        m_IsPlayers = _isPlayers;
        m_BulletSprite = _sprite;
        m_IsRightHeaded = _isRight;
        m_Position = _position;
        m_Rotation = _rotation;
        m_Damage = _damage;
        m_Speed = _speed;

        m_StunValue = _stunValue;
        m_AimedObjID = _objID;
    }

    public object DeepCopy()
    {
        return this.MemberwiseClone();
    }
}

