using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPressButtonLevel : TutorialLevel
{
	public TutorialObject p_Button;
	public bool isPressed = false;
	private void Start()
	{
		FindObjectOfType<Player>();
	}

	public void SetIsPressed()
	{
		isPressed = true;
		p_Button.action -= SetIsPressed;
	}

	public override bool CheckCondition()
	{
        return isPressed;
	}


	public override void Initialize()
	{
		p_Button.action += SetIsPressed;
	}
}
