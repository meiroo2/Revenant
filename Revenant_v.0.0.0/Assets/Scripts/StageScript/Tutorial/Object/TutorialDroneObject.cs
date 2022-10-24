using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDroneObject : TutorialObject
{

    public List<AnimationClip> p_TutorialVideos = new();
    private Dictionary<string, AnimationClip> m_TutorialVideosMap = new();
	[field: SerializeField] public Animator P_VideoAnimator;
    SpriteRenderer m_VideoSpriteRenderer;    

    protected override void Start()
    {
        base.Start();   
        foreach(var video in p_TutorialVideos)
        {
            m_TutorialVideosMap.Add(video.name, video);
        }
        m_VideoSpriteRenderer = P_VideoAnimator.gameObject.GetComponent<SpriteRenderer>();
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

            StartCoroutine(PlayVideo(videoName));
		}
    }

    private IEnumerator PlayVideo(string videoName)
    {
        float alpha = 0;
        var Mat = m_VideoSpriteRenderer.material;
        Debug.Log(Mat.ToString());
		while (!m_VideoSpriteRenderer.enabled)
		{
			yield return null;
		}
		if (!P_VideoAnimator.GetCurrentAnimatorStateInfo(0).IsName("None"))
        {
 

			while (true)
            {

				yield return new WaitForSeconds(0.02f);
				alpha = Mat.GetFloat("_MainTexAlpha");
                if(alpha > 0)
                {
                    alpha -= 0.05f;
                }
                Mat.SetFloat("_MainTexAlpha", alpha);
				Debug.Log(Mat.GetFloat("_MainTexAlpha"));
				if (alpha <= 0)
                    break;
			}

        }

		P_VideoAnimator.Play(videoName);
		Mat.SetFloat("_MainTexAlpha", 0);
		yield return null;


		while (true)
		{
			yield return new WaitForSeconds(0.02f);
			alpha = Mat.GetFloat("_MainTexAlpha");
			if (alpha < 1)
			{
				alpha += 0.05f;
			}
			Mat.SetFloat("_MainTexAlpha", alpha);
			Debug.Log(Mat.GetFloat("_MainTexAlpha"));
			if (alpha >= 1)
				break;
		}


		yield return null;  
    }
}
