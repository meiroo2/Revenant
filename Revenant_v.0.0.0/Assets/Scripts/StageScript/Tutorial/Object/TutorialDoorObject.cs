using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialDoorObject : TutorialObject
{
	public Transform p_SpawnPosition;
	public CamBoundMgr p_CamBoundMgr;
	public Image FadeOutImage;
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
				m_camMgr.StopAllCoroutines();
				m_camMgr.m_IsFollowTarget = false;
				m_camMgr.m_IsMoveEnd = true;
				m_camMgr.MoveToPosition(p_SpawnPosition.position, p_CamBoundMgr);
				StartCoroutine(FadeOut());
			}
		}
	}

	private IEnumerator FadeOut()
	{
		Color32 FadeOutColor = FadeOutImage.color;
		FadeOutColor.a = 255;
		FadeOutImage.color = FadeOutColor;
		yield return null;

		while(FadeOutColor.a > 0)
		{
			FadeOutColor.a -= 5;
			FadeOutImage.color = FadeOutColor;
			yield return new WaitForSeconds(0.02f);
		}

		yield return null;
		FadeOutColor.a = 0;
		FadeOutImage.color = FadeOutColor;
	}
}
