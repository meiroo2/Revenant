using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class TutorialHideLevel : TutorialLevel
{
	public bool isHidden = false;
	private Animator playerAniamtor;

	private void Start()
	{
		playerAniamtor = GameObject.Find("FullBody_VPart").GetComponent<Animator>();
	}



	public override bool CheckCondition()
	{
		if (!isHidden)
			isHidden = playerAniamtor.GetInteger("Hide") == 1;

		return isHidden;
	}


	public override void Initialize()
	{
		base.Initialize();
	}
}
