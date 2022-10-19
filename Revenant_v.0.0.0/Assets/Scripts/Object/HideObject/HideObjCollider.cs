using System;
using Unity.VisualScripting;
using UnityEngine;


public class HideObjCollider : MonoBehaviour, IHotBox, IMatType
{
    // Visible Member Variables
    [Tooltip("해당 히트박스의 재질을 결정합니다.")]
    [field: SerializeField] public MatType m_matType { get; set; }
    
    // Member Variables
    public HideObj m_HideObj { get; set; }
    private BoxCollider2D m_Collider;
    private SoundPlayer m_SoundPlayer;
    
        // IHotBox Variables
        public GameObject m_ParentObj { get; set; }
        public int m_hotBoxType { get; set; } = 1;
        public bool m_isEnemys { get; set; } = false;
        public HitBoxPoint m_HitBoxInfo { get; set; } = HitBoxPoint.OBJECT;
        
        
    // Constructors
    private void Awake()
    {
        m_Collider = GetComponent<BoxCollider2D>();
        SetHideObjCollider(false);
    }
    private void Start()
    {
        m_ParentObj = m_HideObj.gameObject;
        
        var instanceMgr = InstanceMgr.GetInstance();
        m_SoundPlayer = GameMgr.GetInstance().p_SoundPlayer;
    }

    
    // Functions
    public void SetHideObjCollider(bool _isOn)
    {
        m_Collider.enabled = _isOn;
    }
    public int HitHotBox(IHotBoxParam _param)
    {
        switch (m_HideObj.GetHit(_param))
        {
            case 0:
                // 충돌처리 실패
                return 0;
                break;
            
            case 1:
                // 충돌처리 성공, 소리 contactPoint에서 재생
                m_SoundPlayer.PlayHitSoundByMatType(m_matType, _param.m_contactPoint);
                return 1;
                break;
            
            default:
                return 0;
        }
    }

   
}