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
	public bool p_InitFollow = true;

	public float p_YValue = 0f;
	public float p_ShakePower = 1f;
	public float p_RotatePower = 0;
	public float p_ShakeRecoverSpeed = 5f;
	public float p_RotateRecoverSpeed = 1f;
	public float p_BodyHitRatio = 0.5f;
	public float p_CamShakeZoomPower = 0f;


	// Member Variables
	private Vector3 m_CaledCamPos;
	private Vector3 m_BeforeStuckPos;
	private Vector3 m_LerpedCamPos;
	private Vector3 m_FinalCamPos;

	private Vector2 m_MousePos;

	private Vector2 m_CamShakeVector;

	private float m_DeltaTime = 0f;

	private GameObject m_Player;
	private bool m_CamStuckOnce = false;

	private bool m_CamCanMove = true;

	private float m_CamShakeDirectionAngle = 0;
	private float m_CamShakeRotationAngle = 0;

	private float m_OriginCamZoomValue;
	private float m_CamShakeZoomValue = 0f;

	private Camera m_MainCam;
	public CamBoundMgr m_CamBoundMgr;
	private float m_LastGapBetPlayer = 0f;

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
		m_DeltaTime = 0f;
		m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer().gameObject;

		m_LerpedCamPos = transform.position;

		if (p_InitFollow)
			m_LerpedCamPos = m_Player.transform.position;

		transform.position = m_LerpedCamPos;
	}

	public void MoveToTarget(Transform targetTransform, float MoveStopDuration)
    {
        StartCoroutine(MoveToTargetCoroutine(targetTransform.position, MoveStopDuration));
    }

    private IEnumerator MoveToTargetCoroutine(Vector2 targetPosition, float MoveStopDuration)
    {
        // MoveStart
        m_IsFollowTarget = true;
        m_IsMoveEnd = false;


		float MoveStopTimer = 0;
        while(MoveStopTimer < MoveStopDuration)
        {
			m_LerpedCamPos = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 4f);
			m_LerpedCamPos.z = -10f;
			transform.position = m_LerpedCamPos;
			if (Vector2.Distance(transform.position, targetPosition) < 0.1f) // 카메라가 대상을 바라보고 있음
            {
				m_IsFollowTarget = false;
				MoveStopTimer += Time.deltaTime;
			}
			yield return null;
        }

		// 돌아감
		while (true)
        {
			m_LerpedCamPos = Vector3.Lerp(transform.position, m_Player.transform.position, Time.deltaTime * 4f);
			m_LerpedCamPos.z = -10f;
			transform.position = m_LerpedCamPos;
			if (Vector2.Distance(transform.position, m_Player.transform.position) < 0.1f) // 카메라가 대상을 바라보고 있음
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
		if (!m_IsMoveEnd)
			return;
		m_DeltaTime = Time.deltaTime;

		m_LerpedCamPos = Vector3.Lerp(m_LerpedCamPos, m_Player.transform.position, m_DeltaTime * 4f);

		CalCaledPos();
		CalCamRotate();
		CalCamZoom();
		CalCamShake();

		m_FinalCamPos = new Vector3(m_CaledCamPos.x + m_CamShakeVector.x, m_CaledCamPos.y + m_CamShakeVector.y + p_YValue, -10f);
		transform.position = m_FinalCamPos;
	}


	// Physics
	// Functions
	public void ChangeCamBoundMgr(CamBoundMgr _camBound)
	{
		if (m_CamBoundMgr == _camBound)
			return;

		Vector2 playerPos = m_Player.transform.position;
		m_LastGapBetPlayer = transform.position.y - playerPos.y;

		m_CamStuckOnce = false;
		m_CamBoundMgr = _camBound;

		m_LerpedCamPos = new Vector3(playerPos.x,
			playerPos.y + m_LastGapBetPlayer, -10f);

		transform.position = m_LerpedCamPos;
	}

	private void CalCaledPos()
	{
		if (!ReferenceEquals(m_CamBoundMgr, null))
		{
			m_CamCanMove = m_CamBoundMgr.canCamMove(m_LerpedCamPos);

			switch (m_CamCanMove)
			{
				case false when !m_CamStuckOnce:
					m_CaledCamPos = m_CamBoundMgr.getNearCamPos(m_LerpedCamPos);
					m_BeforeStuckPos = m_CaledCamPos;
					m_CamStuckOnce = true;
					break;

				case false when m_CamStuckOnce:
					m_CaledCamPos = m_BeforeStuckPos;
					break;

				case true when m_CamStuckOnce:
					m_CamStuckOnce = false;
					break;

				case true when !m_CamStuckOnce:
					m_CaledCamPos = m_LerpedCamPos;
					break;
			}
		}
		else
		{
			m_CaledCamPos = m_LerpedCamPos;
		}
	}

	private void CalCamRotate()
	{
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, m_DeltaTime *
			p_RotateRecoverSpeed);
	}

	private void CalCamZoom()
	{
		m_CamShakeZoomValue = Mathf.Lerp(m_CamShakeZoomValue, 0f, m_DeltaTime * 5f);
		m_MainCam.orthographicSize = m_OriginCamZoomValue + m_CamShakeZoomValue;
	}

	private void CalCamShake()
	{
		m_CamShakeVector = Vector3.Lerp(m_CamShakeVector, Vector3.zero, m_DeltaTime * p_ShakeRecoverSpeed);
	}


	public void DoCamShake(bool _isHead)
	{
		m_CamShakeDirectionAngle = Random.Range(0, 360);
		m_CamShakeVector = Vector2.up;
		m_CamShakeVector = Quaternion.Euler(0, 0, m_CamShakeDirectionAngle) * m_CamShakeVector;

		m_CamShakeVector *= p_ShakePower;

		int tempRandomRotateAngle = Random.Range(0, 2);
		m_CamShakeRotationAngle = tempRandomRotateAngle == 0 ? -1 : 1;
		m_CamShakeRotationAngle *= p_RotatePower;

		if (!_isHead)
		{
			m_CamShakeVector *= p_BodyHitRatio;
			m_CamShakeRotationAngle *= p_BodyHitRatio;
		}
		else
		{
			// Head Hit
			m_CamShakeZoomValue = p_CamShakeZoomPower;
		}

		transform.rotation = Quaternion.Euler(0, 0, m_CamShakeRotationAngle);
	}

	public void InstantMoveToPlayer(Vector2 _playerOriginPos, Vector2 _PlayerMovePos)
	{
		float Relativex = _playerOriginPos.x - transform.position.x;
		float Relativey = _playerOriginPos.y - transform.position.y;

		m_LerpedCamPos = _PlayerMovePos;
		m_LerpedCamPos.z = -10f;

		m_LerpedCamPos.x -= Relativex;
		m_LerpedCamPos.y -= Relativey;

		transform.position = m_LerpedCamPos;
	}

	public void MoveToPosition(Vector2 pos)
	{
		transform.position = pos;
		m_LerpedCamPos = pos;
		m_LerpedCamPos.z = -10f;
		m_FinalCamPos = m_LerpedCamPos;
	}
}
