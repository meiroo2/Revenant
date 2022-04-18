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

    // ============================유환진이 필요한 기능==================================
    // 1. 터렛 등장
    public void TurretToggle(bool _input)
    {
        m_turretList[0].gameObject.SetActive(_input);
        Invoke(nameof(SendToProgress), m_waitTime);
    }
    

    // 2, 4. 일정 시간 후 터렛 1회 사격
    public void TurretFire_1()
    {
        m_turretList[0].WaitToAttack(1);
    }

    // 3. 난사
    public void TurretFire_3()
    {
        m_turretList[0].Attack_3();
    }

    // 4. 1번째 총알 Transform
    public Transform GetFirstBulletPos()
    {
        Debug.Log(m_first_bullet.position);
        return m_first_bullet;
    }

    // 5. 터렛 퇴장
    public void TurretExit()
    {
        m_turretList[0].m_turretAnimator.ExitAnim();
        Invoke(nameof(SendToProgress), m_waitTime);
    }
    // ======================================================================================

    // 샌드메시지
    public void SendToProgress()
    {
        Debug.Log("sendtoprogress");
        m_room3ProgressMgr.SendMessage("NextProgress");
    }
}
