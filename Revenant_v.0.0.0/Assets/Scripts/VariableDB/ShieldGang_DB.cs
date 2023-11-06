using Sirenix.OdinInspector;
using UnityEngine;

namespace VariableDB
{
    [CreateAssetMenu(fileName = "ShieldGang_DB", menuName = "VariableDB/ShieldGang_DB")]
    public class ShieldGang_DB : Gang_DB
    {
        public int Hp;
        public int ShieldHp;
        public int MeleeDamage;
        public float Speed;
        public float BackSpeedMulti;
        public float BrokenSpeedMulti;
        public float VisionDistance;
        public float AttackDistance;
        public float GapDistance;
        public float AtkAniSpeedMulti;
        [Range(0f, 1f)] public float PointAtkTime;
        public float AtkHoldTime;
        
        [Title("HitBoxes")]
        public int ShieldDmgMulti;
        public int HeadDmgMulti;
        public int BodyDmgMulti;
    }
}