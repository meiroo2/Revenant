using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloImBullet : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * 1f;
    }
}
