using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class DynamicUI : MonoBehaviour
{
    // Member Variables
    private Coroutine m_ExpandCoroutine;
    private Coroutine m_ShakeCoroutine;
    
    public virtual void ExpandUI(RectTransform _target, Vector2 _scaleInit ,Vector2 _scaleTo, float _Speed)
    {
        if (!ReferenceEquals(m_ExpandCoroutine, null))
        {
            StopCoroutine(m_ExpandCoroutine);
        }
        _target.localScale = _scaleInit;
        m_ExpandCoroutine = StartCoroutine(ExpandCoroutine(_target, _scaleTo, _Speed));
    }

    protected IEnumerator ExpandCoroutine(RectTransform _target, Vector2 _scale, float _speed)
    {
        while (true)
        {
            _target.localScale = Vector2.Lerp(_target.localScale, _scale, Time.deltaTime * _speed);

            if (_scale.x - _target.localScale.x <= 0)
            {
                break;
            }
            yield return null;
        }

        _target.localScale = _scale;
        yield break;
    }

    public virtual void Shake(RectTransform _target, Vector2 _initPos, float _distance, float _lerpSpeed, float _mValue, float _mSpeed)
    {
        if (!ReferenceEquals(m_ShakeCoroutine, null))
        {
            StopCoroutine(m_ShakeCoroutine);
        }
        
        _target.anchoredPosition = _initPos;
        m_ShakeCoroutine = StartCoroutine(ShakeCoroutine(_target, _distance, _lerpSpeed, _mValue, _mSpeed));
    }

    protected IEnumerator ShakeCoroutine(RectTransform _target, float _distance, float _lerpSpeed, float _mValue, float _mSpeed)
    {
        Vector2 initPos = _target.anchoredPosition;
        Vector2 pos = initPos;
        float leftX = pos.x - _distance;
        float rightX = pos.x + _distance;

        float lerpSpeed = _lerpSpeed;

        bool _toright = true;
        
        while (true)
        {
            if (leftX >= initPos.x || rightX <= initPos.x)
            {
                break;
            }
            
            if (_toright)
            {
                pos.x += Time.deltaTime * _lerpSpeed;
                if (pos.x > rightX)
                {
                    _toright = false;
                    rightX -= _mValue;
                    lerpSpeed -= _mSpeed;
                }
            }
            else
            {
                pos.x -= Time.deltaTime * _lerpSpeed;
                if (pos.x < leftX)
                {
                    _toright = true;
                    leftX += _mValue;
                    lerpSpeed -= _mSpeed;
                }
            }

            _target.anchoredPosition = Vector2.Lerp(_target.anchoredPosition, pos, Time.deltaTime * lerpSpeed);
            
            yield return null;
        }

        _target.anchoredPosition = initPos;

        yield break;
    }
}