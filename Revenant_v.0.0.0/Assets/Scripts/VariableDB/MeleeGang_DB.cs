using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace VariableDB
{
    [CreateAssetMenu(fileName = "MeleeGang_DB", menuName = "VariableDB/MeleeGang_DB")]
    public class MeleeGang_DB : Variable_DB
    {
        public int Hp;
        public float Speed;
        public float FollowSpeedMulti;
        public int StunThreshold;
        public float VisionDistance;
        public float MeleeAtkDistance;
        [Range(0.0f, 1.0f)] public float PointAtkTime;
        public float DelayAfterAtk;
        public float StunWaitTime;
        public int HeadDmgMulti;
        public int BodyDmgMulti;

        [Title("MeleeWeapon")] 
        public int MeleeDmg;
    }
}