using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Stair : MonoBehaviour
{
    // Visible Member Variables
    [field: SerializeField] public SpriteRenderer[] p_RendererArr;
    public StairPos m_UpPos { get; private set; }
    public StairPos m_DownPos { get; private set; }
    public bool p_InitSpriteOrderToObject = false;


    // Member Variables
    [ShowInInspector, ReadOnly] public bool m_isLeftUp { get; private set; } = true;

    public List<Stair> StairList;
    public int StairNum { get; set; } = 0;
    
    // Constructors
    private void Awake()
    {
        StairList = FindObjectsOfType<Stair>().ToList();
        
        m_isLeftUp = transform.localScale.x > 0 ? true : false;

        var StairPosArr = GetComponentsInChildren<StairPos>();
        if (StairPosArr.Length != 2)
            Debug.Log("ERR : Stair���� StairPos�� 2���� �ƴ�");

        if (StairPosArr[0].transform.position.y > StairPosArr[1].transform.position.y)
        {
            StairPosArr[0].m_IsUpPos = true;
            StairPosArr[1].m_IsUpPos = false;
            m_UpPos = StairPosArr[0];
            m_DownPos = StairPosArr[1];
        }
        else
        {
            StairPosArr[1].m_IsUpPos = true;
            StairPosArr[0].m_IsUpPos = false;
            m_UpPos = StairPosArr[1];
            m_DownPos = StairPosArr[0];
        }
        
        m_UpPos.m_ParentStair = this;
        m_DownPos.m_ParentStair = this;

        if (p_InitSpriteOrderToObject)
        {
            MoveOrder(10);
        }
        else
        {
            MoveOrder(16);
        }
    }

    private void Start()
    {
        for (int i = 0; i < StairList.Count; i++)
            StairList[i].StairNum = i + 1;
    }

    public void MoveOrder(int _order)
    {
        string name = "";

        switch (_order)
        {
            case 10:
                name = "Object";
                break;
            
            case 16:
                name = "Stair";
                break;
        }
        
        for (int i = 0; i < p_RendererArr.Length; i++)
        {
            p_RendererArr[i].sortingLayerName = name;
        }
    }
}