using UnityEngine;
using UnityEngine.Events;


public class EnemyTracknAction : EnemySpawner
{
    // Visible Member Variables
    public UnityEvent p_WillExecute;
    public BasicEnemy[] p_CheckEnemyArr;
    private bool m_IsActivated = false;
    
    // Member Variables
    public bool[] m_IsDead;
    
    
    // Constructors
    private void Awake()
    {
        for (int i = 0; i < p_CheckEnemyArr.Length; i++)
        {
            p_CheckEnemyArr[i].AddEnemySpawner(this);
        }

        m_IsDead = new bool[p_CheckEnemyArr.Length];
        for (int i = 0; i < m_IsDead.Length; i++)
        {
            m_IsDead[i] = false;
        }
    }

    public override void AchieveEnemyDeath(GameObject _enemy)
    {
        if (m_IsActivated)
            return;
        
        if (_enemy.TryGetComponent(out BasicEnemy enemy))
        {
            for (int i = 0; i < p_CheckEnemyArr.Length; i++)
            {
                if (enemy == p_CheckEnemyArr[i])
                {
                    m_IsDead[i] = true;
                }
            }
        }

        bool canPass = true;
        for (int i = 0; i < m_IsDead.Length; i++)
        {
            canPass = m_IsDead[i];
        }

        if (canPass)
        {
            m_IsActivated = true;
            p_WillExecute?.Invoke();
        }
    }
}