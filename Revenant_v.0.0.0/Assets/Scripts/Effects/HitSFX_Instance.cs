using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;


public class HitSFX_Instance : MonoBehaviour
{
    // Visible Member Variables
    public Vector2 p_RandomPos = Vector2.zero;
    public float p_RandomAngle = 0f;
    public Light2D[] p_LightsArr;
    
    
    // Member Variables
    private float[] m_LightIntensityArr;

    private Animator m_Animator;
    private Coroutine m_Coroutine;
    
    
    // Constructors
    private void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();

        if (p_LightsArr.Length <= 0)
            return;
        
        m_LightIntensityArr = new float[p_LightsArr.Length];
        for (int i = 0; i < p_LightsArr.Length; i++)
        {
            m_LightIntensityArr[i] = p_LightsArr[i].intensity;
        }
    }
    private void OnEnable()
    {
        m_Animator.Rebind();
        m_Coroutine = StartCoroutine(CheckAnimation());
        transform.rotation = Quaternion.Euler(0f,0f,0f);

        if (p_RandomPos != Vector2.zero)
        {
            float xVal = Random.Range(-p_RandomPos.x, p_RandomPos.x);
            float yVal = Random.Range(-p_RandomPos.y, p_RandomPos.y);
            transform.position = new Vector2(transform.position.x + xVal,
                transform.position.y + yVal);
        }

        if (p_RandomAngle != 0f)
        {
            Quaternion rotationValue = Quaternion.Euler(0,0, Random.Range(-p_RandomAngle, p_RandomAngle));
            transform.rotation = rotationValue;
        }
    }

    private void OnDisable()
    {
        if(m_Coroutine != null)
            StopCoroutine(m_Coroutine);
        
        RestoreLightIntensity();
    }


    // Functions
    private IEnumerator CheckAnimation()
    {
        float animNormalizedTime;
        while (true)
        {
            animNormalizedTime = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            
            DecreaseLightIntensity(animNormalizedTime);
            
            if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                break;
            
            yield return null;
        }

        m_Coroutine = StartCoroutine(DisableObj());
    }

    private IEnumerator DisableObj()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private void DecreaseLightIntensity(float _input)
    {
        for (int i = 0; i < p_LightsArr.Length; i++)
        {
            p_LightsArr[i].intensity = m_LightIntensityArr[i] - (_input * m_LightIntensityArr[i]);
        }
    }
    private void RestoreLightIntensity()
    {
        for (int i = 0; i < p_LightsArr.Length; i++)
        {
            p_LightsArr[i].intensity = m_LightIntensityArr[i];
        }
    }
}