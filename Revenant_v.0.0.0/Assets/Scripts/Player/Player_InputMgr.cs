using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;


public class Player_InputMgr : MonoBehaviour
{
    // Visible Member Variables

    #region Lock_Variables

    private bool m_LocalMoveInputLock = false;
    public bool p_MoveInputLock
    {
        get { return m_LocalMoveInputLock; }
        set
        {
            if (value)
            {
                m_IsPushLeftKey = false;
                m_IsPushRightKey = false;
            }

            m_LocalMoveInputLock = value;
        }
    }

    
    private bool m_LocalFireLock = false;
    public bool p_FireLock
    {
        get { return m_LocalFireLock; }
        set
        {
            if (value)
                m_IsPushAttackKey = false;

            m_LocalFireLock = value;
        }
    }

    
    private bool m_LocalSideAttackLock = false;
    public bool p_SideAttackLock
    {
        get { return m_LocalSideAttackLock; }
        set
        {
            if (value)
                m_IsPushSideAttackKey = false;

            m_LocalSideAttackLock = value;
        }
    }

    
    private bool m_LocalReloadLock = false;
    public bool p_ReloadLock
    {
        get { return m_LocalReloadLock; }
        set
        {
            if (value)
                m_IsPushReloadKey = false;

            m_LocalReloadLock = value;
        }
    }


    private bool m_LocalRollLock = false;
    public bool p_RollLock
    {
        get { return m_LocalRollLock; }
        set
        {
            if (value)
                m_IsPushRollKey = false;

            m_LocalRollLock = value;
        }
    }
    

    private bool m_LocalBulletTimeLock = false;
    public bool p_BulletTimeLock
    {
        get { return m_LocalBulletTimeLock; }
        set
        {
            if (value)
                m_IsPushBulletTimeKey = false;

            m_LocalBulletTimeLock = value;
        }
    }
    

    private bool m_LocalStairLock = false;
    public bool p_StairLock
    {
        get { return m_LocalStairLock; }
        set
        {
            if (value)
            {
                m_IsPushStairDownKey = false;
                m_IsPushStairUpKey = false;
            }

            m_LocalStairLock = value;
        }
    }
    

    private bool m_LocalInteractLock = false;
    public bool p_InteractLock
    {
        get { return m_LocalInteractLock; }
        set
        {
            if (value)
                m_IsPushInteractKey = false;

            m_LocalInteractLock = value;
        }
    }


    private bool m_LocalHideLock = false;
    public bool p_HideLock
    {
        get { return m_LocalHideLock; }
        set
        {
            if (value)
                m_IsPushHideKey = false;

            m_LocalHideLock = value;
        }
    }

    
    #endregion
    
    
    
    
    
    public bool p_MousePosLock = false;


    // Member Variables
    public bool m_IsPushLeftKey { get; private set; }
    public bool m_IsPushRightKey { get; private set; }
    public bool m_IsPushStairUpKey { get; private set; }
    public bool m_IsPushStairDownKey { get; private set; }
    public bool m_IsPushInteractKey { get; private set; }
    public bool m_IsPushRollKey { get; private set; }
    public bool m_IsPushHideKey { get; private set; }
    public bool m_IsPushReloadKey { get; private set; }
    public bool m_IsPushAttackKey { get; private set; }
    public bool m_IsPushSideAttackKey { get; private set; }
    public bool m_IsPushBulletTimeKey { get; private set; }
    public Vector2 m_MousePos { get; private set; }

    
    
    // Actions
    
    // Attack
    private bool m_ActionOnAttack = false;
    private Action m_AttackAction = null;
    
    
    
    // 좌우 이동
    private bool m_LKeyInput = false;
    private bool m_RKeyInput = false;

    // 계단 업다운
    private bool m_UKeyInput = false;
    private bool m_DKeyInput = false;

    private Coroutine m_InputCoroutine = null;
    
    
    // Constructors
    private void Awake()
    {
        m_InputCoroutine = StartCoroutine(CoroutineUpdate());
    }
    private void OnDisable()
    {
        if (!ReferenceEquals(m_InputCoroutine, null))
        {
            StopCoroutine(m_InputCoroutine);
            m_InputCoroutine = null;
        }
    }
    private void OnEnable()
    {
        if (ReferenceEquals(m_InputCoroutine, null))
            m_InputCoroutine = StartCoroutine(CoroutineUpdate());
    }


