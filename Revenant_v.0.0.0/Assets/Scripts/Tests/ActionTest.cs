using System;
using UnityEngine;


public class ActionTest : MonoBehaviour
{
    private void Start()
    {
        asdas(sans);
    }

    public void sans()
    {
        
    }
    
    public void asdas(Action _action = null)
    {
        
        
        _action?.Invoke();
    }
}