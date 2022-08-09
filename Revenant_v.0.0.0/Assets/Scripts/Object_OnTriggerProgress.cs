using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_OnTriggerProgress : MonoBehaviour
{
    private GameObject m_ProgressMgr;

    private void Awake()
    {
        m_ProgressMgr = GameObject.FindGameObjectWithTag("ProgressMgr");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_ProgressMgr.SendMessage("NextProgress");
            this.gameObject.SetActive(false);
        }
    }
}
