using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseableObj
{
    public bool canUse { get; set; }    // ������Ʈ ��ȣ�ۿ� ���� ����
    public void useObj();               // ������Ʈ�� ��� �� ȣ��Ǵ� �Լ�(���� ������Ʈ�� ���� �߰��� ��)
}