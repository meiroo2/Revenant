using UnityEngine;

namespace VariableDB
{
    [CreateAssetMenu(fileName = "SpecialGang_DB", menuName = "VariableDB/SpecialGang_DB")]
    public class SpecialGang_DB : Variable_DB
    {
        public int Hp;
        public int BulletDamage;
        public float BulletSpeed;
        public float BulletSpread;
        public float FireAnimSpeed;
        public float FireDelay;
        public float MoveSpeed;
        public float RunSpeedMulti;
        public float StunAlertSpeed;
        public int StunThreshold;
        public float VisionDistance;
        public float AttackDistance;
        public float MeleeRollDistance;
        public float GapDistance;
        public int HeadDmgMulti;
        public int BodyDmgMulti;
        public float Roll_Refresh;
        public Vector2 Roll_Tick;
        public int Roll_Chance;
        public float Roll_Cooldown;
        public float Roll_Speed_Multi;
        public float AlertSpeed;
        public float AlertFadeSpeed;
    }
}