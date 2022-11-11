using System;
using System.Collections.Generic;
using UnityEngine;


public class NearPointDetect : MonoBehaviour
{
    public Transform m_PointTransform;

    private List<ContactPoint2D> m_ContactList = new List<ContactPoint2D>();
    private void OnTriggerStay2D(Collider2D other)
    {
        m_ContactList = new List<ContactPoint2D>();
        int number =  other.GetContacts(m_ContactList);
        m_PointTransform.position = other.ClosestPoint(transform.position);
        Debug.Log(number + ", " + other.ClosestPoint(transform.position));
        for (int i = 0; i < number; i++)
        {
            
            Debug.Log(m_ContactList[i].point.ToString());
        }

    }
}