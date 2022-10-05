using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asdsaadas : MonoBehaviour
{
    // Start is called before the first frame update
    public AnimationCurve curve;

    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Time.timeScale *= 2f;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale /= 2f;
        }
    }
}
