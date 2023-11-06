using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace VariableDB
{
    [CreateAssetMenu(fileName = "NormalGang_DB", menuName = "VariableDB/NormalGang_DB")]
    public class NormalGang_DB : Gang_DB
    {
        public int Hp;
        public float Speed;
        public int StunThreshold;
        public float VisionDistance;
        public float GunFireDistance;
        public float MeleeAtkDistance;
        public float AlertSpeed;
        public float StunAlertSpeed;
        public int HeadDmgMulti;
        public int BodyDmgMulti;
        
        [Title("TripleShot")]
        public int BulletDmg;
        public float FireDelay;
        public float BulletSpeed;
        public float BulletRndRotation;
    }
}