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
            P_VideoAnimator.Play(videoName);
		}
    }
}
