using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressMgr : MonoBehaviour
{
    public int m_ProgressValue { get; protected set; } = -1;

    public virtual void NextProgress()
    {
        m_ProgressValue += 1;
        switch (m_ProgressValue)
        {
        }
    }
}