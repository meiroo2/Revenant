using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialDoorObject : TutorialObject
{
	public Transform p_SpawnPosition;
	private bool m_CanEnter = false;

	public override void Initialize()
	{
		action += DoorOpen;
	}

	public void DoorOpen()
	{
		m_CanEnter = true;
		NextAnimation();
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.tag == "@Player")
		{
			if (Input.GetKey(KeyCode.F) && m_CanEnter)
			{
				collision.transform.position = p_SpawnPosition.position;
			}
		}
	}
}
