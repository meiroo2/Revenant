using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BulletMgr : MonoBehaviour
{
    // Visible Member Variables
    public int m_ObjPullCount = 1;
    public GameObject m_PullingObject;

    // Member Variables
    private int m_Idx = 0;
    private Player_Bullet[] m_PulledBulletArr;
    private HitSFXMaker m_HitSFXMaker;
    private SoundMgr_SFX m_SoundMgrSFX;
    private AimCursor m_AimCursor;

    // Constructors
    private void Awake()
    {
        m_PulledBulletArr = new Player_Bullet[m_ObjPullCount];
        for (int i = 0; i < m_ObjPullCount; i++)
        {
            m_PulledBulletArr[i] = Instantiate(m_PullingObject, transform).GetComponent<Player_Bullet>();
            m_PulledBulletArr[i].gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        m_HitSFXMaker = InstanceMgr.GetInstance().GetComponentInChildren<HitSFXMaker>();
        m_SoundMgrSFX = InstanceMgr.GetInstance().GetComponentInChildren<SoundMgr_SFX>();
        m_AimCursor = InstanceMgr.GetInstance().GetComponentInChildren<AimCursor>();

        for (int i = 0; i < m_ObjPullCount; i++)
        {
            m_PulledBulletArr[i].m_HitSFXMaker = m_HitSFXMaker;
            m_PulledBulletArr[i].m_SoundMgrSFX = m_SoundMgrSFX;
        }
    }

    public void MakeBullet(bool _isRightHeaded, int _Damage, float _Speed,
        Vector2 _Position, Quaternion _Rotation)
    {
        m_PulledBulletArr[m_Idx].gameObject.SetActive(true);

        if (_isRightHeaded)
            m_PulledBulletArr[m_Idx].InitBullet(_Speed, _Damage, m_AimCursor.AimedObjid);
        else
            m_PulledBulletArr[m_Idx].InitBullet(-_Speed, _Damage, m_AimCursor.AimedObjid);

        m_PulledBulletArr[m_Idx].transform.SetPositionAndRotation(_Position, _Rotation);

        m_Idx++;
        if (m_Idx >= m_ObjPullCount)
            m_Idx = 0;
    }
}