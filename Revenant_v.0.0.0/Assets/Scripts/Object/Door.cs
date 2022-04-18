using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IUseableObj
{
    // Visible Member Variables
    [field: SerializeField] public bool canUse { get; set; } = true;
    [field: SerializeField] private bool isOpen = false;

    // Member Variables
    public UseableObjList m_ObjProperty { get; set; }
    public bool m_isOn { get; set; } = false;

    // Constructors
    private void Awake()
    {
        
    }
    private void Start()
    {

    }

    // Updates
    private void Update()
    {

    }
    private void FixedUpdate()
    {

    }

    // Physics


    // Functions
    public bool useObj()
    {
        if (isOpen)
        {
            isOpen = false;
            GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
        }
        else
        {
            isOpen = true;
            GetComponent<SpriteRenderer>().color = new Color32(0, 0, 255, 255);
        }

        return true;
    }

}
