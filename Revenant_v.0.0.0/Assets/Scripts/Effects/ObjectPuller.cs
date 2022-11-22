using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Mathematics;

public class ObjectPuller : MonoBehaviour
{
    // Visible Member Variables
    public int m_ObjPullCount = 1;
    public GameObject m_PullingObject;
    public float m_DestroyTimer = 0.8f;

    // Member Variables
    protected int m_Idx = 0;
    protected ForObjPull_Once[] m_PulledObjArr;
    protected SoundPlayer m_SoundPlayer;

    private Vector2 m_OriginPos;

    // Constructors
    protected void Awake()
    {
        m_PulledObjArr = new ForObjPull_Once[m_ObjPullCount];
        for (int i = 0; i < m_ObjPullCount; i++)
        {
            m_PulledObjArr[i] = Instantiate(m_PullingObject, transform).GetComponent<ForObjPull_Once>();
        }

        m_OriginPos = m_PulledObjArr[0].transform.localPosition;
    }

    protected void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_SoundPlayer = GameMgr.GetInstance().p_SoundPlayer;
        
        for (int i = 0; i < m_ObjPullCount; i++)
        {
            m_PulledObjArr[i].m_SoundPlayer = m_SoundPlayer;
            m_PulledObjArr[i].gameObject.SetActive(false);
        }
    }
    

    // Updates

    // Physics

    // Functions
    public virtual void EnableNewObj()
    {
        m_PulledObjArr[m_Idx].gameObject.SetActive(true);
        m_PulledObjArr[m_Idx].InitPulledObj();
        m_Idx++;
        if (m_Idx >= m_ObjPullCount)
            m_Idx = 0;
    }

    /// <summary>
    /// ObjectPuller에서 생성과 동시에 해당 오브젝트를 받아옵니다.
    /// Transform 초기화를 진행합니다.
    /// </summary>
    /// <returns></returns>
    public virtual ForObjPull_Once GetNSpawnNewObj(Transform _parent, Transform _transform = null)
    {
        ForObjPull_Once returnObj = m_PulledObjArr[m_Idx];

        Transform returnObjTransform = returnObj.transform;
        returnObjTransform.parent = transform;
        returnObjTransform.localPosition = m_OriginPos;
        //returnObjTransform.rotation = quaternion.Euler(Vector3.zero);
        
        if (_transform)
        {
            returnObj.transform.parent = _transform;
            returnObjTransform.rotation = 
                quaternion.Euler(0f,0f, _parent.rotation.eulerAngles.z);
        }

        m_PulledObjArr[m_Idx].gameObject.SetActive(true);
        m_PulledObjArr[m_Idx].InitPulledObj();
        m_Idx++;
        if (m_Idx >= m_ObjPullCount)
            m_Idx = 0;

        return returnObj;
    }
}
