using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    // Visible Member Variables
    public GameObject m_NoisePrefab;
    public int m_ObjectPullingCount = 10;

    // Member Variables
    private NoisePrefab[] m_NoiseObjectList;
    private int m_NoiseIdx = 0;

    // Constructors
    private void Awake()
    {
        m_NoiseObjectList = new NoisePrefab[m_ObjectPullingCount];
        for(int i = 0; i < m_ObjectPullingCount; i++)
        {
            m_NoiseObjectList[i] = GameObject.Instantiate(m_NoisePrefab).GetComponent<NoisePrefab>();
            m_NoiseObjectList[i].gameObject.transform.parent = transform;
        }
    }

    // Updates


    // Physics


    // Functions
    public void MakeNoise(NoiseType _noiseType, Vector2 _noiseSize, LocationInfo _noiseLocation, bool _isPlayer)
    {
        m_NoiseObjectList[m_NoiseIdx].gameObject.SetActive(true);
        m_NoiseObjectList[m_NoiseIdx].InstantiateNoise(_noiseType, _noiseSize, _noiseLocation, _isPlayer);
        m_NoiseIdx++;
        if(m_NoiseIdx >= m_NoiseObjectList.Length)
        {
            m_NoiseIdx = 0;
        }
    }

    // ??? ?¬Ù???? ???? ???? ???? ???
}
