using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool isRightHeaded;
    private Vector3 m_playerlocalScale;

    private void Awake()
    {
        isRightHeaded = true;
        m_playerlocalScale = new Vector3(1f, 1f, 1f);
    }

    public void setisRightHeaded(bool _inputVal)
    {
        isRightHeaded = _inputVal;
        if (isRightHeaded)
            m_playerlocalScale.x = 1f;
        else
            m_playerlocalScale.x = -1f;
        transform.localScale = m_playerlocalScale;
    }

    public bool getisRightHeaded() { return isRightHeaded; }
}
