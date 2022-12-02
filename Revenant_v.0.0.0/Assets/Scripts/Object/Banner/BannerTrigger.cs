using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BannerTrigger : MonoBehaviour
{
    private int AnimationPlayCount = 0;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Coroutine _coroutine;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _spriteRenderer.enabled = false;
        _animator.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _spriteRenderer.enabled = true;
            _animator.enabled = true;
            _animator.SetBool("TurnOn", true);
            _coroutine = StartCoroutine(PlayNextIdleAnimation());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetBool("TurnOn", false);
            DisableCoroutine();

            _coroutine = StartCoroutine(DisablePlayingAnimation());
        }
    }

    private void DisableCoroutine()
    {
        if (!ReferenceEquals(_coroutine, null))
        {
            StopCoroutine(_coroutine);
        }
    }
    
    /** Idle 애니메이션을 조건에 맞춰 한번만 재생 */
    private IEnumerator PlayNextIdleAnimation()
    {
        AnimatorStateInfo stateInfo;
        
        while (true)
        {
            yield return null;
            stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.normalizedTime >= 1f)
            {
                AnimationPlayCount++;
                if (AnimationPlayCount != 5)
                {
                    _animator.SetInteger("IdleCount", 1);
                }
                else if (AnimationPlayCount == 5)
                {
                    AnimationPlayCount = 0;
                    _animator.SetInteger("IdleCount", 2);
                }
                yield return null;
            }
        }
    }

    /** 애니메이션 끝나면 Animator와 SpriteRenderer 비활성화하는 함수 */
    private IEnumerator DisablePlayingAnimation()
    {
        AnimatorStateInfo stateInfo;
        
        while (true)
        {
            yield return null;

            stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            
            if (stateInfo.normalizedTime >= 1f)
            {
                _animator.enabled = false;
                _spriteRenderer.enabled = false;
                break;
            }
        }
        
        yield break;
    }
}

