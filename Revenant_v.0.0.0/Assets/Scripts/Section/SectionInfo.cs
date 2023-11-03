using UnityEngine;





public class SectionInfo : MonoBehaviour
{
    public SectionDoor[] SectionDoorArr;

    private void Awake()
    {
        if (SectionDoorArr.Length == 0)
            return;
        
        foreach (var element in SectionDoorArr)
        {
            element.WhichSectionBelong = this;
        }
    }
}