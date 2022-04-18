using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IUseableObj
{
    // Visible Member Variables
    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.OBJECT;
    public bool m_isOn { get; set; } = false;
    public GameObject m_OtherSide;

    // Member Variables
    private GameObject m_Player;


    // Constructors
    private void Awake()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
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

    }
    private void FixedUpdate()
    {

    }

    // Physics


    // Functions
    public bool useObj()
    {
        m_Player.transform.position = m_OtherSide.transform.position;
        return true;
    }

    // 기타 분류하고 싶은 것이 있을 경우
}
