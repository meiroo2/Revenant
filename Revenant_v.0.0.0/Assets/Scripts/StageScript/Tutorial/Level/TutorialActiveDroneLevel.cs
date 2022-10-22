using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActiveDroneLevel : TutorialLevel
{

    public override bool CheckCondition()
    {
        if (p_TutorialDrone.m_animator.GetCurrentAnimatorStateInfo(0).IsName("Drone_Idle"))
            return true;
        else
        {
			return false;
        }
    }

    public override void Initialize()
    {
        p_TutorialDrone.NextAnimation();
    }
}
