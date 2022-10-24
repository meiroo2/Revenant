using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCameraMoveLevel : TutorialLevel
{
    private CameraMgr m_camMgr;
    public Transform p_CameraMoveTarget;

    public float p_EndTerm = 1;
    // Start is called before the first frame update
    void Start()
    {
		m_camMgr = FindObjectOfType<CameraMgr>();
    }

    public override void Initialize()
    {
        m_camMgr.m_IsFollowTarget = true;
		m_camMgr.StopAllCoroutines();
		m_camMgr.MoveToTarget(p_CameraMoveTarget, 1f);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public override bool CheckCondition()
	{
        return !m_camMgr.m_IsMoveEnd;
	}
}
