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
}

public interface IUseableObj
{
    public void ActivateOutline(bool _isOn)
    {
    }

    public UseableObjList m_ObjProperty { get; set; }
    public bool m_isOn { get; set; }
    public int useObj(IUseableObjParam _param);               // 오브젝트를 사용 시 호출되는 함수(하위 오브젝트에 내용 추가할 것)
}