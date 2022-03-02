using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    GameObject bulletPrefab;
    bool canfire = true;
    float firepredelay = 1.0f;
    [SerializeField]
    float bulletspeed = 1.0f;
    [SerializeField]
    float bulletdamage = 1.0f;
    private void Awake()
    {
        
    }
    public void Fire()
    {
        if(canfire)
        {
            Invoke(nameof(firePredelay), firepredelay);
            canfire = false;
        }
    }
    
    void firePredelay()
    {
        BulletCreate();
        
        canfire = true;
        
    }

    void BulletCreate()
    {
        Debug.Log("Fire");
        Instantiate(bulletPrefab, transform);
        
    }
}
