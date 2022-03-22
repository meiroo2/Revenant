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
    public void useObj();               // 오브젝트를 사용 시 호출되는 함수(하위 오브젝트에 내용 추가할 것)
}