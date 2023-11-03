using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;


public class SectionManager : MonoBehaviour
{
    public SectionInfo[] SectionInfoArr;
    private SectionSensor m_PlayerSensor;

    private void Start()
    {
        m_PlayerSensor = GameMgr.GetInstance().p_PlayerMgr.GetPlayer().m_PlayerSectionSensor;
    }

    public bool IsSameSectionWithPlayer(SectionSensor _enemySensor)
    {
        return _enemySensor._curContactSectionInfo == m_PlayerSensor._curContactSectionInfo;
    } 
    
    public List<List<SectionDoor>> GetPathtoPlayer(SectionSensor _startSensor, SectionSensor _endSensor)
    {
        List<List<SectionDoor>> pathLists = new();
        List<SectionDoor> curPathList = new();

        for (int i = 0; i < _startSensor._curContactSectionInfo.SectionDoorArr.Length; i++)
        {
            {
                List<SectionDoor> newPath = new List<SectionDoor>();
                newPath.Add(_startSensor._curContactSectionInfo.SectionDoorArr[i]);
                recursiveCall(pathLists, newPath, _endSensor, true);
                newPath.Clear();
            }
        }
        
        for (int i = 0; i < pathLists.Count; i++)
        {
            Debug.Log(i+"번째 경로 결과 도출 시작");
            for (int j = 0; j < pathLists[i].Count; j++)
            {
                Debug.Log(pathLists[i][j]);
            }
        }

        return pathLists;
    }

    public List<SectionDoor> GetTargetPath(List<List<SectionDoor>> _sectionDoors, TraceType _traceType)
    {
        int idx = 0;
        float shortestLength = 999999999f;

        for (int i = 0; i < _sectionDoors.Count; i++)
        {
            float length = 0f;
            if (_sectionDoors[i].Count == 1)
            {
                idx = i;
                shortestLength = 0f;
                break;
            }
            else
            {
                for (int j = 0; j < _sectionDoors[i].Count - 1; j++)
                {
                    length += Vector2.SqrMagnitude(_sectionDoors[i][j].transform.position -
                                                           _sectionDoors[i][j + 1].transform.position);
                    if (length < shortestLength)
                    {
                        idx = i;
                        shortestLength = length;
                    }
                }
            }
        }

        if (_traceType == TraceType.BackDoor)
        {
            var lists = _sectionDoors.ToList();
            lists.Remove(_sectionDoors[idx]);
            return lists[0];
        }
        
        return _sectionDoors[idx];
    }

    // 재귀 검사
    private void recursiveCall(List<List<SectionDoor>> _finalPathLists, 
        List<SectionDoor> _curPathList,
        SectionSensor _targetSensor, bool _firstSearch = false)
    {
        if (_firstSearch)
        {
            // 담은 문들 중 어떤 문의 반대쪽 방이 플레이어가 있는 방인지?
            if (_curPathList[0].OtherSideDoor.WhichSectionBelong == _targetSensor._curContactSectionInfo)
            {
                _finalPathLists.Add(_curPathList.ToList());
                return;
            }
            else
            {
                // 리스트로 받은 문 경로 중 가장 마지막 문의 반대쪽을 참조, 본인 뺴고 싹 담음
                List<SectionDoor> sectionDoorList = new();
                sectionDoorList.AddRange(_curPathList[_curPathList.Count - 1].OtherSideDoor.WhichSectionBelong.SectionDoorArr);
                sectionDoorList.Remove(_curPathList[_curPathList.Count - 1].OtherSideDoor);
        
        
                foreach (var element in sectionDoorList)
                {
                    // 담은 문들 중 어떤 문의 반대쪽 방이 플레이어가 있는 방인지?
                    if (element.OtherSideDoor.WhichSectionBelong == _targetSensor._curContactSectionInfo)
                    {
                        _curPathList.Add(element);
                        _finalPathLists.Add(_curPathList.ToList());
                        _curPathList.Remove(element);
                        continue;
                    }
                    else
                    {
                        // 플레이어가 없는 방이네요

                        // 해당 방에 문이 1개 이상 존재
                        if (element.OtherSideDoor.WhichSectionBelong.SectionDoorArr.Length > 0)
                        {
                            // 일단 추가를 조짐
                            _curPathList.Add(element);
                            recursiveCall(_finalPathLists, _curPathList, _targetSensor);
                        }
                        else
                        {
                            // 막다른 길 데스
                            continue;
                        }
                    }
                }
            }
        }
        else
        {
            // 리스트로 받은 문 경로 중 가장 마지막 문의 반대쪽을 참조, 본인 뺴고 싹 담음
            List<SectionDoor> sectionDoorList = new();
            sectionDoorList.AddRange(_curPathList[_curPathList.Count - 1].OtherSideDoor.WhichSectionBelong.SectionDoorArr);
            sectionDoorList.Remove(_curPathList[_curPathList.Count - 1].OtherSideDoor);
        
        
            foreach (var element in sectionDoorList)
            {
                // 담은 문들 중 어떤 문의 반대쪽 방이 플레이어가 있는 방인지?
                if (element.OtherSideDoor.WhichSectionBelong == _targetSensor._curContactSectionInfo)
                {
                    _curPathList.Add(element);
                    _finalPathLists.Add(_curPathList.ToList());
                    _curPathList.Remove(element);
                    continue;
                }
                else
                {
                    // 플레이어가 없는 방이네요

                    // 해당 방에 문이 1개 이상 존재
                    if (element.OtherSideDoor.WhichSectionBelong.SectionDoorArr.Length > 0)
                    {
                        // 일단 추가를 조짐
                        _curPathList.Add(element);
                        recursiveCall(_finalPathLists, _curPathList, _targetSensor);
                    }
                    else
                    {
                        // 막다른 길 데스
                        continue;
                    }
                }
            }
        }
    }
}