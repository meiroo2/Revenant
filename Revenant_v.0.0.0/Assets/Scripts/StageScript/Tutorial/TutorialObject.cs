using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObject : MonoBehaviour
{
	public Action action;
	protected Animator m_animator;

	protected virtual void Start()
	{
		if(TryGetComponent(out Animator animator))
		{
			if(animator)
			{
				m_animator = animator;
			}
		}
	}

	/// <summary>
	/// ������Ʈ�� ���� Ʃ�丮�� ������ �������� �� ����
	/// </summary>
	public virtual void Initialize() { }

	public void NextAnimation()
	{
		m_animator.SetInteger("TutorialAnimationIndex", m_animator.GetInteger("TutorialAnimationIndex") + 1);
	}
}
