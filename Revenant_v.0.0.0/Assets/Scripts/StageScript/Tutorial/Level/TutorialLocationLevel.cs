using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 특정 지역에 도착했을 때 다음 단계로 넘어가는 이벤트 레벨입니다.
public class TutorialLocationLevel : TutorialLevel
{
	public TutorialLocationObject p_Location;
	public bool isArrived = false;

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
	}
}
