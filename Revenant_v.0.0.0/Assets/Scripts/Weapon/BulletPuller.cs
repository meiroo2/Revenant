using System.Collections.Generic;
using UnityEngine;



public class BulletPuller : MonoBehaviour
{
    // Visible Member Variables
    public int p_BulletPullCount = 0;
    public GameObject p_PullingBullet;
    protected bool m_isPlayers = false;


    // Member Variables
    protected int m_Idx = 0;
    protected List<Bullet> m_PulledBulletArr = new();
    protected HitSFXMaker m_HitSFXMaker;
    private AimCursor m_AimCursor;


    // Constructors
    private void Awake()
    {
        m_Idx = 0;
        m_isPlayers = true;
		//m_PulledBulletArr = new Bullet[p_BulletPullCount];
		for (var i = 0; i < p_BulletPullCount; i++)
        {
            Bullet bullet = Instantiate(p_PullingBullet, transform).GetComponent<Bullet>();
			m_PulledBulletArr.Add(bullet);
		}

		for (var i = 0; i < p_BulletPullCount; i++)
        {
            m_PulledBulletArr[i].gameObject.transform.parent = transform;
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
    public virtual void MakeBullet(BulletParam _param)
    {
        m_PulledBulletArr[m_Idx].gameObject.SetActive(false);
        m_PulledBulletArr[m_Idx].gameObject.SetActive(true);
        m_PulledBulletArr[m_Idx].InitBullet(_param);

        m_Idx++;
        if (m_Idx >= p_BulletPullCount)
            m_Idx = 0;
    }
}