using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSequence : MonoBehaviour
{
    [SerializeField] private int m_TutorialCount = 0; 
    public List<TutorialLevel> p_TutorialLevel { get; private set; } = new List<TutorialLevel>();

    void FixedUpdate()
    {
        if (p_TutorialLevel[m_TutorialCount].CheckCondition() == true)
        {
            foreach (var tutorialObject in p_TutorialLevel[m_TutorialCount].tutorialObjects)
            {
                tutorialObject.action.Invoke();
			}
			m_TutorialCount++;
		}
    }
}
