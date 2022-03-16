using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemy : MonoBehaviour, IAttacked
{
    public SpriteRenderer[] spriteRenderers;

    public void Attacked(AttackedInfo _AttackedInfo)
    {
        if (_AttackedInfo.m_HitPoint == HitPoints.HEAD)
        {
            Debug.Log(_AttackedInfo.m_Damage.ToString() + "�������� �Ѿ��� �Ӹ��� �¾ҽ��ϴ�!");
            spriteRenderers[0].color = new Color32(255, 0, 0, 255);
            Invoke(nameof(setToRed), 0.5f);
        }

        else if (_AttackedInfo.m_HitPoint == HitPoints.BODY)
        {
            Debug.Log(_AttackedInfo.m_Damage.ToString() + "�������� �Ѿ��� ���븦 �¾ҽ��ϴ�!");
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
