using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HitSFXMaker : MonoBehaviour
{
    // Visible Member Variables
    public int m_ObjPullCount = 1;
    public GameObject[] m_PullingObject;
    public float m_DestroyTimer = 0.8f;

    // Member Variables
    private int[] m_Idx;
    private ForObjPull_Once[,] m_PulledObjArr;

    // Constructors
    private void Awake()
    {
        m_Idx = Enumerable.Repeat(0, m_PullingObject.Length).ToArray();

        for(int i = 0; i < m_PullingObject.Length; i++)
        {
            m_PulledObjArr = new ForObjPull_Once[m_PullingObject.Length, m_ObjPullCount];
        }

        for(int i = 0; i <m_PullingObject.Length; i++)
        {
            for (int j = 0; j < m_ObjPullCount; j++)
            {
                m_PulledObjArr[i, j] = Instantiate(m_PullingObject[i], transform).GetComponent<ForObjPull_Once>();
                m_PulledObjArr[i, j].gameObject.SetActive(false);
            }
        }
    }

    // Updates

    // Physics

    // Functions
    public void EnableNewObj(int _pullingObjIdx, Vector2 _spawnPos, Quaternion _rotation, bool _isRightHeaded)
    {
        if(_pullingObjIdx < m_PullingObject.Length)
        {
            m_PulledObjArr[_pullingObjIdx, m_Idx[_pullingObjIdx]].gameObject.transform.position = _spawnPos;
            m_PulledObjArr[_pullingObjIdx, m_Idx[_pullingObjIdx]].gameObject.transform.rotation = _rotation;

            if (_isRightHeaded)
                m_PulledObjArr[_pullingObjIdx, m_Idx[_pullingObjIdx]].gameObject.transform.localScale = new Vector2(1f, 1f);
            else
                m_PulledObjArr[_pullingObjIdx, m_Idx[_pullingObjIdx]].gameObject.transform.localScale = new Vector2(-1f, 1f);

            m_PulledObjArr[_pullingObjIdx, m_Idx[_pullingObjIdx]].gameObject.SetActive(false);
            m_PulledObjArr[_pullingObjIdx, m_Idx[_pullingObjIdx]].gameObject.SetActive(true);
            m_PulledObjArr[_pullingObjIdx, m_Idx[_pullingObjIdx]].resetTimer(m_DestroyTimer);
            m_Idx[_pullingObjIdx]++;
            if (m_Idx[_pullingObjIdx] >= m_ObjPullCount)
                m_Idx[_pullingObjIdx] = 0;
        }
    }

    // 기타 분류하고 싶은 것이 있을 경우
}
