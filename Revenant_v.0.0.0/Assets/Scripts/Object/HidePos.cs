using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePos : MonoBehaviour, IUseableObj
{
    // Visible Member Variables
    [field: SerializeField] public UseableObjList m_ObjProperty { get; set; } = UseableObjList.HIDEPOS;
    public bool m_isOn { get; set; } = false;

    // Member Variables
    private HideObj m_HideObj;

    // Constructors
    private void Awake()
    {
        m_HideObj = GetComponentInParent<HideObj>();
    }
    private void Start()
    {

    }
    /*
    <커스텀 초기화 함수가 필요할 경우>
    public void Init()
    {

    }
    */

    // Updates
    private void Update()
    {
        m_isOn = m_HideObj.m_isOn;
    }
    private void FixedUpdate()
    {

    }

    // Physics


    // Functions
    public void useObj()
    {
        m_HideObj.setPlayerStateToHide();
    }

    // 기타 분류하고 싶은 것이 있을 경우
}
