using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IUseableObjParam
{
    public Transform m_UserTransform { get; private set; }
    public bool m_isPlayer { get; private set; }
    public int m_ObjInstanceNum;

    public IUseableObjParam(Transform _inputUserTransform, bool _isPlayer, int _instanceNum)
    {
        m_UserTransform = _inputUserTransform;
        m_isPlayer = _isPlayer;
        m_ObjInstanceNum = _instanceNum;
    }
}

public enum UseableObjList
{
    HIDEPOS,
    OBJECT,
    STAIRPOS,
    LAYERDOOR,
    CHECKPOINT,
}

public interface IUseableObj
{
    /// <summary> IUseableObj�� ��ӹ��� ������Ʈ�� Outline�� Ȱ��ȭ��ŵ�ϴ�. </summary>
    /// <param name="_isOn"> ����/���� ���� </param>
    public void ActivateOutline(bool _isOn)
    {
    }

    public bool m_IsOutlineActivated { get; set; }

    public UseableObjList m_ObjProperty { get; set; }
    public bool m_isOn { get; set; }
    public int useObj(IUseableObjParam _param);               // ������Ʈ�� ��� �� ȣ��Ǵ� �Լ�(���� ������Ʈ�� ���� �߰��� ��)
}