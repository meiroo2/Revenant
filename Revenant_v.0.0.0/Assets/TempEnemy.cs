using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemy : MonoBehaviour, IBulletHit
{
    public SpriteRenderer[] spriteRenderers;

    public void BulletHit(float _damage, int _hitPoint)
    {
        if (_hitPoint == 0)
        {
            Debug.Log(_damage.ToString() + "데미지의 총알이 머리를 맞았습니다!");
            spriteRenderers[0].color = new Color32(255, 0, 0, 255);
            Invoke(nameof(setToRed), 0.5f);
        }

        else if (_hitPoint == 1)
        {
            Debug.Log(_damage.ToString() + "데미지의 총알이 몸통를 맞았습니다!");
            spriteRenderers[1].color = new Color32(255, 0, 0, 255);
            Invoke(nameof(setToRed1), 0.5f);
        }  
    }

    private void setToRed()
    {
        spriteRenderers[0].color = new Color32(0, 0, 255, 255);
    }
    private void setToRed1()
    {
        spriteRenderers[1].color = new Color32(0, 0, 255, 255);
    }
}
