using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairHair : MonoBehaviour
{
    Quaternion purpseRot = new Quaternion(0, 0, 180, 1);
    bool input = false;
    void Start()
    {
        //transform.rotation = purpseRot;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            input = true;
        }

        if(input)
        {
            transform.rotation = purpseRot; Quaternion.Lerp(transform.rotation, purpseRot, 0.1f);
        }

        //molamola
    }
}
