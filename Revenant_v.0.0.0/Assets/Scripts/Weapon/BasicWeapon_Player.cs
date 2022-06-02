using System.Collections;
using UnityEngine;
using UnityEditor;



public class BasicWeapon_Player : BasicWeapon
{
    // Visible Member Variables
    [field: SerializeField] public float p_ReloadTime { get; protected set; }

    // Member Variables
    protected ShellMgr m_ShellMgr;
    protected Transform m_Player_Arm;
    protected AimCursor m_aimCursor;
    protected Player m_Player;

    public bool m_isReloading { get; private set; }

    // Functions
    protected virtual void Internal_Reload()
    {
    }

    protected IEnumerator SetReload()
    {
        m_isReloading = true;
        yield return new WaitForSeconds(p_ReloadTime);
        m_isReloading = false;
        Internal_Reload();
    }
}