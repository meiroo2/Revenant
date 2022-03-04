using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    GameObject bulletPrefab;
    bool canfire = true;
    [SerializeField]
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
        // 수정할거 첫발을 쏜 후에 딜레이가 생기도록, 맨 처음 준비 동작 딜레이는 Enemy 스크립트에서 따로
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
        //Debug.Log("Fire");
        GameObject gameObject = Instantiate(bulletPrefab);
        gameObject.transform.position = transform.position;

    }
}
