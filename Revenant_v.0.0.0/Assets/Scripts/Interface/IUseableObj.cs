using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UseableObjList
{
    HIDEPOS,
    OBJECT
}

public interface IUseableObj
{
    public UseableObjList m_ObjProperty { get; set; }
    public bool m_isOn { get; set; }
    public void useObj();               // ������Ʈ�� ��� �� ȣ��Ǵ� �Լ�(���� ������Ʈ�� ���� �߰��� ��)
}