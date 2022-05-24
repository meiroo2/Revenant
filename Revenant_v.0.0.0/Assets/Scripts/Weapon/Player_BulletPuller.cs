using System;
using UnityEngine;

/*
public class Player_BulletPuller : BulletPuller
{
    // Member Variables
    private AimCursor m_AimCursor;
    
    // Constructors
    private void Awake()
    {
        m_Idx = 0;
        m_isPlayers = true;
        m_PulledBulletArr = new Bullet[p_BulletPullCount];
        
        for (var i = 0; i < p_BulletPullCount; i++)
        {
            m_PulledBulletArr[i] = Instantiate(p_PullingBullet, transform).GetComponent<Bullet>();
            m_PulledBulletArr[i].transform.parent = transform;
            m_PulledBulletArr[i].gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        m_AimCursor = InstanceMgr.GetInstance().GetComponentInChildren<AimCursor>();
        m_HitSFXMaker = InstanceMgr.GetInstance().GetComponentInChildren<HitSFXMaker>();

        for (var i = 0; i < p_BulletPullCount; i++)
        {
            m_PulledBulletArr[i].m_HitSFXMaker = m_HitSFXMaker;
        }
    }
    
    // Functions
    public override void MakeBullet(bool _isRightHeaded, Vector2 _Position, Quaternion _Rotation, float _Speed,
        int _Damage)
    {
        m_PulledBulletArr[m_Idx].gameObject.SetActive(false);
        
        m_PulledBulletArr[m_Idx].transform.SetPositionAndRotation(_Position, _Rotation);
        m_PulledBulletArr[m_Idx].transform.localScale = _isRightHeaded ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);

        m_PulledBulletArr[m_Idx].gameObject.SetActive(true);
        m_PulledBulletArr[m_Idx].InitBullet(m_isPlayers, _Speed, _Damage, m_AimCursor.AimedObjid);
        
        m_Idx++;
        if (m_Idx >= p_BulletPullCount)
            m_Idx = 0;
    }
}
*/