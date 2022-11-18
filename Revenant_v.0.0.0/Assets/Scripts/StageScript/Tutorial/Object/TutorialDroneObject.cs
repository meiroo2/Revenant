using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialDroneObject : TutorialObject
{
    public GameObject player;
	public float p_OffsetY;

    protected override void Start()
    {
        player = FindObjectOfType<Player>().gameObject;
	}

    void Update()
    {
		FollowCharacter();
	}

    /// <summary>
    /// 플레이어를 따라갑니다.
    /// </summary>
    public void FollowCharacter()
    {
		float distance = transform.position.x - player.transform.position.x;
		Vector3 targetPosition = Vector3.zero;
		if (Vector3.Distance(transform.position, player.transform.position) > 1)
		{
			float scaleX;
			if (distance < 0)
			{
				scaleX = -1;
				transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
				targetPosition.x = player.transform.position.x + 1;
			}
			else if (distance > 0)
			{
				scaleX = 1;
				transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
				targetPosition.x = player.transform.position.x - 1;
			}


			targetPosition.y = player.transform.position.y + p_OffsetY;
			targetPosition.z = player.transform.position.z;


			transform.position = Vector3.MoveTowards(transform.position, targetPosition, 1 * Time.deltaTime);
		}
	}
}
