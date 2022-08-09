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
    LAYERDOOR,
}

public interface IUseableObj
{
    /// <summary> IUseableObj를 상속받은 오브젝트의 Outline을 활성화시킵니다. </summary>
    /// <param name="_isOn"> 켜짐/꺼짐 여부 </param>
    public void ActivateOutline(bool _isOn)
    {
    }

    public bool m_IsOutlineActivated { get; set; }

    public UseableObjList m_ObjProperty { get; set; }
    public bool m_isOn { get; set; }
    public int useObj(IUseableObjParam _param);               // 오브젝트를 사용 시 호출되는 함수(하위 오브젝트에 내용 추가할 것)
}