using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;


public enum Language_Flag
{
    KOREAN = 0,
    ENGLISH = 1,
    JAPANESE = 2
}

public class LanguageManager : MonoBehaviour
{
    // Member Variables
    [field: SerializeField]
    public TMP_FontAsset[] p_FontAssets { get; private set; }

    public Language_Flag p_CurLanguageFlag { get; private set; } = Language_Flag.KOREAN;
    
    // Const Variables
    private const float m_JPNFontSize = 5f;
    private readonly Vector2 m_JPNFontPosition = new Vector2(0f, 6f);
    
    public void ChangeLanguageFlag(Language_Flag _languageFlag)
    {
        p_CurLanguageFlag = _languageFlag;
    }

    [Button]
    public void ForceResetAllTMP(Language_Flag _languageFlag)
    {
        p_CurLanguageFlag = _languageFlag;
        GameObject[] objArr = GameObject.FindGameObjectsWithTag("MulLangDialog");

        for (int i = 0; i < objArr.Length; i++)
        {
            ChangeLanguage(objArr[i].GetComponent<TMP_MultiLanguage>());
        }
    }

    public void ChangeLanguage(TMP_MultiLanguage _tmp)
    {
        switch (p_CurLanguageFlag)
        {
            case Language_Flag.KOREAN:
                EditTMP(_tmp.p_TMP, p_FontAssets[0], _tmp.p_ScriptableLanguage.p_KOR,
                    _tmp.m_OriginalKORSize, _tmp.p_Rect, _tmp.m_OriginalKORPosition);
                break;
            
            case Language_Flag.ENGLISH:
                EditTMP(_tmp.p_TMP, p_FontAssets[1], _tmp.p_ScriptableLanguage.p_ENG,
                    _tmp.m_OriginalKORSize, _tmp.p_Rect, _tmp.m_OriginalKORPosition);
                break;
            
            case Language_Flag.JAPANESE:
                Vector2 _txtPos = _tmp.p_ForceFixPosition
                    ? _tmp.m_OriginalKORPosition
                    : _tmp.m_OriginalKORPosition + m_JPNFontPosition;
                
                _txtPos.y += _tmp.p_AdjustYValue;
                
                EditTMP(_tmp.p_TMP, p_FontAssets[2],
                    _tmp.p_ScriptableLanguage.p_JPN,
                     _tmp.m_OriginalKORSize + m_JPNFontSize + _tmp.p_ForceAddFontSize, 
                    _tmp.p_Rect, 
                    _txtPos);
                break;
        }
    }

    private void EditTMP(TextMeshProUGUI _tmp, TMP_FontAsset _fontAsset, string _text,
        float _fontSize, RectTransform _rect, Vector2 _position)
    {
        _tmp.font = _fontAsset;
        _tmp.text = _text;
        _tmp.fontSize = _fontSize;
        _rect.anchoredPosition = new Vector3(_position.x, _position.y, 0f);
    }
}