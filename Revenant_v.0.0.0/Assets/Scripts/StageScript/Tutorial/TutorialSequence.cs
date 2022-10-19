using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSequence : MonoBehaviour
{
	[SerializeField] private int m_TutorialCount = 0;
	[field: SerializeField] public List<TutorialLevel> p_TutorialLevel { get; set; } = new List<TutorialLevel>();

	private void Start()
	{

		if (p_TutorialLevel.Count > 0)
		{
			p_TutorialLevel[m_TutorialCount].Initialize();

			foreach (var tutorialObject in p_TutorialLevel[m_TutorialCount].tutorialObjects)
			{
				tutorialObject.Initialize();
			}
		}
	}

	void FixedUpdate()
	{
		if (m_TutorialCount > p_TutorialLevel.Count - 1)
			return;

		if (p_TutorialLevel[m_TutorialCount].CheckCondition() == true || Input.GetKeyDown(KeyCode.P))
		{
			foreach (var tutorialObject in p_TutorialLevel[m_TutorialCount].tutorialObjects)
			{
				tutorialObject.action?.Invoke();
			}

			m_TutorialCount++;
			if (m_TutorialCount < p_TutorialLevel.Count)
			{
				p_TutorialLevel[m_TutorialCount].Initialize();
				foreach (var tutorialObject in p_TutorialLevel[m_TutorialCount].tutorialObjects)
				{
					tutorialObject.Initialize();
				}
			}
		}
	}
}
