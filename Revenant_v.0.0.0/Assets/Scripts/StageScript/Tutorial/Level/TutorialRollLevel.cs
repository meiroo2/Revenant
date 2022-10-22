using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRollLevel : TutorialLevel
{
	public TutorialBot p_tutorialBot;
	public TutorialLocationObject p_Location;
	public bool isArrived = false;

	private void Start()
	{
		p_tutorialBot.gameObject.SetActive(false);
	}
	public void SetIsArrived()
	{
		isArrived = true;
		p_Location.action -= SetIsArrived;
	}

	public override bool CheckCondition()
	{
		return isArrived;
	}

	public override void Initialize()
	{
		base.Initialize();
		p_Location.action += SetIsArrived;
		p_tutorialBot.gameObject.SetActive(true);
	}
}
