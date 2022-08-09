using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class Door_LayerRoom : MonoBehaviour
{
    // Visible Member Variables
    public bool p_IsInitStateCanUse = true;
    [field: SerializeField] public Transform p_CenterPos { get; private set; }
    public Door_LayerRoom p_OtherSide;
    
    
    // Member Variables
    [ReadOnly, ShowInInspector] public bool m_CanUse { get; private set; } = false;
    public Animator m_Animator { get; private set; }
    private CameraMove m_MainCam;
    private Coroutine m_CurCoroutine = null;
    [HideInInspector] public bool m_IsOpen = false;

    
    // Hashes
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");
    private static readonly int CanUse = Animator.StringToHash("CanUse");


    // Constructors
    private void Awake()
    {
        m_MainCam = Camera.main.GetComponent<CameraMove>();

        if(p_CenterPos == null)
            Debug.Log(gameObject.name + "레이어룸 포탈의 CenterPos가 등록되어 있지 않음.");
            
        if (p_OtherSide == null)
            Debug.Log(gameObject.name + " 레이어룸 포탈의 OtherSide가 등록되어 있지 않음.");
        
        Animator animator;
        // ReSharper disable once AssignmentInConditionalExpression
        if (animator = GetComponentInChildren<Animator>())
            m_Animator = animator;
        else
            Debug.Log(gameObject.name + " 레이어룸 포탈의 Animator가 없음");

        if (p_IsInitStateCanUse)
        {
            m_CanUse = true;
            m_Animator.SetBool(CanUse, true);
        }
    }

    
    // Functions

    public void ActivateBothOutline(bool _isOn)
    {
        m_IsOpen = _isOn;
        PlayDoorAni(m_IsOpen);

        if (p_OtherSide.m_IsOpen != m_IsOpen)
        {
            p_OtherSide.m_IsOpen = m_IsOpen;
            p_OtherSide.PlayDoorAni(m_IsOpen);
        }
       
    }
    
    /// <summary> 레이어룸 문의 사용 가능 여부를 변경합니다. </summary>
    /// <param name="_canUse"> 사용 가능 여부 </param>
    public void ChangeCanUse(bool _canUse)
    {
        m_CanUse = _canUse;
        m_Animator.SetBool(CanUse, m_CanUse);
        
        p_OtherSide.m_CanUse = m_CanUse;
        p_OtherSide.m_Animator.SetBool(CanUse, m_CanUse);
    }

    /// <summary> 레이어룸 문의 열림/닫힘 애니메이션을 재생합니다. </summary>
    /// <param name="_isOpen"> true일 경우 열리는 애니 </param>
    public void PlayDoorAni(bool _isOpen)
    {
        if (ReferenceEquals(m_Animator, null))
            return;
        
        m_Animator.SetInteger(IsOpen, _isOpen ? 1 : 0);
    }

    /// <summary> 상호작용한 오브젝트를 반대 쪽에 위치한 레이어룸 문으로 이동시킵니다. </summary>
    /// <param name="_obj"> 이동할 오브젝트의 트랜스폼 </param>
    /// <param name="_isPlayer"> 플레이어 여부 </param>
    public void MoveToOtherSide(Transform _obj, bool _isPlayer)
    {
        if (!m_CanUse)
            return;
        
        
        // 트랜스폼 이동
        if (_isPlayer)
        {
            float yGapBetPlayernDoor = _obj.transform.position.y - p_CenterPos.position.y;
            Vector2 movePos = new Vector2(p_OtherSide.p_CenterPos.position.x, p_OtherSide.p_CenterPos.position.y + yGapBetPlayernDoor);

            _obj.position = movePos;
            m_MainCam.InstantMoveToPlayer(_obj.position, movePos);
        }
        else
        {
            float yGapBetEnemynDoor = _obj.transform.position.y - p_CenterPos.position.y;
            Vector2 movePos = new Vector2(p_OtherSide.p_CenterPos.position.x, p_OtherSide.p_CenterPos.position.y + yGapBetEnemynDoor);

            _obj.position = movePos;
        }
    }
}
