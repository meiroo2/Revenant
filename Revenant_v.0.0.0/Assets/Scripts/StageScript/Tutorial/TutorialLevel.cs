using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevel : MonoBehaviour
{
	public TutorialDroneObject p_TutorialDrone;
	public string p_TutorialVideoName;
	public List<TutorialObject> tutorialObjects;

    public virtual void Initialize() 
	{
		PlayDroneVideo(p_TutorialVideoName);
	}

    public virtual bool CheckCondition()
    {
		// 예외처리 추가하기
		return false;
    }

	public virtual void PlayDroneVideo(string videoName)
	{
		if (p_TutorialDrone != null && p_TutorialVideoName != "")
		p_TutorialDrone.PlayTutorialVideo(p_TutorialVideoName);
	}
}
