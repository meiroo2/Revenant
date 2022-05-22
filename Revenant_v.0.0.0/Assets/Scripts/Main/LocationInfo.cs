using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationInfo : ScriptableObject
{
    [field:SerializeField] public int m_curLayer { get; private set; }
    [field: SerializeField] public int m_curRoom { get; private set; }
    [field: SerializeField] public int m_curFloor { get; private set; }
    [field: SerializeField] public Vector2 m_curPos { get; set; }

    public LocationInfo()
    {
        m_curLayer = 0;
        m_curRoom = 0;
        m_curFloor = 0;
        m_curPos = Vector2.zero;
    }
    public LocationInfo(int _Layer, int _Room, int _Stair, Vector2 _Pos)
    {
        m_curLayer = _Layer;
        m_curRoom = _Room;
        m_curFloor = _Stair;
        m_curPos = _Pos;
    }

    public void setLocation(LocationInfo _location)
    {
        m_curLayer = _location.m_curLayer;
        m_curRoom = _location.m_curRoom;
        m_curFloor = _location.m_curFloor;
        m_curPos = _location.m_curPos;
    }
}