using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePos : MonoBehaviour, IUseableObj
{
    // Visible Member Variables
    [field: SerializeField] public UseableObjList m_ObjProperty { get; set; } = UseableObjList.HIDEPOS;

    // Member Variables


    // Constructors
    private void Awake()
    {

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
    public void useObj()
    {
        //Debug.Log("�������ϴ�!");
    }

    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}
