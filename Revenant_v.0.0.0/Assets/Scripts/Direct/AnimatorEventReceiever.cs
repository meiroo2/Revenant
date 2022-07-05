using System;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorEventExtension
{
    /// <summary>
    /// 특정 클립 시작 콜백 등록
    /// </summary>
    /// <param name="targetClipName">클립 이름</param>
    /// <param name="callback">콜백 등록</param>
    public static void OnClipStart(this Animator animator, string targetClipName, Action callback)
    {
        AnimatorEventReceiever receiever = animator.gameObject.GetComponent<AnimatorEventReceiever>();
        if (receiever == null) receiever = animator.gameObject.AddComponent<AnimatorEventReceiever>();
        receiever.AddStartEvent(targetClipName, callback);
    }

    /// <summary>
    /// 특정 클립 종료 콜백 등록
    /// </summary>
    /// <param name="targetClipName">클립 이름</param>
    /// <param name="callback">콜백 등록</param>
    public static void OnClipEnd(this Animator animator, string targetClipName, Action callback)
    {
        AnimatorEventReceiever receiever = animator.gameObject.GetComponent<AnimatorEventReceiever>();
        if (receiever == null) receiever = animator.gameObject.AddComponent<AnimatorEventReceiever>();
        receiever.AddEndEvent(targetClipName, callback);
    }
}

[RequireComponent(typeof(Animator))]
public class AnimatorEventReceiever : MonoBehaviour
{
    public List<AnimationClip> animationClips = new List<AnimationClip>();

    private Dictionary<string, List<Action>> _startEvnets = new Dictionary<string, List<Action>>();
    private Dictionary<string, List<Action>> _endEvents = new Dictionary<string, List<Action>>();

    private void Awake()
    {
        // 애니메이터 내에 있는 모든 애니메이션 클립의 시작과 끝에 이벤트를 생성한다.
        var animator = GetComponent<Animator>();
        for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
        {
            AnimationClip clip = animator.runtimeAnimatorController.animationClips[i];
            animationClips.Add(clip);

            AnimationEvent animationStartEvent = new AnimationEvent();
            animationStartEvent.time = 0;
            animationStartEvent.functionName = "AnimationStartHandler";
            animationStartEvent.stringParameter = clip.name;
            clip.AddEvent(animationStartEvent);

            AnimationEvent animationEndEvent = new AnimationEvent();
            animationEndEvent.time = clip.length;
            animationEndEvent.functionName = "AnimationEndHandler";
            animationEndEvent.stringParameter = clip.name;
            clip.AddEvent(animationEndEvent);
        }
    }

    public void AddStartEvent(string clipName, Action callback)
    {
        if (!_startEvnets.TryGetValue(clipName, out var actions))
        {
            actions = new List<Action>();
            _startEvnets.Add(clipName, actions);
        }
        actions.Add(callback);
    }

    public void AddEndEvent(string clipName, Action callback)
    {
        if (!_endEvents.TryGetValue(clipName, out var actions))
        {
            actions = new List<Action>();
            _endEvents.Add(clipName, actions);
        }
        actions.Add(callback);
    }

    private void AnimationStartHandler(string name)
    {
        if (_startEvnets.TryGetValue(name, out var actions))
        {
            for (int i = 0; i < actions.Count; i++)
            {
                actions[i]?.Invoke();
            }
            actions.Clear();
        }
    }
    private void AnimationEndHandler(string name)
    {
        if (_endEvents.TryGetValue(name, out var actions))
        {
            for (int i = 0; i < actions.Count; i++)
            {
                actions[i]?.Invoke();
            }
            actions.Clear();
        }
    }
}