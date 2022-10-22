using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEffectPuller : MonoBehaviour
{
    // Visible Member Variables
    public SimpleEffect[] p_PullingSimpleEffectArr;
    
    
    // Member Variables
    private int m_SimpleEffectLimit = 10;
    private List<SimpleEffect> m_SimpleEffectList = new List<SimpleEffect>();
    private int[] m_IdxArr;


    // Constructors
    private void Awake()
    {
        InitCheck();

        for (int i = 0; i < p_PullingSimpleEffectArr.Length; i++)
        {
            for (int j = 0; j < m_SimpleEffectLimit; j++)
            {
                var go = Instantiate(p_PullingSimpleEffectArr[i], transform).GetComponent<SimpleEffect>();
                m_SimpleEffectList.Add(go);
                go.m_ParentTransform = transform;
                go.gameObject.SetActive(false);
            }
        }

        m_IdxArr = new int[p_PullingSimpleEffectArr.Length];
        for (int i = 0; i < m_IdxArr.Length; i++)
        {
            m_IdxArr[i] = i * m_SimpleEffectLimit;
        }
    }
    
    // Functions

    /// <summary>
    /// 이펙트를 원하는 트랜스폼에 붙여서 출력합니다.(나중에 자동 회수)
    /// </summary>
    /// <param name="_effectNum">출력할 이펙트 번호</param>
    /// <param name="_toAttach">부착 목표 트랜스폼</param>
    /// <param name="_ignoreTimescale">TimeScale 무시</param>
    /// <param name="_pos">LocalPosition 조정</param>
    /// <returns>SimpleEffect 트랜스폼</returns>
    public Transform SpawnSimpleEffect(
        int _effectNum, Transform _toAttach, bool _ignoreTimescale = false, Vector2 _pos = default(Vector2))
    {
        if (_effectNum < 0 || _effectNum >= p_PullingSimpleEffectArr.Length)
        {
            Debug.Log("ERR : SpawnSimpleEffect OOR");
            return null;
        }

        var SIEffect = m_SimpleEffectList[m_IdxArr[_effectNum]];
        SIEffect.gameObject.SetActive(true);
        SIEffect.m_Animator.updateMode = _ignoreTimescale ? AnimatorUpdateMode.UnscaledTime : AnimatorUpdateMode.Normal;
        SIEffect.gameObject.transform.parent = _toAttach;
        SIEffect.gameObject.transform.localPosition = _pos;


        m_IdxArr[_effectNum] += 1;
        
        if (m_IdxArr[_effectNum] >= (_effectNum + 1) * m_SimpleEffectLimit)
        {
            m_IdxArr[_effectNum] = _effectNum * m_SimpleEffectLimit;
        }

        return m_SimpleEffectList[m_IdxArr[_effectNum]].gameObject.transform;
    }

    /// <summary>
    /// 이펙트를 원하는 위치에 출력시킵니다. (나중에 자동 회수)
    /// </summary>
    /// <param name="_effectNum">출력할 이펙트 번호</param>
    /// <param name="_spawnPos">출력될 위치</param>
    /// <param name="_ignoreTimescale">Timescale 무시</param>
    /// <returns></returns>
    public Transform SpawnSimpleEffect(int _effectNum, Vector2 _spawnPos, bool _isFlip = false, bool _ignoreTimescale = false)
    {
        if (_effectNum < 0 || _effectNum >= p_PullingSimpleEffectArr.Length)
        {
            Debug.Log("ERR : SpawnSimpleEffect OOR");
            return null;
        }

        var SIEffect = m_SimpleEffectList[m_IdxArr[_effectNum]];
        SIEffect.gameObject.SetActive(true);
        SIEffect.m_Animator.updateMode = _ignoreTimescale ? AnimatorUpdateMode.UnscaledTime : AnimatorUpdateMode.Normal;
        SIEffect.transform.position = _spawnPos;

        Vector3 localScale = SIEffect.transform.localScale;
        localScale.x = _isFlip ? MathF.Abs(localScale.x) * -1f : MathF.Abs(localScale.x);
        SIEffect.transform.localScale = localScale;


        m_IdxArr[_effectNum] += 1;
        
        if (m_IdxArr[_effectNum] >= (_effectNum + 1) * m_SimpleEffectLimit)
        {
            m_IdxArr[_effectNum] = _effectNum * m_SimpleEffectLimit;
        }

        return m_SimpleEffectList[m_IdxArr[_effectNum]].gameObject.transform;
    }
    
    
    private void InitCheck()
    {
        if (p_PullingSimpleEffectArr.Length <= 0)
        {
            Debug.Log("ERR : SimpleEffectPuller - Arr 비어있음");
        }
    }
}
