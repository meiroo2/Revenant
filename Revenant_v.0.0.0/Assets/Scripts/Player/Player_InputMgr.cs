using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class Player_InputMgr : MonoBehaviour
{
    // Visible Member Variables
    public bool m_IsPushLeftKey { get; private set; }
    public bool m_IsPushRightKey { get; private set; }
    public bool m_IsPushStairUpKey { get; private set; }
    public bool m_IsPushStairDownKey { get; private set; }
    public bool m_IsPushInteractKey { get; private set; }
    public bool m_IsPushRollKey { get; private set; }
    public bool m_IsPushHideKey { get; private set; }


    // Member Variables
    public bool m_InputLock { get; set; } = false;
    private bool m_LKeyInput = false;
    private bool m_RKeyInput = false;

    private bool m_UKeyInput = false;
    private bool m_DKeyInput = false;

    private Coroutine m_InputCoroutine;
    
    // Constructors
    private void Awake()
    {
        m_InputCoroutine = StartCoroutine(CoroutineUpdate());
    }
    private void OnDisable()
    {
        StopCoroutine(m_InputCoroutine);
    }


    // Updates
    private IEnumerator CoroutineUpdate()
    {
        while (true)
        {
            if (!m_InputLock)
            {
                CalculateDirectinalKey();
                CalculateStairKey();
                m_IsPushInteractKey = Input.GetKey(KeyCode.F);
                m_IsPushRollKey = Input.GetKey(KeyCode.Space);
                m_IsPushHideKey = Input.GetKey(KeyCode.S);
            }
            else
            {
                ForceSetAllKey(false);
            }
            yield return null;
        }
        // ReSharper disable once IteratorNeverReturns
    }

    
    // Physics


    // Functions
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
        m_IsPushLeftKey = _input;
        m_IsPushRightKey = _input;
        m_IsPushStairUpKey = _input;
        m_IsPushStairDownKey = _input;
        m_IsPushInteractKey = _input;
        m_IsPushRollKey = _input;
        m_IsPushHideKey = _input;
    }
    // 기타 분류하고 싶은 것이 있을 경우
}