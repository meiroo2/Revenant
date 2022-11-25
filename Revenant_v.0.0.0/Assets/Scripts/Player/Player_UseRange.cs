using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

/*
public class UseableObjInfo
{
    public int m_ObjID;
    public IUseableObj m_ObjScript;
    public Vector2 m_ObjPos;
    public GameObject m_Obj;

    public UseableObjInfo(int _ObjID, IUseableObj _ObjScript, Vector2 _ObjPos, GameObject _obj)
    {
        m_ObjID = _ObjID;
        m_ObjScript = _ObjScript;
        m_ObjPos = _ObjPos;
        m_Obj = _obj;
    }
}
*/

public class Player_UseRange : MonoBehaviour
{
    // Visible Member Variables


    // Member Variables
    private Player m_Player;

    private List<Collider2D> m_UseableObjList = new List<Collider2D>();
    private List<Collider2D> m_HideSlotList;
    private HideSlot m_CurHideSlot = null;
    private bool m_IsHide = false;

    private float m_UseDelayTime = 0.5f;
    private bool m_UseDelay = false;
    private Coroutine m_UseDelayCoroutine;
    
    

    // Constructors
    private void Awake()
    {
        m_HideSlotList = new List<Collider2D>();
        m_Player = GetComponentInParent<Player>();
    }
    

    // Updates


    // Physics
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "UseableObj":
                if (collision.TryGetComponent(out IUseableObj iUse))
                {
                    if (iUse.m_ObjProperty == UseableObjList.JUSTTOUCH)
                    {
                        iUse.useObj(new IUseableObjParam(m_Player.transform, true, GetInstanceID()));
                    }
                    else
                    {
                        m_UseableObjList.Add(collision);
                    }
                }
                break;
            
            case "HideSlot":
                m_HideSlotList.Add(collision);
                HighlightbyDistance(true);
                break;
            
            case "CheckPoint":
                m_UseableObjList.Add(collision);
                break;
            
            default:
                Debug.Log("ERR : Player_UseRange에서 정의되지 않은 Tag 발견 " + collision.tag);
                m_UseableObjList.Add(collision);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "UseableObj":
                m_UseableObjList.Remove(collision);
                break;
            
            case "HideSlot":
                m_HideSlotList.Remove(collision);

                if (m_HideSlotList.Count > 0)
                {
                    HighlightbyDistance(true);
                }
                else if(!m_IsHide)
                {
                    //Debug.Log("CurHideSlot 초기화");
                    m_CurHideSlot = null;
                }
                break;
            
            case "CheckPoint":
                m_UseableObjList.Remove(collision);
                break;
            
            default:
                Debug.Log("ERR : Player_UseRange에서 정의되지 않은 Tag 발견");
                m_UseableObjList.Remove(collision);
                break;
        }
    }
    
    
    // Functions

    /// <summary>
    /// UseableObj를 사용합니다.
    /// </summary>
    /// <returns>0 = 실패, 1 = 성공</returns>
    public int UseNearestObj()
    {
        if (m_UseDelay || m_UseableObjList.Count <= 0)
            return 0;

        m_UseableObjList[GetNearestUseableObjIdx()].TryGetComponent(out IUseableObj IObj);
        if (IObj.useObj(new IUseableObjParam(m_Player.transform, true, GetInstanceID())) == 1)
        {
            m_UseDelay = true;
            m_UseDelayCoroutine = StartCoroutine(UseDelayCoroutine());
            return 1;
        }
        else
        {
            m_UseDelay = true;
            m_UseDelayCoroutine = StartCoroutine(UseDelayCoroutine());
            return 0;
        }
    }

    private IEnumerator UseDelayCoroutine()
    {
        yield return new WaitForSeconds(m_UseDelayTime);
        m_UseDelay = false;
    }

    private int GetNearestUseableObjIdx()
    {
        int nearestIdx = 0;
        float nearestDistance = 9999999f;
        
        for (int i = 0; i < m_UseableObjList.Count; i++)
        {
            float distance = Vector2.Distance(transform.position, 
                m_UseableObjList[i].transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestIdx = i;
            }
        }

        return nearestIdx;
    }

    private void HighlightbyDistance(bool _isTrue)
    {
        if (m_HideSlotList.Count <= 0)
            return;
        
        Vector2 playerPos = transform.position;

        int minIdx = 0;
        float minDistance = 999999999f;
        
        foreach (var element in m_HideSlotList)
        {
            float distance = (playerPos - (Vector2)element.transform.position).sqrMagnitude;
            if(distance < minDistance)
            {
                minIdx = m_HideSlotList.IndexOf(element);
                minDistance = distance;
            }
        }

        m_CurHideSlot = m_HideSlotList[minIdx].GetComponent<HideSlot>();
    }
    
    public void ForceExitFromHideSlot()
    {
        if (!m_IsHide)
            return;
        
        m_CurHideSlot.ActivateHideSlot(false);
        m_CurHideSlot = null;
        m_IsHide = false;
    }
    
    /// <summary>
    /// HideSlot에 숨도록 UseRange에게 요청합니다.
    /// </summary>
    /// <param name="_doHide">숨기 / 벗어나기</param>
    /// <returns>0 = 실패, 1 = 성공</returns>
    public int DoHide(bool _doHide)
    {
        if (_doHide)
        {
            if (m_IsHide || m_HideSlotList.Count <= 0)
                return 0;

            m_IsHide = true;
            m_HideSlotList[GetNearestHideSlotIdx()].TryGetComponent(out HideSlot slot);
            //Debug.Log("CurHideSLot 설정");
            m_CurHideSlot = slot;
            m_CurHideSlot.ActivateHideSlot(true);
            return 1;
        }
        else
        {
            if (!m_IsHide)
                return 0;
            
            m_IsHide = false;
            //Debug.Log("CurHideSLot 해제");
            m_CurHideSlot.ActivateHideSlot(false);
            m_CurHideSlot = null;
            return 1;
        }
    }
    
    /// <summary>
    /// 가장 가까운 Slot Idx을 리턴합니다.
    /// </summary>
    /// <returns>Nearest Slot Idx</returns>
    private int GetNearestHideSlotIdx()
    {
        float nearestDistance = 9999999999f;
        int nearestIdx = 0;

        for (int i = 0; i < m_HideSlotList.Count; i++)
        {
            float distanceBetSlot = Vector2.Distance(m_Player.GetPlayerCenterPos(), 
                m_HideSlotList[i].transform.position);

            if (distanceBetSlot < nearestDistance)
            {
                nearestDistance = distanceBetSlot;
                nearestIdx = i;
            }
        }

        return nearestIdx;
    }

    private void CalObjOutline()
    {
        /*
        for (int i = 0; i < m_UseableObjList.Count; i++)
        {
            if (i == m_ShortestIDX && m_UseableObjList[i].m_ObjScript.m_isOn == false)
            {
                m_UseableObjList[i].m_ObjScript.ActivateOutline(true);
            }
            else
            {
                m_UseableObjList[i].m_ObjScript.ActivateOutline(false);
            }
        }
        */
    }
}
