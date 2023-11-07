using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using VariableDB;


public class EnemyMgr : MonoBehaviour
{
    // Test Public Variables
    [TabGroup("NormalGang")] public NormalGang_DB[] NormalGangDBArr;
    [TabGroup("NormalGang")] public int IdxForNormalGangDB;

    [TabGroup("MeleeGang")] public MeleeGang_DB[] MeleeGangDBArr;
    [TabGroup("MeleeGang")] public int IdxForMeleeGangDB;
    
    [TabGroup("DroneGang")] public DroneGang_DB[] DroneGangDBArr;
    [TabGroup("DroneGang")] public int IdxForDroneGangDB;

    [TabGroup("ShieldGang")] public ShieldGang_DB[] ShieldGangDBArr;
    [TabGroup("ShieldGang")] public int IdxForShieldGangDB;
    
    [TabGroup("SpecialGang")] public SpecialGang_DB[] SpecialGangDBArr;
    [TabGroup("SpecialGang")] public int IdxForSpecialGangDB;
    
    // Member Variables


    // Constructors
    

    // Functions
    public NormalGang_DB GetNormalGangDB()
    {
        if (IdxForNormalGangDB < 0 || IdxForNormalGangDB >= NormalGangDBArr.Length)
        {
            Debug.LogError("NormalGangDB를 Return할 수 없습니다.");
            return null;
        }
        
        return NormalGangDBArr[IdxForNormalGangDB];
    }

    public MeleeGang_DB GetMeleeGangDB()
    {
        if (IdxForMeleeGangDB < 0 || IdxForMeleeGangDB >= MeleeGangDBArr.Length)
        {
            Debug.LogError("MeleeGangDB를 Return할 수 없습니다.");
            return null;
        }

        return MeleeGangDBArr[IdxForMeleeGangDB];
    }

    public DroneGang_DB GetDroneGangDB()
    {
        if (IdxForDroneGangDB < 0 || IdxForDroneGangDB >= DroneGangDBArr.Length)
        {
            Debug.LogError("DroneGangDB를 Return할 수 없습니다.");
            return null;
        }

        return DroneGangDBArr[IdxForDroneGangDB];
    }

    public ShieldGang_DB GetShieldGangDB()
    {
        if (IdxForShieldGangDB < 0 || IdxForShieldGangDB >= ShieldGangDBArr.Length)
        {
            Debug.LogError("ShieldGangDB를 Return할 수 없습니다.");
            return null;
        }

        return ShieldGangDBArr[IdxForShieldGangDB];
    }

    public SpecialGang_DB GetSpecialGangDB()
    {
        if (IdxForSpecialGangDB < 0 || IdxForSpecialGangDB >= SpecialGangDBArr.Length)
        {
            Debug.LogError("SpecialGangDB를 Return할 수 없습니다.");
            return null;
        }

        return SpecialGangDBArr[IdxForSpecialGangDB];
    }
    
    [Button]
    public void StickToFloor()
    {
        RaycastHit2D cast;
        int m_LayerMask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("EmptyFloor"));
        
        NormalGang[] tempNGangs = FindObjectsOfType<NormalGang>();
        for (int i = 0; i < tempNGangs.Length; i++)
        {
            cast = Physics2D.Raycast(tempNGangs[i].transform.position, -transform.up, 1f, m_LayerMask);
            tempNGangs[i].transform.position = new Vector2(cast.point.x, cast.point.y + 0.64f);
        }
        
        MeleeGang[] tempMGangs = FindObjectsOfType<MeleeGang>();
        for (int i = 0; i < tempMGangs.Length; i++)
        {
            cast = Physics2D.Raycast(tempMGangs[i].transform.position, -transform.up, 1f, m_LayerMask);
            tempMGangs[i].transform.position = new Vector2(cast.point.x, cast.point.y + 0.64f);
        }
    }

    public void SetNormalGangs()
    {
        NormalGang[] temp = FindObjectsOfType<NormalGang>();
        foreach (var ele in temp)
        {
            ele.SetEnemyValues(this);
        }
    }

    public void SetMeleeGangs()
    {
        MeleeGang[] temp = FindObjectsOfType<MeleeGang>();
        foreach (var ele in temp)
        {
            ele.SetEnemyValues(this);
        }
    }

    public void SetDrones()
    {
        Drone[] temp = FindObjectsOfType<Drone>();
        foreach (var ele in temp)
        {
            ele.SetEnemyValues(this);
        }
    }

    public void SetShieldGangs()
    {
        ShieldGang[] temp = FindObjectsOfType<ShieldGang>();
        foreach (var VARIABLE in temp)
        {
            VARIABLE.SetEnemyValues(this);
        }
    }

    public void SetSpecialForce()
    {
        SpecialForce[] temp = FindObjectsOfType<SpecialForce>();
        foreach (var VARIABLE in temp)
        {
            VARIABLE.SetEnemyValues(this);
        }
    }
}