using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class TutorialEnemyKillLevel : TutorialLevel
{
    public BasicEnemy p_Enemy;
	public bool isEnemyDead = false;

	private void Start()
	{
		p_Enemy.gameObject.SetActive(false);
	}

	public override bool CheckCondition()
	{
		if (p_Enemy.m_CurEnemyStateName == EnemyStateName.DEAD)
			isEnemyDead = true;

		return isEnemyDead;
	}


	public override void Initialize()
	{
		p_Enemy.gameObject.SetActive(true);
		base.Initialize();
	}
}
