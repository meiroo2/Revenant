using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPuller : MonoBehaviour
{
    // Visible Member Variables
    public int m_ObjPullCount = 1;
    public GameObject m_PullingObject;
    public float m_DestroyTimer = 0.8f;

    // Member Variables
    protected int m_Idx = 0;
    protected ForObjPull_Once[] m_PulledObjArr;

    // Constructors
    private void Awake()
    {
        m_PulledObjArr = new ForObjPull_Once[m_ObjPullCount];
        for (int i = 0; i < m_ObjPullCount; i++)
        {
            m_PulledObjArr[i] = Instantiate(m_PullingObject, transform).GetComponent<ForObjPull_Once>();
            m_PulledObjArr[i].gameObject.SetActive(false);
        }
    }

    // Updates
    private void Update()
    {

    }

    // Physics

    // Functions
    public virtual void EnableNewObj()
    {
        m_PulledObjArr[m_Idx].gameObject.SetActive(true);
        m_PulledObjArr[m_Idx].resetTimer(m_DestroyTimer);
        m_Idx++;
        if (m_Idx >= m_ObjPullCount)
            m_Idx = 0;
    }

    // 기타 분류하고 싶은 것이 있을 경우
}
