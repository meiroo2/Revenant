using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IUseableObjParam
{
    public Transform m_UserTransform { get; private set; }
    public bool m_isPlayer { get; private set; }

    public IUseableObjParam(Transform _inputUserTransform, bool _isPlayer)
    {
        m_UserTransform = _inputUserTransform;
        m_isPlayer = _isPlayer;
    }
}

public enum UseableObjList
{
    HIDEPOS,
    OBJECT,
}

public interface IUseableObj
{
    public UseableObjList m_ObjProperty { get; set; }
    public bool m_isOn { get; set; }
    public int useObj(IUseableObjParam _param);               // ������Ʈ�� ��� �� ȣ��Ǵ� �Լ�(���� ������Ʈ�� ���� �߰��� ��)
}