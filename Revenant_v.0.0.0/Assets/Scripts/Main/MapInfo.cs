using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MapInfo
{
    int m_curStage = 0;
    int m_curMap = 0;
    int m_curLayerroom = 0;
    int m_curFloor = 0;
    int m_curRoom = 0;
    public MapInfo(int _stage, int _map, int _layerroom, int _floor, int _room)
    {
        m_curStage = _stage;
        m_curMap = _map;
        m_curLayerroom = _layerroom;
        m_curFloor = _floor;
        m_curRoom = _room;
    }
}
