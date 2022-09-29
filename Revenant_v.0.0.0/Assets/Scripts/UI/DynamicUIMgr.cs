using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class DynamicUIMgr : MonoBehaviour
{
    // Member Variables
    private Dictionary<RectTransform, Coroutine> m_ExpandDic = new Dictionary<RectTransform, Coroutine>();
    private Dictionary<RectTransform, Coroutine> m_ShakeDic = new Dictionary<RectTransform, Coroutine>();
    private Dictionary<Image, Coroutine> m_ColorDic = new Dictionary<Image, Coroutine>();
    
    
    private Dictionary<Text, Coroutine> m_TextFadeDic = new Dictionary<Text, Coroutine>();

    public void FadeUI(Text _text, bool _isFadeIn, float _speed)
    {
        if (m_TextFadeDic.TryGetValue(_text, out Coroutine element))
        {
            if (!ReferenceEquals(element, null))
            {
                StopCoroutine(element);
            }
            
            m_TextFadeDic.Remove(_text);
            
            Color txtColor = _text.color;
            txtColor.a = _isFadeIn ? 0f : 1f;
            
            _text.color = txtColor;
        }
        
        m_TextFadeDic.Add(_text, StartCoroutine(FadeUI_TxtCoroutine(_text, _isFadeIn, _speed)));
    }

    private IEnumerator FadeUI_TxtCoroutine(Text _txt, bool _isFadeIn, float _speed)
    {
        Color txtColor = _txt.color;
        
        while (true)
        {
            if (_isFadeIn)
            {
                txtColor.a += Time.deltaTime * _speed;
                if (txtColor.a >= 1f)
                {
                    txtColor.a = 1f;
                    _txt.color = txtColor;
                    break;
                }
            }
            else
            {
                txtColor.a -= Time.deltaTime * _speed;
                if (txtColor.a <= 1f)
                {
                    txtColor.a = 0f;
                    _txt.color = txtColor;
                    break;
                }
            }

            _txt.color = txtColor;
            yield return null;
        }
        
        yield break;
    }

    public void ChangeColor(Image _image, Color _initColor, Color _destColor, float _speed)
    {
        if (m_ColorDic.TryGetValue(_image, out Coroutine element))
        {
            if (!ReferenceEquals(element, null))
                StopCoroutine(element);

            _image.color = _initColor;
            m_ColorDic.Remove(_image);
        }
        
        m_ColorDic.Add(_image, StartCoroutine(ChangeColorCoroutine(_image, _destColor, _speed)));
    }

    private IEnumerator ChangeColorCoroutine(Image _image, Color _destColor, float _speed)
    {
        Color imgColor = _image.color;
        float imgColorSum = 0f;
        float destColorSum = _destColor.r + _destColor.g + _destColor.b + _destColor.a;
        
        while (true)
        {
            imgColor = Color.Lerp(imgColor, _destColor, Time.unscaledDeltaTime * _speed);
            _image.color = imgColor;

            imgColorSum = imgColor.r + imgColor.g + imgColor.b + imgColor.a;
            
            if (Mathf.Abs(destColorSum - imgColorSum) < 0.1f)
            {
                break;
            }
            yield return null;
        }
        
        yield break;
    }
    
    public void ExpandUI(RectTransform _target, Vector2 _initScale ,Vector2 _scaleTo, float _Speed)
    {
        if (m_ExpandDic.TryGetValue(_target, out Coroutine element))
        {
            if (!ReferenceEquals(element, null))
                StopCoroutine(element);
            
            _target.localScale = _initScale;
            m_ExpandDic.Remove(_target);
        }

        m_ExpandDic.Add(_target, StartCoroutine(ExpandCoroutine(_target, _scaleTo, _Speed)));
    }

    private IEnumerator ExpandCoroutine(RectTransform _target, Vector2 _destScale, float _speed)
    {
        float targetScale = 0f;
        float destScale = _destScale.x + _destScale.y;
        
        while (true)
        {
            _target.localScale = Vector2.Lerp(_target.localScale, _destScale, Time.unscaledDeltaTime * _speed);
            targetScale = _target.localScale.x + _target.localScale.y;


            if (Mathf.Abs(destScale - targetScale) <= 0.001f)
            {
                break;
            }

            yield return null;
        }

        _target.localScale = _destScale;
        yield break;
    }

    public void Shake(RectTransform _target, Vector2 _initPos, float _distance, float _lerpSpeed, float _mValue, float _mSpeed)
    {
        if (m_ShakeDic.TryGetValue(_target, out Coroutine element))
        {
            if (!ReferenceEquals(element, null))
                StopCoroutine(element);
            
            _target.anchoredPosition = _initPos;
            m_ShakeDic.Remove(_target);
        }
     
        m_ShakeDic.Add(_target, StartCoroutine(ShakeCoroutine(_target, _distance, _lerpSpeed, _mValue, _mSpeed)));
    }

    private IEnumerator ShakeCoroutine(RectTransform _target, float _distance, float _lerpSpeed, float _mValue, float _mSpeed)
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
                pos.x += Time.unscaledDeltaTime * _lerpSpeed;
                if (pos.x > rightX)
                {
                    _toright = false;
                    rightX -= _mValue;
                    lerpSpeed -= _mSpeed;
                }
            }
            else
            {
                pos.x -= Time.unscaledDeltaTime * _lerpSpeed;
                if (pos.x < leftX)
                {
                    _toright = true;
                    leftX += _mValue;
                    lerpSpeed -= _mSpeed;
                }
            }

            _target.anchoredPosition = Vector2.Lerp(_target.anchoredPosition, pos, Time.unscaledDeltaTime * lerpSpeed);
            
            yield return null;
        }

        _target.anchoredPosition = initPos;

        yield break;
    }
}