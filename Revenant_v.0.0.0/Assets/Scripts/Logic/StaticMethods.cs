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

}