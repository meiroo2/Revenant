using UnityEngine;
using UnityEngine.PlayerLoop;


public class GunFire_ObjPull : ForObjPull_Once
{
    private void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (m_isStart)
        {
            if(m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f)
            {
                gameObject.SetActive(false);
                m_isStart = false;
            }
        }
    }
}