using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCursor : MonoBehaviour
{
    // Visible Member Variables

    // Member Variables
    public int AimedObjid { get; private set; } = 0;
    private Collider2D m_AimedCollider;
    private Vector2 m_CursorPos;

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
        m_CursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = m_CursorPos;
    }
    private void FixedUpdate()
    {

    }

    // Physics
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(m_AimedCollider != collision)
        {
            m_AimedCollider = collision;
            AimedObjid = m_AimedCollider.gameObject.GetInstanceID();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        AimedObjid = 0;
    }

    // Functions


    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}
