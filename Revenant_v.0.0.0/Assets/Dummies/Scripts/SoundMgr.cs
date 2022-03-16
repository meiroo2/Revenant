using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoundMgr : MonoBehaviour
{
    // Visible Member Variables


    // Member Variables
    private FMOD.Studio.EventInstance m_eventInstance;
    public float m_level = 0f;
    public bool m_isEnd = true;
    public TextMeshProUGUI m_tmp;

    // Constructors
    private void Awake()
    {

    }
    private void Start()
    {
        m_eventInstance = FMODUnity.RuntimeManager.CreateInstance("event:/BGM/Target_Practice");
    }
    /*
    <커스텀 초기화 함수가 필요할 경우>
    public void Init()
    {

    }
    */

    // Updates
    private void Update()
    {
        if (m_isEnd == false)
            m_tmp.text = "Score : " + (m_level * 100f).ToString();

        if (!m_isEnd)
        {
            m_eventInstance.setParameterByName("Target_Success", m_level);

            if (m_level > 0f)
            {
                m_level -= Time.deltaTime * 0.1f;
            }
            else
                m_level = 0f;
        }
    }
    private void FixedUpdate()
    {

    }

    // Physics


    // Functions
    public void playGunFireSound(int _num, GameObject _position)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Weapons/Rifle/Rifle_Shot", _position);
    }
    public void playGunFireSound(int _num, Vector2 _position)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Weapons/Rifle/Rifle_Shot", _position);
    }
    public void playAttackedSound(MatType _matType, GameObject _position)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Weapons/Bullet_Hit/Normal", _position);
    }
    public void playAttackedSound(MatType _matType, Vector2 _position)
    {
        switch (_matType)
        {
            case MatType.Normal:
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Weapons/Bullet_Hit/Normal", _position);
                break;

            case MatType.Target_Head:
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Weapons/Bullet_Hit/Target_Head", _position);
                break;

            case MatType.Target_Body:
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Weapons/Bullet_Hit/Target_Body", _position);
                break;
        }
    }


    // 기타 분류하고 싶은 것이 있을 경우
    public void endGame()
    {
        m_eventInstance.setParameterByName("isEnd", 1);
        m_isEnd = true;
    }
    public void startGame()
    {
        m_level = 0f;
        m_eventInstance.start();
        m_isEnd = false;
        m_eventInstance.setParameterByName("isEnd", 0);
    }
}
