using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButtonObject : TutorialObject
{
	public override void Initialize()
	{
		action += NextAnimation;
	}

	private void OnTriggerStay2D(Collider2D collision)
    {
		if (collision.tag == "@Player")
		{
			if (Input.GetKey(KeyCode.F))
			{
				action?.Invoke();
			}
		}
	}
}
