using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimImageCanvas : MonoBehaviour
{
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
