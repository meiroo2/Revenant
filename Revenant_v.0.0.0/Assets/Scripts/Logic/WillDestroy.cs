using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillDestroy : MonoBehaviour
{
    private float Timer = 3f;
    private void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0f)
            Destroy(this.gameObject);
    }
}
