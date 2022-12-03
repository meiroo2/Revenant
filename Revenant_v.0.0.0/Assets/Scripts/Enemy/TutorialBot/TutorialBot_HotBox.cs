using System;
using System.ComponentModel;
using Sirenix.OdinInspector;
using UnityEngine;


public class TutorialBot_HotBox : MonoBehaviour, IHotBox, IMatType
{
    // Visible Member Variables
    [field: SerializeField, BoxGroup("TutorialBot")] public MatType m_matType { get; set; }
    [field: SerializeField, BoxGroup("TutorialBot")] public HitBoxPoint m_HitBoxInfo { get; set; }
    [field: SerializeField, BoxGroup("TutorialBot")] public GameObject m_ParentObj { get; set; }
    [field: SerializeField, BoxGroup("TutorialBot")] public TutorialBot m_Enemy { get; private set; }

    
    // Member Variables
    private SoundPlayer m_SoundPlayer;
    private HitSFXMaker m_HitSFXMaker;
    public int m_hotBoxType { get; set; } = 0;
    public bool m_isEnemys { get; set; } = true;
    
    
    // Constructors
    private void Start()
    {
        m_SoundPlayer = GameMgr.GetInstance().p_SoundPlayer;
        m_HitSFXMaker = InstanceMgr.GetInstance().GetComponentInChildren<HitSFXMaker>();
    }


    // Functions
    public int HitHotBox(IHotBoxParam _param)
    {
        m_SoundPlayer.PlayHitSoundByMatType(m_matType, transform);
        m_HitSFXMaker.EnableNewObj(0, _param.m_contactPoint);
		m_Enemy.AttackedByWeapon(m_HitBoxInfo, _param.m_Damage, _param.m_stunValue, _param.m_weaponType);
		m_SoundPlayer.PlayHitSoundByMatType(m_matType, transform);

		return 1;
    }
}