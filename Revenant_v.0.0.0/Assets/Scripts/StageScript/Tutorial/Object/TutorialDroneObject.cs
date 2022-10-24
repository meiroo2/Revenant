using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDroneObject : TutorialObject
{

    public List<AnimationClip> p_TutorialVideos = new();
    private Dictionary<string, AnimationClip> m_TutorialVideosMap = new();
	[field: SerializeField] public Animator P_VideoAnimator;

    protected override void Start()
    {
        base.Start();   
        foreach(var video in p_TutorialVideos)
        {
            m_TutorialVideosMap.Add(video.name, video);
        }
	}

    void Update()
    {
        if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Drone_contactIdle"))
        {
            P_VideoAnimator.GetComponent<SpriteRenderer>().enabled = false;
		}
        else
        {
			P_VideoAnimator.GetComponent<SpriteRenderer>().enabled = true;
		}
    }

    public void PlayTutorialVideo(string videoName)
    {
        m_TutorialVideosMap.TryGetValue(videoName, out AnimationClip clip);

        if (clip == null)
        {
            Debug.Log("해당 이름을 가진 클립이 없습니다");
        }
        else
        {
			if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Drone_Idle"))
			{
                m_animator.SetInteger("TutorialAnimationIndex", 3);
				m_animator.Play("Drone_contact");
                Debug.Log("실행");
			}

			P_VideoAnimator.Play(videoName);
		}
    }
}
