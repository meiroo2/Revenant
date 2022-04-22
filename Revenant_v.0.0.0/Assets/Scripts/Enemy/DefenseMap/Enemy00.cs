using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy00 : MonoBehaviour, IEnemyType
{
    public void getInfo()
    {
        Debug.Log(gameObject.name);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