    // Updates
    private IEnumerator CoroutineUpdate()
    {
        while (true)
        {
            if (!m_LocalMoveInputLock)
                CalculateDirectinalKey();

            if (!p_StairLock)
                CalculateStairKey();

            if (!p_InteractLock)
                m_IsPushInteractKey = Input.GetKey(KeyCode.F);

            if (!p_RollLock)
                m_IsPushRollKey = Input.GetKey(KeyCode.Space);

            if (!p_HideLock)
                m_IsPushHideKey = Input.GetKey(KeyCode.S);

            if (!p_ReloadLock)
                m_IsPushReloadKey = Input.GetKey(KeyCode.R);

            if (!p_FireLock)
            {
                m_IsPushAttackKey = Input.GetMouseButtonDown(0);
                
                if (m_ActionOnAttack && m_IsPushAttackKey)
                    m_AttackAction?.Invoke();
            }

            if (!p_SideAttackLock)
                m_IsPushSideAttackKey = Input.GetMouseButtonDown(1);

            if (!p_BulletTimeLock)
                m_IsPushBulletTimeKey = Input.GetKeyDown(KeyCode.Q);

            if (!p_MousePosLock)
                m_MousePos = Input.mousePosition;
            
            yield return null;
        }
    }


    // Physics


    // Functions
    
    /// <summary>
    /// Attack(발사) 후에 실행할 함수를 Action으로 받습니다.
    /// </summary>
    /// <param name="_action"></param>
    public void SetAttackAction(Action _action)
    {
        m_AttackAction = null;
        m_AttackAction = _action;
        m_ActionOnAttack = true;
    }

    public void ResetAttackAction()
    {
        m_AttackAction = null;
        m_ActionOnAttack = false;
    }
    
    public void SetAllLockByBool(bool _toLock)
    {
        p_FireLock = _toLock;
        p_HideLock = _toLock;
        p_InteractLock = _toLock;
        p_ReloadLock = _toLock;
        p_RollLock = _toLock;
        p_StairLock = _toLock;
        p_BulletTimeLock = _toLock;
        p_MousePosLock = _toLock;
        p_MoveInputLock = _toLock;
        p_SideAttackLock = _toLock;
    }
    public void SetAllInputLock(bool _toLock)
    {
        if (_toLock)
        {
            if (!ReferenceEquals(m_InputCoroutine, null))
            {
                StopCoroutine(m_InputCoroutine);
                m_InputCoroutine = null;
            }

            ForceSetAllKey(false);
        }
        else
        {
            if (!ReferenceEquals(m_InputCoroutine, null))
            {
                StopCoroutine(m_InputCoroutine);
                m_InputCoroutine = null;
            }
            m_InputCoroutine = StartCoroutine(CoroutineUpdate());
            ForceSetAllKey(true);
        }
    }
    /// <summary>
    /// 방향키 값을 리턴합니다. -1, 0, 1 = 좌, X, 우
    /// </summary>
    /// <returns></returns>
    public int GetDirectionalKeyInput()
    {
        if (m_IsPushLeftKey)
            return -1;
        else if (m_IsPushRightKey)
            return 1;
        else
            return 0;
    }
    private void CalculateStairKey()
    {
        m_UKeyInput = Input.GetKey(KeyCode.W);
        m_DKeyInput = Input.GetKey(KeyCode.S);

        switch (m_UKeyInput)
        {
            case true when !m_DKeyInput:
                m_IsPushStairUpKey = true;
                m_IsPushStairDownKey = false;
                break;
            
            case false when m_DKeyInput:
                m_IsPushStairUpKey = false;
                m_IsPushStairDownKey = true;
                break;
            
            default:
                m_IsPushStairUpKey = false;
                m_IsPushStairDownKey = false;
                break;
        }
    }
    private void CalculateDirectinalKey()
    {
        m_LKeyInput = Input.GetKey(KeyCode.A);
        m_RKeyInput = Input.GetKey(KeyCode.D);

        switch (m_LKeyInput)
        {
            case true when !m_RKeyInput:
                m_IsPushLeftKey = true;
                m_IsPushRightKey = false;
                break;
            
            case false when m_RKeyInput:
                m_IsPushLeftKey = false;
                m_IsPushRightKey = true;
                break;
            
            default:
                m_IsPushLeftKey = false;
                m_IsPushRightKey = false;
                break;
        }
    }

    private void ForceSetAllKey(bool _input)
    {
        p_FireLock = false;
        p_HideLock = false;
        p_InteractLock = false;
        p_ReloadLock = false;
        p_RollLock = false;
        p_StairLock = false;
        p_BulletTimeLock = false;
        p_MousePosLock = false;
        p_MoveInputLock = false;
        p_SideAttackLock = false;
    }
    // 기타 분류하고 싶은 것이 있을 경우
}