using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using VariableDB;

public class PlayerDBManager : MonoBehaviour
{
    public Player_DB[] PlayerDBArr;
    public int IdxForPlayerDB;
    
    public bool TryGetPlayerDB(out Player_DB playerDB)
    {
        if (IdxForPlayerDB < 0 || IdxForPlayerDB >= PlayerDBArr.Length ||
            ReferenceEquals(PlayerDBArr?[IdxForPlayerDB], null))
        {
            playerDB = null;
            return false;   
        }
        else
        {
            playerDB = PlayerDBArr[IdxForPlayerDB];
            return true;
        }
    }
}