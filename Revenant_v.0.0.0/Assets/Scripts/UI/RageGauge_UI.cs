using System;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class RageGauge_UI : MonoBehaviour
{
	// Visible Member Variables
	// UI 변수 및 함수 분류하기
	[BoxGroup("게이지 비주얼")] public SlicedFilledImage p_GaugeImg;

	[BoxGroup("게이지 비주얼")] public Image p_RollGaugeImg;
	[BoxGroup("게이지 비주얼")] public Image p_MeleeGaugeImg;
	[BoxGroup("게이지 비주얼")] public Image p_UltGaugeImg;

	[BoxGroup("게이지 비주얼")] public RectTransform p_RollParentTransform;
	[BoxGroup("게이지 비주얼")] public RectTransform p_MeleeParentTransform;
	[BoxGroup("게이지 비주얼")] public RectTransform p_UltParentTransform;

	[Space(20f)] 
	public Image p_RollEffectImg;
	public Image p_MeleeEffectImg;
	public Image p_UltEffectImg;
	
	
	[Space(10f)]
	[BoxGroup("게이지 비주얼")] public Sprite p_RollGaugeNormalSprite;
	[BoxGroup("게이지 비주얼")] public Sprite p_RollGaugeFullSprite;
	[BoxGroup("게이지 비주얼")] public Sprite p_MeleeGaugeNormalSprite;
	[BoxGroup("게이지 비주얼")] public Sprite p_MeleeGaugeFullSprite;
	[BoxGroup("게이지 비주얼")] public Sprite p_UltGaugeNormalSprite;
	[BoxGroup("게이지 비주얼")] public Sprite p_UltGaugeFullSprite;
	[BoxGroup("게이지 비주얼")] public Image p_UltOvaSprite;
	
	[Space(20f)]
	public float p_MoveSpeed = 2f;
	public float p_UIOnSpeed = 2f;
	
	// Assign

	
	// Member Variables
	private Vector2 m_InitPos;
	private Vector2 m_MovedPos;
	private Coroutine m_UIMoveCoroutine = null;
	
	private Vector2 m_RollImgOriginPos;
	private Vector2 m_MeleeImgOriginPos;
	private Vector2 m_UltImgOriginPos;

	private Coroutine m_RollShakeDelayCoroutine = null;
	private bool m_RollShakeDelayEnd = true;
	
	private Coroutine m_MeleeShakeDelayCoroutine = null;
	private bool m_MeleeShakeDelayEnd = true;
	
	private Coroutine m_UltShakeDelayCoroutine = null;
	private bool m_UltShakeDelayEnd = true;
	
	private Coroutine m_RollImgCoroutine = null;
	private Coroutine m_MeleeImgCoroutine = null;
	private Coroutine m_UltImgCoroutine = null;
	
	private Material m_OriginMat;

	private float m_Timer = 0f;
	private static readonly int ManualTimer = Shader.PropertyToID("_ManualTimer");

	private Coroutine m_UIOnCoroutine = null;
	private readonly int UiTime = Shader.PropertyToID("_UiTime");

	private void Awake()
	{
		m_OriginMat = p_GaugeImg.material;

		m_RollImgOriginPos = p_RollParentTransform.anchoredPosition;
		m_MeleeImgOriginPos = p_MeleeParentTransform.anchoredPosition;
		m_UltImgOriginPos = p_UltParentTransform.anchoredPosition;
		
		p_GaugeImg.fillAmount = 0f;
		p_RollGaugeImg.fillAmount = 0f;
		p_MeleeGaugeImg.fillAmount = 0f;
		p_UltGaugeImg.fillAmount = 0f;
	}

	private void Start()
	{
		m_InitPos = transform.localPosition;
		m_MovedPos = new Vector2(m_InitPos.x, m_InitPos.y - 200f);
	}
	
	// Updates
	

	// Functions
	public IEnumerator UIOnEnumerator(Image _img)
	{
		Material mat = Instantiate(_img.material);
		_img.material = mat;
		
		float lerpVal = 1f;
		while (true)
		{
			_img.material.SetFloat(UiTime, lerpVal);
			
			lerpVal -= Time.deltaTime * p_UIOnSpeed;

			if (lerpVal <= 0f)
			{
				lerpVal = 0f;
				_img.material.SetFloat(UiTime, lerpVal);
				
				break;
			}
			
			yield return null;
		}

		yield break;
	}
	
	public void MoveRageGaugeUI(bool _toOutside)
	{
		if (!ReferenceEquals(m_UIMoveCoroutine, null))
		{
			StopCoroutine(m_UIMoveCoroutine);
			m_UIMoveCoroutine = null;
		}

		if (_toOutside)
			m_UIMoveCoroutine = StartCoroutine(MoveSomeTransform(transform, m_InitPos, m_MovedPos));
		else
			m_UIMoveCoroutine = StartCoroutine(MoveSomeTransform(transform, m_MovedPos, m_InitPos));
	}

	private IEnumerator MoveSomeTransform(Transform _transform, Vector2 _initPos, Vector2 _destPos)
	{
		float speed = p_MoveSpeed;
		float lerpVal = 0f;
		while (true)
		{
			_transform.localPosition = Vector2.Lerp(_initPos, _destPos, lerpVal);
			
			lerpVal += (Time.unscaledDeltaTime * speed);
			speed += Time.unscaledDeltaTime * 50f;

			if (lerpVal >= 1f)
			{
				lerpVal = 1f;
				_transform.localPosition = Vector2.Lerp(_initPos, _destPos, lerpVal);
				break;
			}

			yield return null;
		}
	}
	
	public void UpdateRageGaugeUI(int _idx, float _fillAmount)
	{
		if (_fillAmount < 0f)
			_fillAmount = 0f;
		
		if (_fillAmount > 1f)
			_fillAmount = 1f;
		
		switch (_idx)
		{
			case 0:
				// Gauge
				p_GaugeImg.fillAmount = _fillAmount;
				break;
			
			case 1:
				// Roll
				if (p_RollGaugeImg.fillAmount < 1f)
				{
					// 1f 밑이였음
					
					if (_fillAmount >= 1f)
					{
						p_RollGaugeImg.sprite = p_RollGaugeFullSprite;
						StartCoroutine(ActivateEffect(p_RollEffectImg));
						StartCoroutine(UIOnEnumerator(p_RollGaugeImg));
					}

					p_RollGaugeImg.fillAmount = _fillAmount;
				}
				else
				{
					// 이미 1f였음
					
					if (_fillAmount < 1f)
					{
						p_RollGaugeImg.sprite = p_RollGaugeNormalSprite;
						p_RollGaugeImg.fillAmount = _fillAmount;
					}
				}
				break;
			
			case 2:
				// Melee
				if (p_MeleeGaugeImg.fillAmount < 1f)
				{
					// 1f 밑이였음
					
					if (_fillAmount >= 1f)
					{
						p_MeleeGaugeImg.sprite = p_MeleeGaugeFullSprite;
						StartCoroutine(ActivateEffect(p_MeleeEffectImg));
						StartCoroutine(UIOnEnumerator(p_MeleeGaugeImg));
					}

					p_MeleeGaugeImg.fillAmount = _fillAmount;
				}
				else
				{
					if (_fillAmount < 1f)
					{
						p_MeleeGaugeImg.sprite = p_MeleeGaugeNormalSprite;
						p_MeleeGaugeImg.fillAmount = _fillAmount;
					}
				}
				break;
			
			case 3:
				// Ult
				if (p_UltGaugeImg.fillAmount < 1f)
				{
					if (_fillAmount >= 1f)
					{
						p_UltGaugeImg.sprite = p_UltGaugeFullSprite;
						StartCoroutine(ActivateEffect(p_UltEffectImg));
						StartCoroutine(UIOnEnumerator(p_UltGaugeImg));
						StartCoroutine(UIOnEnumerator(p_UltOvaSprite));
					}

					p_UltGaugeImg.fillAmount = _fillAmount;
				}
				else
				{
					if (_fillAmount < 1f)
					{
						p_UltGaugeImg.sprite = p_UltGaugeNormalSprite;
						p_UltGaugeImg.fillAmount = _fillAmount;
					}
				}
				break;
		}
	}

	private IEnumerator ActivateEffect(Image _image)
	{
		Material mat = Instantiate(_image.material);
		_image.material = mat;
		
	    int UiTime = Shader.PropertyToID("_UiTime");
		float lerpVal = 0.4f;
		
		while (true)
		{
			_image.material.SetFloat(UiTime, lerpVal);
			lerpVal += Time.unscaledDeltaTime * 3.5f;

			if (lerpVal >= 1f)
			{
				lerpVal = 1f;
				_image.material.SetFloat(UiTime, lerpVal);
				break;
			}

			yield return null;
		}

		yield break;
	}
	
	/// <summary>
	/// Idx에 따라 스킬 Icon을 흔듭니다.
	/// 0, 1, 2 -> Roll, Melee, Ult
	/// </summary>
	/// <param name="_idx"></param>
	public void ShakeImg(int _idx)
	{
		switch (_idx)
		{
			case 0:
				if (!m_RollShakeDelayEnd)
					break;

				if (!ReferenceEquals(m_RollImgCoroutine, null))
				{
					StopCoroutine(m_RollImgCoroutine);
					m_RollImgCoroutine = null;
				}

				p_RollParentTransform.anchoredPosition = m_RollImgOriginPos;
				m_RollImgCoroutine = StartCoroutine(ShakeCoroutine(p_RollParentTransform, 
					2f, 0.5f, 30f));

				m_RollShakeDelayEnd = false;
				m_RollShakeDelayCoroutine =
					StartCoroutine(DelayCoroutine(() => m_RollShakeDelayEnd = true));
				break;
			
			case 1:
				if (!m_MeleeShakeDelayEnd)
					break;
				
				if (!ReferenceEquals(m_MeleeImgCoroutine, null))
				{
					StopCoroutine(m_MeleeImgCoroutine);
					m_MeleeImgCoroutine = null;
				}

				p_MeleeParentTransform.anchoredPosition = m_MeleeImgOriginPos;
				m_MeleeImgCoroutine = StartCoroutine(ShakeCoroutine(p_MeleeParentTransform, 
					2f, 0.5f, 30f));

				m_MeleeShakeDelayEnd = false;
				m_MeleeShakeDelayCoroutine=
					StartCoroutine(DelayCoroutine(() => m_MeleeShakeDelayEnd = true));
				break;
			
			case 2:
				if (!m_UltShakeDelayEnd)
					break;
				
				if (!ReferenceEquals(m_UltImgCoroutine, null))
				{
					StopCoroutine(m_UltImgCoroutine);
					m_UltImgCoroutine = null;
				}
				
				p_UltParentTransform.anchoredPosition = m_UltImgOriginPos;
				m_UltImgCoroutine = StartCoroutine(ShakeCoroutine(p_UltParentTransform, 
					2f, 0.5f, 30f));

				m_UltShakeDelayEnd = false;
				m_UltShakeDelayCoroutine = 
					StartCoroutine(DelayCoroutine(() => m_UltShakeDelayEnd = true));
				break;
		}
	}

	/// <summary>
	/// 스킬 Icon의 흔들림을 강제 정지합니다.
	///  0, 1, 2 -> Roll, Melee, Ult
	/// </summary>
	/// <param name="_idx"></param>
	public void ForceStopShake(int _idx)
	{
		switch (_idx)
		{
			case 0:
				if (!ReferenceEquals(m_RollImgCoroutine, null))
				{
					StopCoroutine(m_RollImgCoroutine);
					m_RollImgCoroutine = null;
					p_RollParentTransform.anchoredPosition = m_RollImgOriginPos;
				}
				break;
			
			case 1:
				if (!ReferenceEquals(m_MeleeImgCoroutine, null))
				{
					StopCoroutine(m_MeleeImgCoroutine);
					m_MeleeImgCoroutine = null;
					p_MeleeParentTransform.anchoredPosition = m_MeleeImgOriginPos;
				}
				break;
			
			case 2:
				if (!ReferenceEquals(m_UltImgCoroutine, null))
				{
					StopCoroutine(m_UltImgCoroutine);
					m_UltImgCoroutine = null;
					p_UltParentTransform.anchoredPosition = m_UltImgOriginPos;
				}
				break;
		}
	}

	private IEnumerator DelayCoroutine(Action _EndAction)
	{
		yield return new WaitForSecondsRealtime(0.3f);
		_EndAction?.Invoke();
		
		yield break;
	}
	
	private IEnumerator ShakeCoroutine(RectTransform _target, float _maxDistance, float _minusDistance, float _lerpSpeed)
	{
		Vector2 initPos = _target.anchoredPosition;
		
		Vector2 Lpos = initPos;
		Lpos.x -= _maxDistance;
		
		Vector2 Rpos = initPos;
		Rpos.x += _maxDistance;

		float LerpVal = 0f;
		bool toLeft = true;
		while (true)
		{
			if (toLeft)
			{
				// R에서 L로
				LerpVal += Time.unscaledDeltaTime * _lerpSpeed;
				
				if (LerpVal > 1f)
				{
					LerpVal = 1f;
					_target.anchoredPosition = Vector2.Lerp(Rpos, Lpos, LerpVal);
					Rpos.x -= _minusDistance;

					LerpVal = 0f;
					toLeft = false;
					
					if (Rpos.x <= initPos.x)
					{
						_target.anchoredPosition = initPos;
						yield break;
					}
				}
				else
				{
					_target.anchoredPosition = Vector2.Lerp(Rpos, Lpos, LerpVal);
				}
			}
			else
			{
				// L에서 R로
				LerpVal += Time.unscaledDeltaTime * _lerpSpeed;
				
				if (LerpVal > 1f)
				{
					LerpVal = 1f;
					_target.anchoredPosition = Vector2.Lerp(Lpos, Rpos, LerpVal);
					Lpos.x += _minusDistance;

					LerpVal = 0f;
					toLeft = true;
					
					if (Lpos.x >= initPos.x)
					{
						_target.anchoredPosition = initPos;
						yield break;
					}
				}
				else
				{
					_target.anchoredPosition = Vector2.Lerp(Lpos, Rpos, LerpVal);
				}
			}
			
			yield return null;
		}
		
		yield break;
	}
}
