using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLine : MonoBehaviour
{
    public GameObject StartPos;
    public GameObject EndPos;
    public GameObject m_Line;

    private void Update()
    {
        if(m_Line.transform.position.x < EndPos.transform.position.x)
        {
            m_Line.transform.position = StartPos.transform.position;
        }

        
    }

    private void FixedUpdate()
    {
        m_Line.transform.Translate(Vector2.left * Time.deltaTime * 5f);
    }
}
