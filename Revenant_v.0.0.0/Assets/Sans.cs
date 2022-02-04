using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sans : MonoBehaviour
{
    float sans = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, sans);
        transform.Translate(new Vector3(0.5f * Time.deltaTime, 0f, 0f));
        sans += Time.deltaTime * 50f;
    }
}
