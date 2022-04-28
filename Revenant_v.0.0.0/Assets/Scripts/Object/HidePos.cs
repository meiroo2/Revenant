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
    <Ŀ���� �ʱ�ȭ �Լ��� �ʿ��� ���>
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
    public int useObj(IUseableObjParam _param)
    {
        if (!m_HideObj.m_isOn)  // �� ����
        {
            m_HideObj.m_isPlayerHide = true;
            m_HideObj.setPlayerStateToHide();   // ���´�
            return 1;    // ���� ����
        }
        else
        {       // ������ ���� �ִ� ����
            // �÷��̾ ���� ���¸�
            if (m_HideObj.m_Player.m_curPlayerState == playerState.HIDDEN || m_HideObj.m_Player.m_curPlayerState == playerState.HIDDEN_STAND)
            {
                m_HideObj.m_isPlayerHide = false;
                m_HideObj.setPlayerStateToHide();
                return 1;
            }
            else
                return 0;
        }
    }

    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}
