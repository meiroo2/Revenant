using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class HitSFXMaker : MonoBehaviour
{
    // Visible Member Variables
    public int m_ObjPullCount = 1;
    public GameObject[] p_PullingObject;

    // Member Variables
    private int[] m_CurIdxs;
    private int[] m_Limits;
    private HitSFX_Instance[] m_PulledSFXArr;

    // Constructors
    private void Awake()
    {
        m_CurIdxs = new int[p_PullingObject.Length];
        m_Limits = new int[p_PullingObject.Length];
        
        List<HitSFX_Instance> list = new List<HitSFX_Instance>();
        for (int i = 0; i < p_PullingObject.Length; i++)
        {
            for (int j = 0; j < m_ObjPullCount; j++)
            {
                GameObject instanced = Instantiate(p_PullingObject[i], transform, true);
                list.Add(instanced.GetComponent<HitSFX_Instance>());
                instanced.SetActive(false);
            }

            m_Limits[i] = list.Count - 1;
            m_CurIdxs[i] = m_Limits[i] - (m_ObjPullCount - 1);
        }
        
        m_PulledSFXArr = list.ToArray();
    }

    // Updates

    // Physics

    // Functions
    public void EnableNewObj(int _idx, Vector2 _spawnPos, bool _isRightHeaded = true)
    {
        if (_idx < 0 || _idx >= p_PullingObject.Length)
            return;

        if (m_PulledSFXArr[m_CurIdxs[_idx]].gameObject.activeSelf)
            m_PulledSFXArr[m_CurIdxs[_idx]].gameObject.SetActive(false);
        
        m_PulledSFXArr[m_CurIdxs[_idx]].gameObject.SetActive(true);
        Transform sfxTransform = m_PulledSFXArr[m_CurIdxs[_idx]].transform;
        sfxTransform.localScale = new Vector3(Mathf.Abs(sfxTransform.localScale.x) * (_isRightHeaded ? 1 : -1) ,
            sfxTransform.localScale.y, sfxTransform.localScale.z);
        sfxTransform.position = _spawnPos;

        sfxTransform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

        m_CurIdxs[_idx]++;

        if (m_CurIdxs[_idx] > m_Limits[_idx])
            m_CurIdxs[_idx] = m_Limits[_idx] - (m_ObjPullCount - 1);
    }
}
