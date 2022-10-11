using System.Collections;
using UnityEditor;
using UnityEngine;


public class BasicWeapon : MonoBehaviour
{
    // Visible Member Variables
    //public GameObject m_bulletPrefab;
    [field : SerializeField] public bool m_isPlayers { get; protected set; }

    [Space(20f)] [Header("여기서부터 설정값들")]
    // 0 = 무한탄창, 1 = 제한탄창
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
    public int m_LeftRounds { get; protected set; } = 0;
    public int m_LeftMags { get; set; } = 0;
    protected bool m_isShotDelayEnd = true;


    // Constructors


    // Updates


    // Physics


    // Functions
    
    /// <summary>
    /// 남은 총알 수를 강제로 설정합니다. (UI 즉시 업데이트)
    /// </summary>
    /// <param name="_leftRound">남은 총알 수</param>
    public virtual void SetLeftRounds(int _leftRound)
    {
    }

    public void SetCallback(m_WeaponDelegate _inputFunc, bool _resetDele = false)
    {
        if (_resetDele)
            m_Callback = null;

        m_Callback += _inputFunc;
    }
    public virtual int Fire() { return 0; } // 0 = Fail, 1 = Success, 2 = Fail(No Ammo)
    public virtual void Reload() { }
    public virtual bool GetCanReload()   // 0 = Fail, 1 = Success
    {
        return false;
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