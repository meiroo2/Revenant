using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialHatchObject : TutorialObject
{
	private Collider2D[] m_Colliders;
	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		m_Colliders = GetComponentsInChildren<Collider2D>();
		foreach (var collider in m_Colliders)
		{
			collider.enabled = false;
		}

	}

	public override void Initialize()
	{
		action += NextAnimation;
	}

	public void ActiveCollider()
	{
		foreach (var collider in m_Colliders)
		{
			collider.enabled = true;
		}

		transform.GetChild(0).gameObject.SetActive(true);
		m_animator.StopPlayback();
		m_animator.enabled = false;
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if(Input.GetKey(KeyCode.F) && collision.tag == "@Player" && m_animator.enabled == false)
		{
			m_animator.enabled = true;
			transform.GetChild(0).gameObject.SetActive(false);
			NextAnimation();
			collision.gameObject.SetActive(false);
		}
	}

	public void LoadNextScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}
