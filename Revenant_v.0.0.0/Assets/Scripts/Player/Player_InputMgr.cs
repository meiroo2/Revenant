using System;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class Player_InputMgr : MonoBehaviour
{
    // Visible Member Variables
    public bool p_isPushLeftKey { get; private set; }
    public bool p_isPushRightKey { get; private set; }
    public bool p_isPushStairUpKey { get; private set; }
    public bool p_isPushStairDownKey { get; private set; }
    public bool p_isPushInteractKey { get; private set; }


    // Member Variables
    private float m_DirectinalKeyInput = 0f;

    
    // Constructors


    // Updates
    private void Update()
    {
        CalculateDirectinalKey();

        p_isPushStairUpKey = Input.GetKey(KeyCode.W);
        p_isPushStairDownKey = Input.GetKey(KeyCode.S);
        p_isPushInteractKey = Input.GetKey(KeyCode.F);
    }

    // Physics


    // Functions
    private void CalculateDirectinalKey()
    {
        m_DirectinalKeyInput = Input.GetAxisRaw("Horizontal");
        
        switch (m_DirectinalKeyInput)
        {
            case > 0:
                p_isPushRightKey = true;
                p_isPushLeftKey = false;
                break;
            
            case < 0:
                p_isPushLeftKey = true;
                p_isPushRightKey = false;
                break;
            
            default:
                p_isPushLeftKey = false;
                p_isPushRightKey = false;
                break;
        }
    }

    // 기타 분류하고 싶은 것이 있을 경우
}