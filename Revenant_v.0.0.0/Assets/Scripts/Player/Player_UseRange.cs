using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseableObjInfo
{
    public int m_ObjID;
    public IUseableObj m_ObjScript;
    public Vector2 m_ObjPos;

    public UseableObjInfo(int _ObjID, IUseableObj _ObjScript, Vector2 _ObjPos)
    {
        m_ObjID = _ObjID;
        m_ObjScript = _ObjScript;
        m_ObjPos = _ObjPos;
    }
}

public class Player_UseRange : MonoBehaviour
{
    // Visible Member Variables


    // Member Variables
    private bool isPressedFKey = false;
    private float FTimer = 0.1f;

    private List<UseableObjInfo> m_UseableObjs = new List<UseableObjInfo>();
    private int m_ShortestIDX = -1;
    private float m_ShortestLength = 999f;
    private Player m_Player;

    private IUseableObjParam m_UseableObjParam;

    // Constructors
    private void Awake()
    {
        m_Player = GetComponentInParent<Player>();
        m_UseableObjParam = new IUseableObjParam(m_Player.transform, true, m_Player.GetInstanceID());
    }

    // Updates
    private void Update()
    {
        if (isPressedFKey)
        {
            FTimer -= Time.deltaTime;
            if (FTimer <= 0f)
            {
                FTimer = 0.1f;
                isPressedFKey = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
            isPressedFKey = true;
    }


    // Physics
    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_UseableObjs.Add(new UseableObjInfo(collision.gameObject.GetInstanceID(), collision.GetComponent<IUseableObj>(), collision.transform.position));
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (m_UseableObjs.Count > 0)
        {
            m_ShortestLength = 999f;

            for (int i = 0; i < m_UseableObjs.Count; i++)
            {
                if (m_ShortestLength > ((Vector2)transform.position - m_UseableObjs[i].m_ObjPos).sqrMagnitude)
                {
                    m_ShortestLength = ((Vector2)transform.position - m_UseableObjs[i].m_ObjPos).sqrMagnitude;
                    m_ShortestIDX = i;
                }
            }
        }

        if (isPressedFKey && m_ShortestLength < 999f)
        {
            switch (m_UseableObjs[m_ShortestIDX].m_ObjScript.m_ObjProperty)
            {
                case UseableObjList.OBJECT:
                    m_UseableObjs[m_ShortestIDX].m_ObjScript.useObj(m_UseableObjParam);
                    break;

                case UseableObjList.HIDEPOS:
                    if (Vector2.Distance(transform.position, collision.transform.position) > 0.4f)
                        break;

                    switch (m_UseableObjs[m_ShortestIDX].m_ObjScript.useObj(m_UseableObjParam))
                    {
                        case 0:
                            // 见扁 角菩
                            break;

                        case 1:
                            // 见扁 己傍
                            m_Player.changePlayerFSM(playerState.HIDDEN);
                            break;

                        case 2:
                            // 见扁 秦力
                            m_Player.changePlayerFSM(playerState.IDLE);
                            break;
                    }
                    break;
            }
            isPressedFKey = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (m_UseableObjs.Count > 0)
        {
            for (int i = 0; i < m_UseableObjs.Count; i++)
            {
                if (m_UseableObjs[i].m_ObjID == collision.gameObject.GetInstanceID())
                {
                    m_UseableObjs.RemoveAt(i);
                    break;
                }
            }

            if (m_UseableObjs.Count == 0)
                m_ShortestIDX = -1;
        }
    }

    // Functions


}
