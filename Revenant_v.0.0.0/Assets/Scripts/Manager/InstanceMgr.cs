using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceMgr : MonoBehaviour
{
    public GameObject m_MainCanvas;

    [field: SerializeField] private GameObject[] m_ShouldBeMadeInWorld;
    [field: SerializeField] private GameObject[] m_ShouldBeMadeInCanvas;

    private static InstanceMgr Instance;
    public static InstanceMgr GetInstance() { return Instance; }

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < m_ShouldBeMadeInWorld.Length; i++)
        {
            Instantiate(m_ShouldBeMadeInWorld[i], this.gameObject.transform);
        }

        for (int i = 0; i < m_ShouldBeMadeInCanvas.Length; i++)
        {
            Instantiate(m_ShouldBeMadeInCanvas[i], m_MainCanvas.transform);
        }
    }
}
