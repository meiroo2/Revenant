using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;
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
    private CameraMgr m_MainCam;
    private Coroutine m_CurCoroutine = null;
    [HideInInspector] public bool m_IsOpen = false;

    
    // Hashes
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");
    private static readonly int CanUse = Animator.StringToHash("CanUse");

    private Player _player { get; set; }
    private List<BasicEnemy> EnemyList;

    // Constructors
    private void Awake()
    {
        m_MainCam = Camera.main.GetComponent<CameraMgr>();
        EnemyList = FindObjectsOfType<BasicEnemy>().ToList();

        if(p_CenterPos == null)
            Debug.Log(gameObject.name + "?????? ????? CenterPos?? ????? ???? ????.");
            
        if (p_OtherSide == null)
            Debug.Log(gameObject.name + " ?????? ????? OtherSide?? ????? ???? ????.");
        
        Animator animator;
        // ReSharper disable once AssignmentInConditionalExpression
        if (animator = GetComponentInChildren<Animator>())
            m_Animator = animator;
        else
            Debug.Log(gameObject.name + " ?????? ????? Animator?? ????");

        if (p_IsInitStateCanUse)
        {
            m_CanUse = true;
            m_Animator.SetBool(CanUse, true);
        }
    }

    private void Start()
    {
        _player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
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
    
    /// <summary> ?????? ???? ??? ???? ???��? ????????. </summary>
    /// <param name="_canUse"> ??? ???? ???? </param>
    public void ChangeCanUse(bool _canUse)
    {
        m_CanUse = _canUse;
        m_Animator.SetBool(CanUse, m_CanUse);
        
        p_OtherSide.m_CanUse = m_CanUse;
        p_OtherSide.m_Animator.SetBool(CanUse, m_CanUse);
    }

    /// <summary> ?????? ???? ????/???? ????????? ???????. </summary>
    /// <param name="_isOpen"> true?? ??? ?????? ??? </param>
    public void PlayDoorAni(bool _isOpen)
    {
        if (ReferenceEquals(m_Animator, null))
            return;
        
        m_Animator.SetInteger(IsOpen, _isOpen ? 1 : 0);
    }

    /// <summary> ???????? ????????? ??? ??? ????? ?????? ?????? ?????????. </summary>
    /// <param name="_obj"> ????? ????????? ??????? </param>
    /// <param name="_isPlayer"> ?��???? ???? </param>
    public void MoveToOtherSide(Transform _obj, bool _isPlayer)
    {
        if (!m_CanUse)
            return;
        
        // ??????? ???
        if (_isPlayer)
        {
            float yGapBetPlayernDoor = _obj.transform.position.y - p_CenterPos.position.y;
            Vector2 movePos = new Vector2(p_OtherSide.p_CenterPos.position.x, p_OtherSide.p_CenterPos.position.y + yGapBetPlayernDoor);

            
            _obj.position = movePos;
            m_MainCam.InstantMoveToPlayer(_obj.position, movePos);

            foreach (var Enemy in EnemyList)
            {
                if (Enemy.m_CurEnemyFSM._enemyState == Enemy_FSM.EnemyState.Chase)
                {
                    Enemy.WayPointsVectorList.Add(p_CenterPos.position);
                    Enemy.bMoveToUsedDoor = true;
                }
            }

        }
        else
        {
            float yGapBetEnemynDoor = _obj.transform.position.y - p_CenterPos.position.y;
            Vector2 movePos = new Vector2(p_OtherSide.p_CenterPos.position.x, p_OtherSide.p_CenterPos.position.y + yGapBetEnemynDoor);

            _obj.position = movePos;
        }
    }
}
