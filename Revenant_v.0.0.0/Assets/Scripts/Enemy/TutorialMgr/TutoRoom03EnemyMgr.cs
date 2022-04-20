using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoRoom03EnemyMgr : MonoBehaviour
{

    TuRoom03_ProgressMgr m_room3ProgressMgr;

    [SerializeField]
    float m_waitTime = 0.5f;
    public List<Turret_Controller> m_turretList { get; set; } = new List<Turret_Controller>();

    [field: SerializeField]
    public Transform m_first_bullet { get; set; }
    private void Awake()
    {
        m_room3ProgressMgr = GameObject.Find("ProgressMgr").GetComponent<TuRoom03_ProgressMgr>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TurretToggle(true);
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            TurretFire_3();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            TurretExit();
        }
    }

    // ============================��ȯ���� �ʿ��� ���==================================
    // 1. �ͷ� ����
    public void TurretToggle(bool _input)
    {
        m_turretList[0].gameObject.SetActive(_input);
        Invoke(nameof(SendToProgress), m_waitTime);
    }
    

    // 2, 4. ���� �ð� �� �ͷ� 1ȸ ���
    public void TurretFire_1()
    {
        m_turretList[0].WaitToAttack(1);
    }

    // 3. ����
    public void TurretFire_3()
    {
        m_turretList[0].Attack_3();
    }

    // 4. 1��° �Ѿ� Transform
    public Transform GetFirstBulletPos()
    {
        Debug.Log(m_first_bullet.position);
        return m_first_bullet;
    }

    // 5. �ͷ� ����
    public void TurretExit()
    {
        m_turretList[0].m_turretAnimator.ExitAnim();
        Invoke(nameof(SendToProgress), m_waitTime);
    }
    // ======================================================================================

    // ����޽���
    public void SendToProgress()
    {
        Debug.Log("sendtoprogress");
        m_room3ProgressMgr.SendMessage("NextProgress");
    }
}
