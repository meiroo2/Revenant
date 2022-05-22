using System.Collections;
using UnityEditor;
using UnityEngine;


public class BasicWeapon : MonoBehaviour
{
    // Visible Member Variables
    public GameObject m_bulletPrefab;
    [field : SerializeField] public bool m_isPlayers { get; protected set; }

    [Space(20f)] [Header("여기서부터 설정값들")]
    public int p_WeaponType;
    public float p_BulletSpeed;
    public int p_BulletDamage;
    public int p_StunValue;
    public float p_MinimumShotDelay;
    public int p_MaxBullet;
    public int p_MaxMag;


    // Member Variables
    protected SoundMgr_SFX m_SoundMgrSFX;
    
    
    public int m_LeftBullet { get; set; } = 0;
    public int m_LeftMag { get; set; } = 0;
    protected bool m_isShotDelayEnd = true;


    // Constructors


    // Updates


    // Physics


    // Functions
    public virtual int Fire() { return 0; } // 0 = Fail, 1 = Success, 2 = Fail(No Ammo)
    public virtual int Reload() { return 0; }   // 0 = Fail, 1 = Success
    public virtual void InitWeapon()
    {
        // 남은 총알 및 탄창 값 대입
    }
    public virtual void ExitWeapon()
    {
        // 남은 총알 및 탄창 값 반출
    }
    protected IEnumerator SetShotDelay()
    {
        yield return new WaitForSeconds(p_MinimumShotDelay);
        m_isShotDelayEnd = true;
    }
}