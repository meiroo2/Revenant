using System.Collections;
using UnityEditor;
using UnityEngine;


public class BasicWeapon : MonoBehaviour
{
    // Visible Member Variables
    //public GameObject m_bulletPrefab;
    [field : SerializeField] public bool m_isPlayers { get; protected set; }

    [Space(20f)] [Header("여기서부터 설정값들")]
    public int p_WeaponType;
    public float p_BulletSpeed;
    public int p_BulletDamage;
    public int p_StunValue;
    public float p_MinFireDelay;
    public int p_MaxRound;
    public int p_MaxMag;


    // Member Variables
    public delegate void m_WeaponDelegate();
    protected m_WeaponDelegate m_Callback = null;
    
    public SoundPlayer m_SoundPlayer { get; set; }
    public int m_LeftRounds { get; set; } = 0;
    public int m_LeftMags { get; set; } = 0;
    protected bool m_isShotDelayEnd = true;


    // Constructors


    // Updates


    // Physics


    // Functions
    public void SetCallback(m_WeaponDelegate _inputFunc, bool _resetDele = false)
    {
        if (_resetDele)
            m_Callback = null;

        m_Callback += _inputFunc;
    }
    public virtual int Fire() { return 0; } // 0 = Fail, 1 = Success, 2 = Fail(No Ammo)
    public virtual void Reload() { }
    public virtual int GetCanReload()   // 0 = Fail, 1 = Success
    {
        return 0;
    }
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
        yield return new WaitForSeconds(p_MinFireDelay);
        m_isShotDelayEnd = true;
    }
}