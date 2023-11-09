using System;
using UnityEngine;

namespace Enemy.BossGang.TimeSlice
{
    public class TimeSliceCol : ObjectDefine, IMatType, IHotBox
    {
        public MatType m_matType { get; set; } = MatType.Normal;
        
        public GameObject m_ParentObj { get; set; }
        public int m_hotBoxType { get; set; } = 1;
        public bool m_isEnemys { get; set; } = false;
        public HitBoxPoint m_HitBoxInfo { get; set; } = HitBoxPoint.FLOOR;
        private SoundPlayer m_SoundPlayer;
        // Member Variables
        private HitSFXMaker m_HitSFXMaker;
        private float m_SkillLockTime = 0f;
        
        
        // Constructors
        private void Awake()
        {
            m_ParentObj = this.gameObject;
            m_ObjectType = ObjectType.Floor;
            m_CanAttacked = true;
        }
        private void Start()
        {
            var instance = InstanceMgr.GetInstance();

            m_HitSFXMaker = instance.GetComponentInChildren<HitSFXMaker>();
            m_SoundPlayer = GameMgr.GetInstance().p_SoundPlayer;
        }
        
        public int HitHotBox(IHotBoxParam _param)
        {
            m_SoundPlayer.PlayHitSoundByMatType(m_matType, _param.m_contactPoint);

            m_HitSFXMaker.EnableNewObj(0, _param.m_contactPoint);
        
            return 1;
        }

        public void SetSkillLockTime(float skillLockTime)
        {
            m_SkillLockTime = skillLockTime;
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out Player player))
            {
               player.m_RageGauge.LockSkill(SkillType.Roll, m_SkillLockTime);
               player.m_RageGauge.LockSkill(SkillType.Melee, m_SkillLockTime);
            }
        }
    }
}