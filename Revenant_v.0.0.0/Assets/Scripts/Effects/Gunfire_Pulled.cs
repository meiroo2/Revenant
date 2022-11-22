using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class Gunfire_Pulled : ForObjPull_Once
{
    // Visible Member Variables
    public float p_IntensityFadeoutSpeed = 1f;
    
    [Space(20f)]
    [Header("할당 필요")]
    public Light2D p_NormalMapLight;


    // Member Variables
    private float[] m_NormalizeVal;
    private Coroutine m_CurCoroutine;
    
    
    // Constructors
    private new void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        
        if(p_NormalMapLight == null)
            Debug.Log("Gunfire_Pulled에 Light 없음");

        m_NormalizeVal = new float[2];
        m_NormalizeVal[0] = p_NormalMapLight.intensity;
    }

    /// <summary>라이트의 Intensity를 초기화하고, 작동 중인 코루틴을 재시작합니다.</summary>
    public override void InitPulledObj()
    {
        p_NormalMapLight.intensity = m_NormalizeVal[0];

        if (!ReferenceEquals(m_CurCoroutine, null))
            StopCoroutine(m_CurCoroutine);
        
        m_CurCoroutine = StartCoroutine(WaitUntilAnimFinish());
    }

    private IEnumerator WaitUntilAnimFinish()
    {
        float deltaTime;
        while (true)
        {
            if (p_NormalMapLight.intensity > 0f)
            {
                deltaTime = Time.deltaTime * p_IntensityFadeoutSpeed;
                p_NormalMapLight.intensity -= deltaTime * m_NormalizeVal[0];
            }

            if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                p_NormalMapLight.intensity = 0f;
                break;
            }
            yield return null;
        }
        
        gameObject.SetActive(false);
        yield break;
    }
}