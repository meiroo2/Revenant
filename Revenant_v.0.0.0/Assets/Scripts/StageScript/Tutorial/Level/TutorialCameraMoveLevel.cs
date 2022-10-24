using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCameraMoveLevel : TutorialLevel
{
    private CameraMgr m_camMgr;
    private Player m_Player;
    public Transform p_CameraMoveTarget;


    public float p_EndTerm = 1;
    // Start is called before the first frame update
    void Start()
    {
		m_camMgr = FindObjectOfType<CameraMgr>();
		m_Player = FindObjectOfType<Player>();
	}

    public override void Initialize()
    {
        m_camMgr.m_IsFollowTarget = true;
		m_camMgr.StopAllCoroutines();
		m_camMgr.MoveToTarget(p_CameraMoveTarget, 1f);
		m_Player.m_InputMgr.SetAllInputLock(true);
	}

    // Update is called once per frame
    void Update()
    {

    }

	public override bool CheckCondition()
	{


		if (!m_camMgr.m_IsFollowTarget)
		{
            if(tutorialObjects.Count > 0)
			{
				foreach (var obj in tutorialObjects)
				{
					obj.action?.Invoke();
				}
			}
		}

        if(m_camMgr.m_IsMoveEnd)
        {
			m_Player.m_InputMgr.SetAllInputLock(false);
		}

		return m_camMgr.m_IsMoveEnd;
	}
}
