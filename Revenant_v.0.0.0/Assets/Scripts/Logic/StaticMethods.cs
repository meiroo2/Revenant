using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public static class StaticMethods
{
    private static Vector2 m_TempVec2;
    private static Vector3 m_TempVec3;
    private static PixelPerfectCamera m_PPC = Camera.main.GetComponent<PixelPerfectCamera>();

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
        m_TempVec3 = _rawpos;
        m_TempVec3.x = Mathf.RoundToInt(m_TempVec3.x * m_PPC.assetsPPU);
        m_TempVec3.y = Mathf.RoundToInt(m_TempVec3.y * m_PPC.assetsPPU);

        m_TempVec3.x /= m_PPC.assetsPPU;
        m_TempVec3.y /= m_PPC.assetsPPU;

        return m_TempVec3;
    }

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