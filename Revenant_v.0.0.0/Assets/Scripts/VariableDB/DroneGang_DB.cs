using Sirenix.OdinInspector;
using UnityEngine;

namespace VariableDB
{
    [CreateAssetMenu(fileName = "DroneGang_DB", menuName = "VariableDB/DroneGang_DB")]
    public class DroneGang_DB : Gang_DB
    {
        public int Hp;
        public float Speed;
        public float BreakPower;
        public float RushSpeedMulti;
        public float RushTriggerDistance;
        public int DroneDmgMulti;
        public int BombDmgMulti;
        public float DetectSpeed;
        public float VisionDistance;
        [Range(0f, 1f)] public float DecidePositionPointTime;
        
        [Title("BombWeapon")] 
        public int BombDamage;
        public float BombRadius;
    }
}