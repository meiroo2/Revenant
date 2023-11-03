using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;




public class SectionDoor : MonoBehaviour
{
    public SectionDoor OtherSideDoor;
    public SectionDoor_Col DoorCol;

    [HideInInspector]
    public SectionInfo WhichSectionBelong;
    
    private const float _centerXPosCheat = 0.4f;
    private const float _gap2Tile = 0.64f;
    
    public void TeleportTransform(Transform _transform)
    {
        _transform.position = OtherSideDoor.GetOutPos();
    }

    /// <summary>
    /// 현재 문의 출구 좌표를 알려줍니다.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetOutPos()
    {
        Vector2 returnPos = transform.position;
        
        returnPos.x += _centerXPosCheat;
        returnPos.y += _gap2Tile;
        
        return returnPos;
    }
}