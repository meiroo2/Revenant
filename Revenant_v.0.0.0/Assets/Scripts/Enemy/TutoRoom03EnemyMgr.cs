using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoRoom03EnemyMgr : MonoBehaviour
{
    public List<Turret_Controller> m_turretList { get; set; } = new List<Turret_Controller>();

    [field: SerializeField]
    public Transform m_first_bullet { get; set; }

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
    }

    // 1. �ͷ� ����, 1ȸ ���
    public void TurretToggle(bool _input)
    {
        m_turretList[0].gameObject.SetActive(_input);
        m_turretList[0].WaitToAttack(1);
    }
    // 2. ����
    public void TurretFire_3()
    {
        m_turretList[0].Attack_3();
    }
    // 3. ���� �ð� �� �ͷ� 1ȸ ���
    public void TurretFire_1()
    {
        m_turretList[0].WaitToAttack(1);
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
    }
}
