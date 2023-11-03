using System;
using UnityEngine;






public class SectionSensor : MonoBehaviour
{
    public SectionInfo _curContactSectionInfo;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out SectionInfo SectionInfo))
        {
            _curContactSectionInfo = SectionInfo;
        }
    }
}