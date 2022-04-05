using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject m_MainCanvas;

    [field: SerializeField] private GameObject[] m_ShouldBeMadeInWorld;
    private static GameObject Instance;
    public static GameObject GetInstance() { return Instance; }

    private void Awake()
    {
        Instance = this.gameObject;
        for (int i = 0; i < m_ShouldBeMadeInWorld.Length; i++)
        {
            Instantiate(m_ShouldBeMadeInWorld[i], this.gameObject.transform);
        }
    }
}
