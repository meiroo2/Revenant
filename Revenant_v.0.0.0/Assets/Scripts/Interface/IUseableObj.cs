using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseableObj
{
    public bool canUse { get; set; }    // 오브젝트 상호작용 가능 여부
    public void useObj();               // 오브젝트를 사용 시 호출되는 함수(하위 오브젝트에 내용 추가할 것)
}