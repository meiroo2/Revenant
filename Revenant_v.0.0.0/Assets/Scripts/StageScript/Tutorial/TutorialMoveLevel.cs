using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMoveLevel : TutorialLevel
{
	private bool m_isPushA = false;
	private bool m_isPushD = false;
	private float m_KeyAPushTimer = 1f;
	private float m_KeyDPushTimer = 1f;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.A))
			m_isPushA = true;
		else if (Input.GetKeyUp(KeyCode.A))
			m_isPushA = false;

		if (Input.GetKeyDown(KeyCode.D))
			m_isPushD = true;
		else if (Input.GetKeyUp(KeyCode.D))
			m_isPushD = false;
	}

	private void FixedUpdate()
	{

	}

	public override bool CheckCondition()
	{
		if (m_isPushA)
		{
			m_KeyAPushTimer -= Time.deltaTime;
		}
		else if (m_KeyAPushTimer > 0)
		{
			m_KeyAPushTimer = 1f;
		}

		if (m_isPushD)
		{
			m_KeyDPushTimer -= Time.deltaTime;
		}
		else if (m_KeyDPushTimer > 0)
		{
			m_KeyDPushTimer = 1f;
		}


		if (m_KeyAPushTimer <= 0 && m_KeyDPushTimer <= 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
