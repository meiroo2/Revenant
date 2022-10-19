using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

public static class StaticMethods
{
    private static Vector2 m_TempVec2;
    private static Vector3 m_TempVec3;
    private static PixelPerfectCamera m_PPC = Camera.main.GetComponent<PixelPerfectCamera>();


    /// <summary>
    /// 비활성화된 오브젝트를 포함 모든 오브젝트를 리턴합니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<T> FindAllObjects<T>() where T : UnityEngine.Object
    {
        List<T> objects = new List<T>();
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            if (scene.isLoaded)
            {
                var rootObject = scene.GetRootGameObjects();
                for (int j = 0; j < rootObject.Length; j++)
                {
                    var go = rootObject[j];
                    objects.AddRange(go.GetComponentsInChildren<T>(true));
                }
            }
        }
        return objects;
    }
    
    /// <summary>
    /// 받은 List에서 가장 _centerPos에 가까운 IHotBox를 리턴합니다.
    /// </summary>
    /// <param name="_centerPos">기준 좌표</param>
    /// <param name="_hotBoxList">HotBox 리스트</param>
    /// <returns>가장 가까운 IHotBox</returns>
    public static IHotBox GetNearestHotBox(Vector2 _centerPos, List<IHotBox> _hotBoxList)
    {
        if (_hotBoxList.Count == 1)
            return _hotBoxList[0];
        
        int nearIdx = 0;
        float nearDistance = (_centerPos - (Vector2)_hotBoxList[0].m_ParentObj.transform.position).sqrMagnitude;

        for (int i = 1; i < _hotBoxList.Count; i++)
        {
            float distance = (_centerPos - (Vector2)_hotBoxList[i].m_ParentObj.transform.position).sqrMagnitude;
            if (distance < nearDistance)
            {
                nearIdx = i;
                nearDistance = distance;
            }
        }

        return _hotBoxList[nearIdx];
    }
    
    
    /// <summary>
    /// 받은 Array에서 가장 _centerPos에 가까운 IHotBox를 리턴합니다.
    /// </summary>
    /// <param name="_centerPos">기준 좌표</param>
    /// <param name="_hotBoxList">HotBox 리스트</param>
    /// <returns>가장 가까운 IHotBox</returns>
    public static IHotBox GetNearestHotBox(Vector2 _centerPos, IHotBox[] _hotBoxList)
    {
        if (_hotBoxList.Length == 1)
            return _hotBoxList[0];
        
        int nearIdx = 0;
        float nearDistance = (_centerPos - (Vector2)_hotBoxList[0].m_ParentObj.transform.position).sqrMagnitude;

        for (int i = 1; i < _hotBoxList.Length; i++)
        {
            float distance = (_centerPos - (Vector2)_hotBoxList[i].m_ParentObj.transform.position).sqrMagnitude;
            if (distance < nearDistance)
            {
                nearIdx = i;
                nearDistance = distance;
            }
        }

        return _hotBoxList[nearIdx];
    }
    
    
    /// <summary>
    /// NormalTime을 받아 어떠한 벡터값을 지정한 위치로 갔다가 오도록 Lerp값을 리턴합니다.
    /// </summary>
    /// <param name="_originPos">원본 위치</param>
    /// <param name="_destPos">지정 위치</param>
    /// <param name="_deadLine">NormalTime 기준 어느 지점에서 돌아올까?</param>
    /// <param name="_normalTime">Normalized Time</param>
    /// <returns></returns>
    public static Vector2 GetLerpPosByNormalizedTime(Vector2 _originPos, Vector2 _destPos, float _deadLine, float _normalTime)
    {
        Debug.Log(_originPos + ", " + _destPos);
        
        return _normalTime <= _deadLine ? 
            Vector2.Lerp(_originPos, _destPos, (_normalTime / _deadLine)) :
            Vector2.Lerp(_destPos, _originPos, -(_normalTime - _deadLine) / (_deadLine - 1f));
    }
    
    /// <summary>
    /// int형 확률을 대입해 당첨 여부를 알려줍니다.
    /// </summary>
    /// <param name="_probability">몇 퍼센트?</param>
    /// <returns>true면 당첨</returns>
    public static bool GetProbabilityWinning(int _probability)
    {
        int randomValue = Random.Range(1, 101);
        return randomValue <= _probability ? true : false;
    }
    
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
    /// 왼쪽을 향하고 직교하는 벡터를 반환합니다.
    /// </summary>
    /// <param name="_rawVec"> 인자 벡터 </param>
    /// <returns> 직교하는 벡터 </returns>
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