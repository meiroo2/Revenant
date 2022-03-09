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
        // �����Ұ� ù���� �� �Ŀ� �����̰� ���⵵��, �� ó�� �غ� ���� �����̴� Enemy ��ũ��Ʈ���� ����
        if(canfire)
        {
            BulletCreate();
            Invoke(nameof(firePredelay), firepredelay);
            canfire = false;
        }
    }
    
    void firePredelay()
    {
        canfire = true;
        
    }

    void BulletCreate()
    {
        //Debug.Log("Fire");
        GameObject gameObject = Instantiate(bulletPrefab);
        gameObject.transform.position = transform.position;

    }
}
