using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialDoorObject : TutorialObject
{
	public Transform p_SpawnPosition;
	private bool m_CanEnter = false;
	private CameraMgr m_camMgr;
	public override void Initialize()
	{
		m_camMgr = FindObjectOfType<CameraMgr>();
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
				m_camMgr.m_CamBoundMgr = null;
				collision.transform.position = p_SpawnPosition.position;
				m_camMgr.MoveToPosition(p_SpawnPosition.position);
			}
		}
	}
}
