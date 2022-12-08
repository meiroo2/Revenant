using System;
using UnityEngine;


public class TempGapDistanceManipulator : MonoBehaviour
{
    public ShieldGang p_ToManipulate;

    private bool m_IsOn = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (m_IsOn)
            return;

        m_IsOn = true;
        p_ToManipulate.p_GapDistance = 0f;
    }
}