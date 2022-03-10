using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr_Background : MonoBehaviour
{
    // Visible Member Variables
    public FMODUnity.EventReference[] m_AMBs;
    public FMODUnity.EventReference[] m_BGMs;

    // Member Variables
    private FMOD.Studio.EventInstance m_AMBInstance;
    private FMOD.Studio.EventInstance m_BGMInstance;

    // Constructors
    private void Awake()
    {
        m_AMBInstance = FMODUnity.RuntimeManager.CreateInstance(m_AMBs[0]);
        m_BGMInstance = FMODUnity.RuntimeManager.CreateInstance(m_BGMs[0]);
    }
    private void Start()
    {
        m_AMBInstance.start();
    }

    // Updates
    private void Update()
    {

    }
    private void FixedUpdate()
    {

    }

    // Physics


    // Functions


    // 기타 분류하고 싶은 것이 있을 경우
}
