using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FootMgr : MonoBehaviour
{
    private Player m_Player;
    private Vector2 m_FootRayPos;
    private Player_InputMgr m_InputMgr;
    private StairPos m_StairPos = null;

    
    private bool m_isOnStair = false;
    private Vector2 m_jumpPos;
    private RaycastHit2D m_FootHit;
    private int m_LayerMask;

    public Vector2 m_PlayerNormal { get; private set; }

    
    private Coroutine m_StairPosCoroutine;
    private Coroutine m_StairCoroutine;

    private const float m_SensorXGap = 0.05f;
    private const float m_SensorYGap = 0.01f;
    
    
    
    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_Player = instance.GetComponentInChildren<Player_Manager>().m_Player;
        m_InputMgr = m_Player.m_InputMgr;

        m_LayerMask = LayerMask.GetMask("Floor");
        m_PlayerNormal = Vector2.up;
    }
    
    
    private void Update()
    {
        m_FootRayPos = new Vector2(transform.position.x, transform.position.y + 0.05f);
        
        switch (m_Player.gameObject.layer)
        {
            case 10:
                m_LayerMask = LayerMask.GetMask("Stair");
                break;
            
            case 12:
                m_LayerMask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("EmptyFloor"));
                break;
        }

        m_FootHit = Physics2D.Raycast(m_FootRayPos, -transform.up, 0.3f, m_LayerMask);
        
        Debug.DrawRay(m_FootRayPos, -transform.up * 0.3f, Color.blue, 0.05f);
        m_PlayerNormal = m_FootHit.normal;
    }

    public RaycastHit2D GetFootRayHit()
    {
        return m_FootHit;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (m_isOnStair)
            return;
        
        if (col.TryGetComponent(out StairPos element))
        {
            switch (element.m_IsUpPos)
            {
                case true:
                    m_Player.m_WorldUI.PrintSprite(0);
                    break;
                
                case false:
                    m_Player.m_WorldUI.PrintSprite(1);
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out StairPos element))
        {
            m_Player.m_WorldUI.PrintSprite(-1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (m_isOnStair)
            return;
        
        
        if (m_InputMgr.m_IsPushStairUpKey)
        {
            if (!collision.TryGetComponent(out StairPos DownPos)) 
                return;
            
            if (DownPos.m_IsUpPos == true)
                return;

            m_StairPos = DownPos;
            m_StairPos.m_ParentStair.MoveOrder(16);
            
            Debug.Log("���� �ö󰡱� ����");
            m_Player.m_WorldUI.PrintSprite(-1);
            
            m_Player.GoToStairLayer(true);
            m_isOnStair = true;

            if(!ReferenceEquals(m_StairPosCoroutine, null))
                StopCoroutine(m_StairPosCoroutine);
            m_StairPosCoroutine = StartCoroutine(StairPosCoroutine(false));
        }
        else if (m_InputMgr.m_IsPushStairDownKey)
        {
            if (!collision.TryGetComponent(out StairPos UpPos)) 
                return;
            
            if (UpPos.m_IsUpPos == false)
                return;
            
            m_StairPos = UpPos;
            m_StairPos.m_ParentStair.MoveOrder(16);
            
            Debug.Log("�Ʒ��� �������� ����");
            m_Player.m_WorldUI.PrintSprite(-1);
            
            m_Player.GoToStairLayer(true);
            m_isOnStair = true;
            
            if(!ReferenceEquals(m_StairPosCoroutine, null))
                StopCoroutine(m_StairPosCoroutine);
            m_StairPosCoroutine = StartCoroutine(StairPosCoroutine(true));
        }
    }

    private IEnumerator StairPosCoroutine(bool _isUp)
    {
        float stairPosX = m_StairPos.transform.position.x;
        bool isLeftUp = m_StairPos.m_ParentStair.m_isLeftUp;

        if (_isUp)
        {
            while (true)
            {
                if (transform.position.y < m_StairPos.transform.position.y - m_SensorYGap)
                {
                    Debug.Log("������ Y���� ������");
                    
                    if(!ReferenceEquals(m_StairCoroutine, null))
                        StopCoroutine(m_StairCoroutine);
                    m_StairCoroutine = StartCoroutine(StairCoroutine(true));
                    break;
                }

                if (isLeftUp)
                {
                    // ������ X��ǥ���� ���� == Ű�ٿ��� �������� ����
                    if (transform.position.x < stairPosX - m_SensorXGap)
                    {
                        m_StairPos.m_ParentStair.MoveOrder(16);
                    
                        Debug.Log("���������ٰ� �������� ����");
                        m_Player.m_WorldUI.PrintSprite(-1);
                        
                        m_Player.GoToStairLayer(false);
                        m_isOnStair = false;
                        m_StairPos = null;
                        break;
                    }
                }
                else
                {
                    // ������ X��ǥ���� ������ == Ű�ٿ��� ���������� ����
                    if (transform.position.x > stairPosX + m_SensorXGap)
                    {
                        m_StairPos.m_ParentStair.MoveOrder(16);
                    
                        Debug.Log("���������ٰ� ���������� ����");
                        m_Player.m_WorldUI.PrintSprite(-1);
                        
                        m_Player.GoToStairLayer(false);
                        m_isOnStair = false;
                        m_StairPos = null;
                        break;
                    }
                }

                if (!m_InputMgr.m_IsPushStairDownKey)
                {
                    m_StairPos.m_ParentStair.MoveOrder(16);
                    
                    Debug.Log("���������ٰ� Ű ��");
                    m_Player.m_WorldUI.PrintSprite(-1);
                    
                    m_Player.GoToStairLayer(false);
                    m_isOnStair = false;
                    m_StairPos = null;
                    break;
                }
                
                yield return null;
            }
        }
        else
        {
            while (true)
            {
                 // �Ʒ� -> �� �ö󰡴� ��
                if (transform.position.y > m_StairPos.transform.position.y + m_SensorYGap)
                {
                    Debug.Log("�Ʒ����� Y��ǥ���� �ö��");
                    if(!ReferenceEquals(m_StairCoroutine, null))
                        StopCoroutine(m_StairCoroutine);
                    m_StairCoroutine = StartCoroutine(StairCoroutine(false));
                    break;
                }

                if (isLeftUp)
                {
                    if (transform.position.x > stairPosX + m_SensorXGap)
                    {
                        m_StairPos.m_ParentStair.MoveOrder(10);
                    
                        Debug.Log("�ö󰡷��ٰ� ���������� ����");
                        m_Player.m_WorldUI.PrintSprite(-1);
                        
                        m_Player.GoToStairLayer(false);
                        m_isOnStair = false;
                        m_StairPos = null;
                        break;
                    }
                }
                else
                {
                    if (transform.position.x < stairPosX - m_SensorXGap)
                    {
                        m_StairPos.m_ParentStair.MoveOrder(10);
                    
                        Debug.Log("�ö󰡷��ٰ� �������� ����");
                        m_Player.m_WorldUI.PrintSprite(-1);
                        
                        m_Player.GoToStairLayer(false);
                        m_isOnStair = false;
                        m_StairPos = null;
                        break;
                    }
                }
               

                if (!m_InputMgr.m_IsPushStairUpKey)
                {
                    m_StairPos.m_ParentStair.MoveOrder(10);
                    
                    Debug.Log("�ö󰡷��ٰ� Ű ��");
                    m_Player.m_WorldUI.PrintSprite(-1);
                    
                    m_Player.GoToStairLayer(false);
                    m_isOnStair = false;
                    m_StairPos = null;
                    break;
                }
                
                yield return null;
            }
        }

        yield break;
    }

    private IEnumerator StairCoroutine(bool _startFromUp)
    {
        var UpPosX = m_StairPos.m_ParentStair.m_UpPos.transform.position.x;
        var DownPosX = m_StairPos.m_ParentStair.m_DownPos.transform.position.x;

        if (m_StairPos.m_ParentStair.m_isLeftUp)
        {
            while (true)
            {
                if (transform.position.x <= UpPosX) // �� �������� ����
                {
                    m_StairPos.m_ParentStair.MoveOrder(16);

                    Debug.Log("���ƿӴ�");
                    m_Player.m_WorldUI.PrintSprite(-1);
                
                    m_Player.GoToStairLayer(false);
                    m_isOnStair = false;
                    m_StairPos = null;
                    break;
                }
                else if (transform.position.x >= DownPosX)  // �Ʒ� �������� ������
                {
                    m_StairPos.m_ParentStair.MoveOrder(10);

                    Debug.Log("���ƿӴ�");
                    m_Player.m_WorldUI.PrintSprite(-1);
                
                    m_Player.GoToStairLayer(false);
                    m_isOnStair = false;
                    m_StairPos = null;
                    break;
                }

                yield return null;
            }
        }
        else
        {
            while (true)
            {
                if (transform.position.x >= UpPosX)
                {
                    m_StairPos.m_ParentStair.MoveOrder(16);

                    Debug.Log("���ƿӴ�");
                    m_Player.m_WorldUI.PrintSprite(-1);
                
                    m_Player.GoToStairLayer(false);
                    m_isOnStair = false;
                    m_StairPos = null;
                    break;
                }
                else if (transform.position.x <= DownPosX)
                {
                    m_StairPos.m_ParentStair.MoveOrder(10);

                    Debug.Log("���ƿӴ�");
                    m_Player.m_WorldUI.PrintSprite(-1);
                
                    m_Player.GoToStairLayer(false);
                    m_isOnStair = false;
                    m_StairPos = null;
                    break;
                }

                yield return null;
            }
        }
        
        yield break;
    }
}