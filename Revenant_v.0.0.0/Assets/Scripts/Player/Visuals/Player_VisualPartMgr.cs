using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_VisualPartMgr : MonoBehaviour
{
    // 이 스크립트는 각 파트(머리, 몸 등)의 파트장 역할을 합니다.
    // 아래의 IPlayerVisualPart는 각 파트의 하위 오브젝트들입니다.
    // 예) 머리1, 머리2, 머리3....
    public IPlayerVisualPart[] m_EachPart;

    public int m_curVisiblePartIdx { get; private set; } = 0;

    private void Awake()
    {
        m_EachPart = GetComponentsInChildren<IPlayerVisualPart>();
        m_curVisiblePartIdx = 0;
    }

    public virtual void m_setPartVisible(int _idx, bool _isVisible)
    {
        if (_idx >= 0 && _idx < m_EachPart.Length)
        {
            for (int i = 0; i < m_EachPart.Length; i++)
                m_EachPart[i].SetVisible(!_isVisible);

            m_EachPart[_idx].SetVisible(_isVisible);
            m_curVisiblePartIdx = _idx;
        }
        else
            Debug.Log("PartVisualMgr_Idx_Out-of-Range");
    }
    public virtual void m_setPartVisible(bool _isVisible)
    {
        m_EachPart[m_curVisiblePartIdx].SetVisible(_isVisible);
    }
    public virtual void m_setFullVisible(bool _isVisible)
    {
        for (int i = 0; i < m_EachPart.Length; i++)
            m_EachPart[i].SetVisible(_isVisible);
    }
    public virtual void m_setPartAniVisible(int _idx, bool _isVisible)
    {
        if (_idx >= 0 && _idx < m_EachPart.Length)
        {
            for (int i = 0; i < m_EachPart.Length; i++)
                m_EachPart[i].SetAniVisible(!_isVisible);

            m_EachPart[_idx].SetAniVisible(_isVisible);
            m_curVisiblePartIdx = _idx;
        }
        else
            Debug.Log("PartVisualMgr_Idx_Out-of-Range");
    }
    public virtual void m_setPartAniVisible(bool _isVisible)
    {
        m_EachPart[m_curVisiblePartIdx].SetAniVisible(_isVisible);
    }
    public virtual void m_setPartAni(string _Paramname, int _value)
    {
        m_EachPart[m_curVisiblePartIdx].SetAnim(_Paramname, _value);
    }
    public virtual void m_setPartSprite(int _idx)
    {
        m_EachPart[m_curVisiblePartIdx].SetSprite(_idx);
    }
}