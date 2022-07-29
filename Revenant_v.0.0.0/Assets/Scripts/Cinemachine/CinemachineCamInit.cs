using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCamInit : MonoBehaviour
{
    private Vector3 m_InitPos;

    private void Awake()
    {
        m_InitPos = transform.position;
        m_InitPos.z = -10f;
        transform.position = m_InitPos;
    }
}
