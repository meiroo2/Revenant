using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEndLevel : TutorialLevel
{

	public override bool CheckCondition()
	{
		return true;
	}


	public override void Initialize()
	{
		base.Initialize();
		p_TutorialDrone.m_animator.Play("Drone_Off");
		p_TutorialDrone.PlayTutorialVideo("NoneVideo");
	}
}
