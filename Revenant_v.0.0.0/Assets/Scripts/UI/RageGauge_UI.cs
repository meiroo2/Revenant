using System;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageGauge_UI : MonoBehaviour
{
	// Visible Member Variables
	// UI 변수 및 함수 분류하기
	[BoxGroup("게이지 비주얼")] public Image p_BackImg;
	[BoxGroup("게이지 비주얼")] public Image p_GaugeImg;
	[BoxGroup("게이지 비주얼")] public Image p_BulletTimeIndicator; // 경우에 따라 옮기기
	[BoxGroup("게이지 비주얼")] public Text p_BulletTimeTxt;

	public Color NatureRefillColor;
	public Color NatureConsumeColor;

	private Vector2 m_InitPos;
	private Vector2 m_InitBackImgScale;
	private Vector2 m_InitGaugeImgScale;
	private Color m_InitGaugeColor;

	private RectTransform m_RectTransform;
	private DynamicUIMgr m_DynamicUIMgr;

	public Action OnNotAbleBulletTime;	// BulletTime 진입이 불가능할 때의 이벤트 
	public Action OnBulletTimeStart;	// BulletTime 시작시 이벤트
	public Action OnBulletTimeEnd;		// BulletTime 종료시 이벤트

	private void Awake()
	{
		p_BulletTimeTxt.color = new Color(1f, 1f, 1f, 0f);
		m_InitGaugeColor = p_GaugeImg.color;
		m_RectTransform = GetComponent<RectTransform>();

		if (ReferenceEquals(p_GaugeImg, null))
		{
			Debug.LogError("RageGauge_UI에서 이미지 할당되지 않음.");
		}

		//m_DynamicUIMgr.Shake(m_RectTransform, m_InitPos, 30f, 1500f, 10f, 30f);
		m_InitBackImgScale = p_BackImg.rectTransform.localScale;
		m_InitGaugeImgScale = p_GaugeImg.rectTransform.localScale;
		p_BulletTimeIndicator.enabled = false;
	}

	// Start is called before the first frame update
	void Start()
	{
		m_InitPos = m_RectTransform.anchoredPosition;
		m_DynamicUIMgr = GameMgr.GetInstance().GetComponent<DynamicUIMgr>();

		// UI 이벤트 추가
		OnNotAbleBulletTime += ShakeTransform;
		OnBulletTimeStart += PlayAnimationOnBulletTimeStart;
		OnBulletTimeStart += delegate { p_BulletTimeIndicator.enabled = false;  };
		OnBulletTimeEnd   += PlayAnimationOnBulletTimeEnd;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void ShakeTransform()
	{
		m_DynamicUIMgr.Shake(m_RectTransform, m_InitPos, 30f, 1500f, 10f, 30f);
	}

	/// <summary>
	/// RageGauge를 일시적으로 Timer로 사용하기 위해 일반화된 시간값을 받습니다. (즉시 적용)
	/// </summary>
	/// <param name="_normalTime">Normalized 된 시간값</param>
	public void GetTimePassed(float _normalTime)
	{
		p_BulletTimeTxt.text = (Math.Truncate(_normalTime * 10) / 10).ToString();
		p_GaugeImg.fillAmount = _normalTime;
	}

	/// <summary>
	/// BulletTime이 시작했을 때 실행되는 애니메이션입니다.
	/// </summary>
	private void PlayAnimationOnBulletTimeStart()
	{
		RectTransform backForm = p_BackImg.rectTransform;
		m_DynamicUIMgr.ExpandUI(backForm, m_InitBackImgScale,
		new Vector2(backForm.localScale.x + 0.07f, backForm.localScale.y), 5f);

		RectTransform gaugeForm = p_GaugeImg.rectTransform;
		m_DynamicUIMgr.ExpandUI(gaugeForm, m_InitGaugeImgScale,
		new Vector2(gaugeForm.localScale.x + 0.05f, gaugeForm.localScale.y), 5f);

		m_DynamicUIMgr.ChangeColor(p_GaugeImg, m_InitGaugeColor, Color.white, 3f);
		m_DynamicUIMgr.FadeUI(p_BulletTimeTxt, true, 50f);
	}

	/// <summary>
	/// BulletTime이 끝났을 때 실행되는 애니메이션입니다.
	/// </summary>
	private void PlayAnimationOnBulletTimeEnd()
	{
		RectTransform backForm = p_BackImg.rectTransform;
		m_DynamicUIMgr.ExpandUI(backForm, backForm.localScale,
		m_InitBackImgScale, 5f);

		RectTransform gaugeForm = p_GaugeImg.rectTransform;
		m_DynamicUIMgr.ExpandUI(gaugeForm, gaugeForm.localScale,
		m_InitGaugeImgScale, 5f);

		m_DynamicUIMgr.ChangeColor(p_GaugeImg, Color.white, m_InitGaugeColor, 3f);
		m_DynamicUIMgr.FadeUI(p_BulletTimeTxt, false, 50f);
	}
}
