using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParicleMgr : MonoBehaviour
{
    public GameObject p_PullingObj;

    private GameObject[] m_Objs;
    private Rigidbody2D[] m_Rigids;

    private void Awake()
    {
        m_Objs = new GameObject[7];
        m_Rigids = new Rigidbody2D[7];

        for(int i = 0; i < m_Objs.Length; i++)
        {
            m_Objs[i] = Instantiate(p_PullingObj);
            m_Objs[i].transform.parent = transform;
            m_Objs[i].transform.localPosition = Vector2.zero;

            m_Rigids[i] = m_Objs[i].GetComponent<Rigidbody2D>();
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < m_Objs.Length; i++)
        {
            m_Objs[i].transform.localPosition = Vector2.zero;
            m_Rigids[i].velocity = (new Vector2(Random.Range(-3f, -1f), Random.Range(1f, 3f)));
            m_Rigids[i].constraints = RigidbodyConstraints2D.None;
        }
    }
}
