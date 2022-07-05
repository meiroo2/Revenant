using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.PlayerLoop;


public class HitSFX_ObjPull : ForObjPull_Once
{
    // Visible Member Variables
    [field: SerializeField] public bool m_HaveParticle { get; private set; } = false;

    // Member Variables;
    private Light2D m_Light;
    private AnimatorStateInfo m_AniState;
    private int m_Phase = 0;
    
    
    private new void Awake()
    {
        base.Awake();
        m_Light = GetComponentInChildren<Light2D>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!m_isStart) 
            return;
        
        if (!m_HaveParticle)    // 파티클 이펙트 존재하지 않을 경우(스프라이트만 있음)
        {
            m_AniState = m_Animator.GetCurrentAnimatorStateInfo(0);

            switch (m_Phase)
            {
                case 0:
                    if (m_AniState.normalizedTime >= 0.5f)
                    {
                        m_Light.gameObject.SetActive(false);
                        m_Phase = 1;
                    }
                    break;
                
                case 1:
                    if (m_AniState.normalizedTime >= 1f)
                    {
                        m_isStart = false;
                        m_Phase = 2;
                        gameObject.SetActive(false);
                    }
                    break;
            }
        }
        else
        {
            m_AniState = m_Animator.GetCurrentAnimatorStateInfo(0);

            switch (m_Phase)
            {
                case 0:
                    if (m_AniState.normalizedTime >= 0.5f)
                    {
                        m_Light.gameObject.SetActive(false);
                        m_Phase = 1;
                    }
                    break;
                
                case 1:
                    StartCoroutine(Internal_Timer(1f));
                    m_Phase = 2;
                    break;
                
                case 2:
                    break;
            }
        }
    }

    public override void resetTimer(float _inputTime)
    {
        m_Phase = 0;
        m_Light.gameObject.SetActive(true);
        m_Timer = _inputTime;
        m_isStart = true;
    }
    
    private IEnumerator Internal_Timer(float _time)
    {
        yield return new WaitForSeconds(_time);
        m_isStart = false;
        gameObject.SetActive(false);
    }
}