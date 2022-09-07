using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public static class StaticMethods
{
    private static Vector2 m_TempVec2;
    private static Vector3 m_TempVec3;
    private static PixelPerfectCamera m_PPC = Camera.main.GetComponent<PixelPerfectCamera>();

    
    public static Vector2 GetRotatedVec(Vector2 _inputVec, float _angle)
    {
        float deg2rad = _angle * Mathf.Deg2Rad;
        float sinElement = Mathf.Sin(deg2rad);
        float cosElement = Mathf.Cos(deg2rad);

        return new Vector2(
            _inputVec.x * cosElement - _inputVec.y * sinElement,
            _inputVec.x * sinElement + _inputVec.y * cosElement
        );
    }
    public static bool IsRoomEqual(LocationInfo _first, LocationInfo _second)
    {
        if (_first is null || _second is null)
            return false;
        
        if (_first.p_curLayer == _second.p_curLayer &&
            _first.p_curRoom == _second.p_curRoom &&
            _first.p_curFloor == _second.p_curFloor)
            return true;
        else
        {
            return false;
        }
    }
    public static Vector2 getPixelPerfectPos(Vector2 _rawpos)
    {
        m_TempVec2 = _rawpos;
        m_TempVec2.x = Mathf.RoundToInt(m_TempVec2.x * m_PPC.assetsPPU);
        m_TempVec2.y = Mathf.RoundToInt(m_TempVec2.y * m_PPC.assetsPPU);

        m_TempVec2.x /= m_PPC.assetsPPU;
        m_TempVec2.y /= m_PPC.assetsPPU;

        return m_TempVec2;
    }
    public static Vector3 getPixelPerfectPos(Vector3 _rawpos)
    {
        /*
        m_TempVec3 = _rawpos;
        m_TempVec3.x = Mathf.RoundToInt(m_TempVec3.x * m_PPC.assetsPPU);
        m_TempVec3.y = Mathf.RoundToInt(m_TempVec3.y * m_PPC.assetsPPU);

        m_TempVec3.x /= m_PPC.assetsPPU;
        m_TempVec3.y /= m_PPC.assetsPPU;
        */
        return _rawpos;
    }

    /// <summary>
    /// 수직인 벡터를 반환합니다.
    /// </summary>
    /// <param name="_rawVec"> 넣을 벡터 </param>
    /// <returns> 넣은 벡터에 수직인 벡터 </returns>
    public static Vector2 getLPerpVec(Vector2 _rawVec)
    {
        m_TempVec2.x = -_rawVec.y;
        m_TempVec2.y = _rawVec.x;

        return m_TempVec2;
    }

    public static int getAnglePhase(Vector2 _origin, Vector2 _target, int _unitAngle, int _limitAngle = 0)
    {
        int curAnglePhase = 0;
        Vector2 distance = new Vector2(_target.x - _origin.x, _target.y - _origin.y);

        float curActualAngle;

        if (_origin.x < _target.x)
            curActualAngle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        else
            curActualAngle = -(Mathf.Atan2(-distance.y, -distance.x) * Mathf.Rad2Deg);

        if (_limitAngle > 0)
        {
            if (curActualAngle > 90 - _limitAngle)
                curActualAngle = 90 - _limitAngle;
            else if (curActualAngle < -90 + _limitAngle)
                curActualAngle = -90 + _limitAngle;
        }

        float anglePhase = (180f - _limitAngle * 2) / _unitAngle;

        for (int i = 0; i < _unitAngle; i++)
        {
            if (curActualAngle <= (90 - _limitAngle) - anglePhase * i &&
                curActualAngle > (90 - _limitAngle) - anglePhase * (i + 1))
            {
                curAnglePhase = i;
                break;
            }
        }

        return curAnglePhase;
    }

}