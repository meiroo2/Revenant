using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WeaponMgr : WeaponMgr
{
   public int[] p_LeftBullet;
   public int[] p_LeftMag;
   
   private new void Awake()
   {
      base.Awake();

      for (int i = 0; i < p_Weapons.Count; i++)
      {
         p_Weapons[i].m_LeftBullet = p_LeftBullet[i];
         p_Weapons[i].m_LeftMag = p_LeftMag[i];
      }
   }
}