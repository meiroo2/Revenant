using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ArmMgr : MonoBehaviour
{
    // Visible Member Variables
    public float HighLimitAngle_Long = 60f;
    public float LowLimitAngle_Long = -60f;
    public float HighLimitAngle_Short = 10f;
    public float LowLimitAngle_Short = -10f;
    [Space(5f)]
    public float RecoilDistance = 0.05f;
    public float RecoilSpeed = 20f;

    [Space(20f)]
    [Header("For Publics")]
    public Transform p_GunTransform;
    public Transform p_LongGunPos;
    public Transform p_ShortGunPos;

    [Space(5f)]
    [Header("For IKs")]
    public Transform p_LongOutArmEffectorPos;
    public Transform p_LongInArmEffectorPos;
    public Transform p_ShortOutArmEffectorPos;
    public Transform p_ShortInArmEffectorPos;
    public Transform p_HeadEffectorPos;

    // Member Variables
    private Player_AniMgr m_PlayerAniMgr;
    private WeaponMgr m_WeaponMgr;
    private PlayerRotation m_PlayerRotation;
    private Player m_Player;

    private bool m_isRecoilLerping = false;
    private float m_RecoilTimer = 1f;

    private Vector2[] m_LongArmEffectorOriginPos = new Vector2[2];  // 긴팔의 원본 이펙터포지션 (바깥, 안)
    private Vector2[] m_ShortArmEffectorOriginPos = new Vector2[2]; // 짧은팔의 원본 이펙터 포지션 (바깥, 안)

    private Vector2 m_HeadEffectorOriginPos;



    // Constructors
    private void Awake()
    {
        m_PlayerRotation = GetComponent<PlayerRotation>();

        m_LongArmEffectorOriginPos[0] = p_LongOutArmEffectorPos.localPosition;
        m_LongArmEffectorOriginPos[1] = p_LongInArmEffectorPos.localPosition;

        m_ShortArmEffectorOriginPos[0] = p_ShortOutArmEffectorPos.localPosition;
        m_ShortArmEffectorOriginPos[1] = p_ShortInArmEffectorPos.localPosition;
    }
    private void Start()
    {
        InstanceMgr _instance = InstanceMgr.GetInstance();
        m_WeaponMgr = _instance.GetComponentInChildren<Player_Manager>().m_Player.m_WeaponMgr;
        m_PlayerAniMgr = _instance.GetComponentInChildren<Player_Manager>().m_Player.m_PlayerAniMgr;
        m_Player = _instance.GetComponentInChildren<Player_Manager>().m_Player;

        m_HeadEffectorOriginPos = p_HeadEffectorPos.localPosition;
    }

    // Updates
    private void Update()
    {
        if (m_Player.m_isRightHeaded)
            p_HeadEffectorPos.localPosition = new Vector2(m_HeadEffectorOriginPos.x - (m_PlayerRotation.m_curAnglewithLimit / 400f), m_HeadEffectorOriginPos.y);
        else
            p_HeadEffectorPos.localPosition = new Vector2(m_HeadEffectorOriginPos.x - (m_PlayerRotation.m_curAnglewithLimit / 400f), m_HeadEffectorOriginPos.y);

        if (Input.GetMouseButtonDown(0))
        {
            switch (m_WeaponMgr.m_CurWeapon.Fire())
            {
                
                case 0:
                    // 발사 실패
                    break;

                case 1:
                    // 발사 성공
                    m_RecoilTimer = 1f;
                    doRecoil();
                    m_isRecoilLerping = true;
                    break;

                case 2:
                    // 총알 없음
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            m_WeaponMgr.m_CurWeapon.Reload();
        }

        if (m_isRecoilLerping)
        {
            doRecoilLerp();
        }
    }

    // Physics


    // Functions
    public void changeArmPartPos(int _ArmIdx)
    {

        switch (_ArmIdx)
        {
            case 0:
                p_LongOutArmEffectorPos.localPosition = m_LongArmEffectorOriginPos[0];
                p_LongInArmEffectorPos.localPosition = m_LongArmEffectorOriginPos[1];
                p_GunTransform.position = p_LongGunPos.position;
                m_PlayerRotation.changeAngleLimit(HighLimitAngle_Long, LowLimitAngle_Long);
                break;

            case 1:
                p_ShortOutArmEffectorPos.localPosition = m_ShortArmEffectorOriginPos[0];
                p_ShortInArmEffectorPos.localPosition = m_ShortArmEffectorOriginPos[1];
                p_GunTransform.position = p_ShortGunPos.position;
                m_PlayerRotation.changeAngleLimit(HighLimitAngle_Short, LowLimitAngle_Short);
                break;
        }
    }
    private void doRecoil()
    {
        switch (m_PlayerAniMgr.m_curArmIdx)
        {
            case 0:
                p_GunTransform.localPosition = new Vector2(p_LongGunPos.localPosition.x - RecoilDistance, p_LongGunPos.localPosition.y);
                p_LongOutArmEffectorPos.localPosition = new Vector2(m_LongArmEffectorOriginPos[0].x - RecoilDistance, m_LongArmEffectorOriginPos[0].y);
                p_LongInArmEffectorPos.localPosition = new Vector2(m_LongArmEffectorOriginPos[1].x - RecoilDistance, m_LongArmEffectorOriginPos[1].y);
                break;

            case 1:
                p_GunTransform.localPosition = new Vector2(p_ShortGunPos.localPosition.x - RecoilDistance, p_ShortGunPos.localPosition.y);
                p_ShortOutArmEffectorPos.localPosition = new Vector2(m_ShortArmEffectorOriginPos[0].x - RecoilDistance, m_ShortArmEffectorOriginPos[0].y);
                p_ShortInArmEffectorPos.localPosition = new Vector2(m_ShortArmEffectorOriginPos[1].x - RecoilDistance, m_ShortArmEffectorOriginPos[1].y);
                break;
        }
    }
    private void doRecoilLerp()
    {
        switch (m_PlayerAniMgr.m_curArmIdx)
        {
            case 0:
                p_GunTransform.localPosition = Vector2.Lerp(p_GunTransform.localPosition, p_LongGunPos.localPosition, Time.deltaTime * RecoilSpeed);
                p_LongOutArmEffectorPos.localPosition = Vector2.Lerp(p_LongOutArmEffectorPos.localPosition, m_LongArmEffectorOriginPos[0], Time.deltaTime * RecoilSpeed);
                p_LongInArmEffectorPos.localPosition = Vector2.Lerp(p_LongInArmEffectorPos.localPosition, m_LongArmEffectorOriginPos[1], Time.deltaTime * RecoilSpeed);
                break;

            case 1:
                p_GunTransform.localPosition = Vector2.Lerp(p_GunTransform.localPosition, p_ShortGunPos.localPosition, Time.deltaTime * RecoilSpeed);
                p_ShortOutArmEffectorPos.localPosition = Vector2.Lerp(p_ShortOutArmEffectorPos.localPosition, m_ShortArmEffectorOriginPos[0], Time.deltaTime * RecoilSpeed);
                p_ShortInArmEffectorPos.localPosition = Vector2.Lerp(p_ShortInArmEffectorPos.localPosition, m_ShortArmEffectorOriginPos[1], Time.deltaTime * RecoilSpeed);
                break;
        }
        m_RecoilTimer -= Time.deltaTime;
        if(m_RecoilTimer <= 0f)
        {
            m_RecoilTimer = 1f;
            m_isRecoilLerping = false;
        }
    }

    // 기타 분류하고 싶은 것이 있을 경우
}