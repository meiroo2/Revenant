using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_UseRange : MonoBehaviour
{
    // Visible Member Variables


    // Member Variables
    private bool isPressedFKey = false;
    private float Timer = 0.1f;

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
        if (isPressedFKey)
        {
            Timer -= Time.deltaTime;
            if(Timer <= 0f)
            {
                Timer = 0.1f;
                isPressedFKey = false;
            }
        }


        if (Input.GetKeyDown(KeyCode.F))
            isPressedFKey = true;
    }
    private void FixedUpdate()
    {

    }

    // Physics
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isPressedFKey)
        {
            collision.GetComponent<IUseableObj>().useObj();
            isPressedFKey = false;
        }
    }

    // Functions


}
