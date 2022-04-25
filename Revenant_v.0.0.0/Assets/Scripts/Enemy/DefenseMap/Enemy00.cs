using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy00 : MonoBehaviour, IEnemyType
{
    bool isActive = false;
    public void setActive()
    {
        if (isActive)
            Debug.Log("Already Active == True");
        else
        {
            gameObject.SetActive(true);
            isActive = true;
        }
    }
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
