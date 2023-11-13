using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy.BossGang
{
    public class BossGang_SpeedDownCol : MonoBehaviour
    {
        public float PlayerSpeedRatio = 0.5f;

        private Player _player = null;
        private Coroutine _lifeTimeCoroutine = null;
        private Animator _animator;
        private Collider2D _collider;
        
        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _collider = GetComponentInChildren<Collider2D>();
        }

        [Button]
        public void SetLifeTime(float time)
        {
            if (!ReferenceEquals(_lifeTimeCoroutine, null))
            {
                StopCoroutine(_lifeTimeCoroutine);
                _lifeTimeCoroutine = null;
            }

            _lifeTimeCoroutine = StartCoroutine(lifeTimeEnumerator(time));
        }

        private IEnumerator lifeTimeEnumerator(float timer)
        {
            yield return new WaitForSeconds(timer);

            _collider.enabled = false;
            
            if(!ReferenceEquals(_player, null))
                _player.SetPlayerSpeedRatio(1f);
            
            _animator.SetBool("Disappear", true);
            yield return new WaitForSeconds(2f);
            Destroy(this.gameObject);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag($"@Player"))
                return;
            
            if (other.TryGetComponent(out _player))
            {
                _player.SetPlayerSpeedRatio(PlayerSpeedRatio);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag($"@Player"))
                return;
            
            if (other.TryGetComponent(out _player))
            {
                _player.SetPlayerSpeedRatio(1f);
            }
        }
    }
}