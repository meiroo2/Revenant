
using Sirenix.OdinInspector;
using UnityEngine;

namespace VariableDB
{
    [CreateAssetMenu(fileName = "Player_DB", menuName = "VariableDB/Player_DB")]
    public class Player_DB : Variable_DB
    {
        #region Player

        [field: SerializeField, TitleGroup("Player")]
        public int HP { get; private set; }

        [field: SerializeField, TitleGroup("Player")]
        public float StunInvincibleTime { get; private set; }

        [field: SerializeField, TitleGroup("Player")]
        public float Speed { get; private set; }

        [field: SerializeField, TitleGroup("Player")]
        public float BackSpeedMulti { get; private set; }

        [field: SerializeField, TitleGroup("Player")]
        public float RollSpeedMulti { get; private set; }

        [field: SerializeField, TitleGroup("Player")]
        public float RollDecelerationSpeed { get; private set; }

        [field: SerializeField, TitleGroup("Player")]
        public float ReloadSpeed { get; private set; }

        [field: SerializeField, TitleGroup("Player")]
        public float MeleeSpeedMulti { get; private set; }

        [field: SerializeField, TitleGroup("Player")]
        public int MeleeDamage { get; private set; }

        [field: SerializeField, TitleGroup("Player")]
        public int MeleeStunValue { get; private set; }

        [field: SerializeField, TitleGroup("Player"), Range(0f, 1f)]
        public float JustEvadeStartTime { get; private set; }

        [field: SerializeField, TitleGroup("Player"), Range(0f, 1f)]
        public float JustEvadeEndTime { get; private set; }

        #endregion



        #region Negotiator

        [field: SerializeField, TitleGroup("Negotiator")]
        public int Damage { get; private set; }

        [field: SerializeField, TitleGroup("Negotiator")]
        public int StunValue { get; private set; }

        [field: SerializeField, TitleGroup("Negotiator")]
        public float MinFireDelay { get; private set; }

        #endregion



        #region RageGauge

        [field: SerializeField, TitleGroup("RageGauge")]
        public float Gauge_Max { get; private set; } // 게이지의 최대 수치

        [field: SerializeField, TitleGroup("RageGauge")]
        public float Gauge_Refill_Nature { get; private set; }  // 1초마다 자동으로 충전되는 게이지의 양

        [field: SerializeField, TitleGroup("RageGauge")]
        public float Gauge_Refill_Attack_Multi { get; private set; }  // 피해량 비례 게이지 충전율

        [field: SerializeField, TitleGroup("RageGauge")]
        public float Gauge_Refill_Evade { get; private set; }  // 회피시 게이지 충전량

        [field: SerializeField, TitleGroup("RageGauge")]
        public float Gauge_Refill_JustEvade { get; private set; }  // 저스트 회피시 게이지 충전량

        [field: SerializeField, TitleGroup("RageGauge")]
        public float Gauge_Refill_Limit { get; private set; }  // 자연회복 및 자연감소

        [field: SerializeField, TitleGroup("RageGauge")]
        public float Gauge_Consume_Nature { get; private set; } // 1초마다 자동으로 감소되는 게이지의 양

        [field: SerializeField, TitleGroup("RageGauge")]
        public float Gauge_Consume_Roll { get; private set; }  // 구르기시 소모되는 게이지의 양

        [field: SerializeField, TitleGroup("RageGauge")]
        public float Gauge_Consume_Melee { get; private set; } 

        #endregion

        
        
        #region BulletTime

        [field: SerializeField, TitleGroup("BulletTime")]
        public float BulletTimeLimit { get; private set; }

        [field: SerializeField, TitleGroup("BulletTime")]
        public float ShotDelayTime { get; private set; }

        #endregion
    }
}