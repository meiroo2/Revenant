using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellMgr : ObjectPuller
{
    private Rigidbody2D[] m_PulledObjRigidArr;
    private SoundMgr_SFX m_SoundMgrSFX;

    private void Awake()
    {
        m_PulledObjArr = new ForObjPull_Once[m_ObjPullCount];
        for (int i = 0; i < m_ObjPullCount; i++)
        {
            m_PulledObjArr[i] = Instantiate(m_PullingObject, transform).GetComponent<ForObjPull_Once>();
            m_PulledObjArr[i].gameObject.SetActive(false);
        }

        m_PulledObjRigidArr = new Rigidbody2D[m_ObjPullCount];
        for (int i = 0; i < m_ObjPullCount; i++)
        {
            m_PulledObjRigidArr[i] = m_PulledObjArr[i].GetComponent<Rigidbody2D>();
        }
    }
    private void Start()
    {
        m_SoundMgrSFX = InstanceMgr.GetInstance().GetComponentInChildren<SoundMgr_SFX>();
        for (int i = 0; i < m_ObjPullCount; i++)
        {
            m_PulledObjArr[i].GetComponent<ForObjPull_Once>().m_SoundSFXMgr = m_SoundMgrSFX;
        }
    }

    public void MakeShell(Vector2 _Spawnpos, Vector2 _PowerDirection)
    {
        m_PulledObjRigidArr[m_Idx].velocity = Vector2.zero;

        m_PulledObjArr[m_Idx].transform.position = _Spawnpos;
        EnableNewObj();
        m_PulledObjRigidArr[m_Idx - 1].AddForce(_PowerDirection, ForceMode2D.Impulse);
    }
}