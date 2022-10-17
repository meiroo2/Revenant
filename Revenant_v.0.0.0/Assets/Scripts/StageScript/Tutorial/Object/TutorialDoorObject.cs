using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoorObject : TutorialObject
{
	public Transform p_WarpPosition;
	private bool m_CanEnter = false;

	public override void Initialize()
	{
		Debug.Log("문");
		action += DoorOpen;
	}

	public void DoorOpen()
	{
		m_CanEnter = true;
		NextAnimation();
		Debug.Log("실행");
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.tag == "@Player")
		{
			if (Input.GetKey(KeyCode.F) && m_CanEnter)
			{
				Debug.Log("워프");
				collision.gameObject.transform.position = p_WarpPosition.position;
			}
		}
	}
}
