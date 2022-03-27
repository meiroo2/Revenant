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
    <Ŀ���� �ʱ�ȭ �Լ��� �ʿ��� ���>
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

    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}
