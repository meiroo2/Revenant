using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGenerator : MonoBehaviour
{
    public GameObject thisFire;
    private float tempT = 1f;

    private void Update()
    {
        tempT -= Time.deltaTime;
        if(tempT <= 0f)
        {
            tempT = 1f;
            GameObject temp = Instantiate(thisFire);
            temp.transform.position = transform.position;
        }
    }
}