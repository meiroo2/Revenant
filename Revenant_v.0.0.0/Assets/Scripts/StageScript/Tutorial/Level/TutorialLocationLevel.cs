using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ư�� ������ �������� �� ���� �ܰ�� �Ѿ�� �̺�Ʈ �����Դϴ�.
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
