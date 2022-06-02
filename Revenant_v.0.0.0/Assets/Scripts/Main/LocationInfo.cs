using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct LocationNodes
{
    [SerializeField] public GameObject[] p_RoomnInteracts;
}


[Serializable]
public class LocationInfo : MonoBehaviour
{
    // Visual Member Variables
    [field: SerializeField] public int p_curLayer { get; private set; } = 0;
    [field: SerializeField] public int p_curRoom { get; private set; } = 0;
    [field: SerializeField] public int p_curFloor { get; private set; } = 0;
    [field: SerializeField] public Vector2 p_curPos { get; set; } = Vector2.zero;
    [SerializeField] public LocationNodes[] p_ConnectedRooms;


    // Member Variables
    private LocationInfo[] m_MemberRooms;


    // Constructors
    private void Awake()
    {
        m_MemberRooms = new LocationInfo[p_ConnectedRooms.Length];
        for (int i = 0; i < m_MemberRooms.Length; i++)
        {
            m_MemberRooms[i] = p_ConnectedRooms[i].p_RoomnInteracts[0].GetComponent<LocationInfo>();
        }
    }


    // Functions
    public virtual void SetLocation(LocationInfo _location, bool _resetVector = false)
    {
        if (_location == null)
            return;

        p_curLayer = _location.p_curLayer;
        p_curRoom = _location.p_curRoom;
        p_curFloor = _location.p_curFloor;

        if (_resetVector)
            p_curPos = _location.p_curPos;
    }

    public virtual void SetLocation(int _layer, int _room, int _floor, Vector2 _pos, bool _resetVector = false)
    {
        p_curLayer = _layer;
        p_curRoom = _room;
        p_curFloor = _floor;

        if (_resetVector)
            p_curPos = _pos;
    }

    public bool CanGotoRoom(LocationInfo _destination)
    {
        for (int i = 0; i < m_MemberRooms.Length; i++)
        {
            if (StaticMethods.IsRoomEqual(m_MemberRooms[i], _destination))
                return true;
        }

        return false;
    }

    private int GetRoomIdx(LocationInfo _destination)
    {
        for (int i = 0; i < m_MemberRooms.Length; i++)
        {
            if (StaticMethods.IsRoomEqual(m_MemberRooms[i], _destination))
                return i;
        }

        return -1;
    }

    public Vector2 GetRoomDestPos(Vector2 _entity, LocationInfo _dest)
    {
        int roomIdx = GetRoomIdx(_dest);

        float minimumDistance = 999999f;
        int destIdx = -1;


        if (roomIdx != -1)
        {
            destIdx = 1;
            minimumDistance = Vector2.Distance(_entity,
                p_ConnectedRooms[roomIdx].p_RoomnInteracts[destIdx].transform.position);

            // �ش� ������ �� �� �ִ� ��ΰ� 2���� �̻�
            if (p_ConnectedRooms[roomIdx].p_RoomnInteracts.Length > 3)
            {
                for (int i = 2; i < p_ConnectedRooms[roomIdx].p_RoomnInteracts.Length; i++)
                {
                    if (minimumDistance > Vector2.Distance(_entity,
                            p_ConnectedRooms[roomIdx].p_RoomnInteracts[i].transform.position))
                    {
                        minimumDistance = Vector2.Distance(_entity,
                            p_ConnectedRooms[roomIdx].p_RoomnInteracts[i].transform.position);
                        destIdx = i;
                    }
                }
            }

            return p_ConnectedRooms[roomIdx].p_RoomnInteracts[destIdx].transform.position;
        }
        else
        {
            return Vector2.zero;
        }
    }
}