using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActiveDroneLevel : TutorialLevel
{

    public override bool CheckCondition()
    {
        if (p_TutorialDrone.m_animator.GetCurrentAnimatorStateInfo(0).IsName("Drone_Idle") || p_TutorialDrone.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            return true;
        else
        {
			return false;
        }
    }

    public override void Initialize()
    {
        p_TutorialDrone.m_animator.SetInteger("TutorialAnimationIndex", 1);
		p_TutorialDrone.m_animator.Play("Drone_On");
        p_TutorialDrone.PlayTutorialVideo("NoneVideo");
	}
}
