using System.Collections;
using UnityEngine;


public class EnemyChecker : EnemySpawner
{
    public StageManager m_StageMgr;
    
    protected override void SpawnAllEnemys()
    {
        Debug.Log("정상확인");
        m_StageMgr.SendToStageMgr(0);
    }
    
    protected override IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(p_SpawnDelay);
        
    }
}