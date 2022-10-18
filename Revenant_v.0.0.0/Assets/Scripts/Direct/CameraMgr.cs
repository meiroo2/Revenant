using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class CameraMgr : MonoBehaviour
{
    // Visible Member Variables
    public CamBoundMgr p_CamBoundMgr;
    public bool p_InitFollow = true;

    public float p_YValue = 0f;
    public float p_ShakePower = 1f;
    public float p_RotatePower = 0;
    public float p_ShakeRecoverSpeed = 5f;
    public float p_RotateRecoverSpeed = 1f;
    public float p_BodyHitRatio = 0.5f;
    public float p_CamShakeZoomPower = 0f;


    // Member Variables
    private Vector3 m_CameraPos;
    private Vector3 m_CameraTempPos;
    private Vector2 m_MousePos;
    
    private Vector2 m_PlusVec;
    
    private GameObject m_Player;
    private bool m_CamStuckOnce = false;
    private bool m_FollowMouse = false;
    
    private float m_CamShakeDirectionAngle = 0;
    private float m_CamShakeRotationAngle = 0;

    private float m_OriginCamZoomValue;
    private float m_CamShakeZoomValue = 0f;

    private Camera m_MainCam;

    public bool m_IsFollowTarget { get; set; } = false;
    public bool m_IsMoveEnd { get; set; } = true;

    // Constructors
    private void Awake()
    {
        m_MainCam = Camera.main;
        m_OriginCamZoomValue = m_MainCam.orthographicSize;
        Cursor.visible = true;
    }
    
    private void Start()
    {
        m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer().gameObject;

        m_CameraPos = transform.position;

        if (p_InitFollow)
            m_CameraPos = m_Player.transform.position;

        transform.position = m_CameraPos;
    }

    public void MoveToTarget(Transform targetTransform, float MoveStopDuration)
    {
        StartCoroutine(MoveToTargetCoroutine(targetTransform, MoveStopDuration));
    }

    private IEnumerator MoveToTargetCoroutine(Transform targetTransform, float MoveStopDuration)
    {
        // MoveStart
        m_IsFollowTarget = true;
        m_IsMoveEnd = false;


		float MoveStopTimer = 0;
        Debug.Log("start");
        while(MoveStopTimer < MoveStopDuration)
        {
			m_CameraPos = Vector3.Lerp(m_CameraPos, targetTransform.position, Time.deltaTime * 4f);
			m_CameraPos.z = -10f;
			transform.position = p_CamBoundMgr.getNearCamPos(m_CameraPos);
			if (Vector2.Distance(m_CameraPos, targetTransform.position) < 0.05f) // 카메라가 대상을 바라보고 있음
            {
				m_IsFollowTarget = false;
				MoveStopTimer += Time.deltaTime;
			}
			Debug.Log("move");
			yield return null;
        }

		// 돌아감
		while (true)
        {
			m_CameraPos = Vector3.Lerp(m_CameraPos, m_Player.transform.position, Time.deltaTime * 4f);
			m_CameraPos.z = -10f;
			transform.position = p_CamBoundMgr.getNearCamPos(m_CameraPos);
			if (Vector2.Distance(m_CameraPos, m_Player.transform.position) < 0.05f) // 카메라가 대상을 바라보고 있음
			{
                break;
			}
			yield return null;
		}

        // MoveEnd
        m_IsMoveEnd = true;

		yield return null;
	}



    // Updates
    private void Update()
    {
        if (m_IsMoveEnd == false)
        {
			return;
        }

        if (m_CamStuckOnce)
        {
            m_CamStuckOnce = false;
            m_CameraPos = m_CameraTempPos;
        }

        m_CameraPos = Vector3.Lerp(m_CameraPos, m_Player.transform.position, Time.deltaTime * 4f);
        
        if (p_CamBoundMgr.canCamMove(m_CameraPos))
        {
            m_CameraPos.z = -10f;
            transform.position = m_CameraPos;
        }
        else
        {
            m_CameraPos.z = -10f;
            transform.position = p_CamBoundMgr.getNearCamPos(m_CameraPos);
        }

        transform.position = new Vector3(transform.position.x + m_PlusVec.x, transform.position.y + m_PlusVec.y + p_YValue, -10f);

        m_PlusVec = Vector3.Lerp(m_PlusVec, Vector3.zero, Time.deltaTime * p_ShakeRecoverSpeed);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime *
            p_RotateRecoverSpeed);

        m_CamShakeZoomValue = Mathf.Lerp(m_CamShakeZoomValue, 0f, Time.deltaTime * 5f);
        
        m_MainCam.orthographicSize = m_OriginCamZoomValue + m_CamShakeZoomValue;
    }
    

    // Physics


    // Functions
    public void DoCamShake(bool _isHead)
    {
        m_CamShakeDirectionAngle = Random.Range(0, 360);
        m_PlusVec = Vector2.up;
        m_PlusVec = Quaternion.Euler(0, 0, m_CamShakeDirectionAngle) * m_PlusVec;

        m_PlusVec *= p_ShakePower;

        int tempRandomRotateAngle = Random.Range(0, 2);
        m_CamShakeRotationAngle = tempRandomRotateAngle == 0 ? -1 : 1;
        m_CamShakeRotationAngle *= p_RotatePower;
        
        if (!_isHead)
        {
            m_PlusVec *= p_BodyHitRatio;
            m_CamShakeRotationAngle *= p_BodyHitRatio;
        }
        else
        {
            // Head Hit
            m_CamShakeZoomValue = p_CamShakeZoomPower;
        }

        transform.rotation = Quaternion.Euler(0,0,m_CamShakeRotationAngle);
    }
    private void PreciseMode(int _input)
    {
        m_FollowMouse = _input switch
        {
            0 => false,
            1 => true,
            _ => m_FollowMouse
        };
    }
    public void InstantMoveToPlayer(Vector2 _playerOriginPos, Vector2 _PlayerMovePos)
    {
        float Relativex = _playerOriginPos.x - transform.position.x;
        float Relativey = _playerOriginPos.y - transform.position.y;

        m_CameraPos = _PlayerMovePos;
        m_CameraPos.z = -10f;

        m_CameraPos.x -= Relativex;
        m_CameraPos.y -= Relativey;

        m_CameraTempPos = m_CameraPos;
        transform.position = m_CameraPos;
    }
}
