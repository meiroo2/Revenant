using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtil : MonoBehaviour
{
    /// <summary>
    /// 게임의 최소 단위인 0.01에 맞춰 반올림을 진행하고 리턴합니다.
    /// </summary>
    /// <param name="_inputVal">계산이 필요한 자료형</param>
    /// <returns></returns>
    public float GetPreciseVal(float _inputVal)
    {
        return Mathf.Round(_inputVal * 100f) * 0.01f;
    }
    
    /// <summary>
    /// 게임의 최소 단위인 0.01에 맞춰 반올림을 진행하고 리턴합니다.
    /// </summary>
    /// <param name="_inputVal">계산이 필요한 자료형</param>
    /// <returns></returns>
    public Vector2 GetPreciseVal(Vector2 _inputVal)
    {
        return new Vector2(GetPreciseVal(_inputVal.x), GetPreciseVal(_inputVal.y));
    }


    /// <summary>
    /// 게임에서 사용하는 1타일의 크기를 원하는 만큼 리턴합니다.
    /// </summary>
    /// <param name="_tileNum">원하는 타일 개수</param>
    /// <returns></returns>
    public float GetTileValue(int _tileNum)
    {
        return GetPreciseVal(0.32f * _tileNum);
    }
}
