using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;


public class TMP_MultiLanguage : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI p_TMP { get; private set; }
    [field: SerializeField] public RectTransform p_Rect { get; private set; }
    [field: SerializeField] public ScriptableLanguage p_ScriptableLanguage { get; private set; }
    [field: SerializeField] public bool p_ForceFixPosition = false;
    [field: SerializeField] public float p_ForceAddFontSize { get; private set; } = 0f;
    [field: SerializeField] public float p_AdjustYValue { get; private set; } = 0f;

    public float m_OriginalKORSize { get; private set; } = 10f;
    public Vector2 m_OriginalKORPosition { get; private set; }

    public void Awake()
    {
        m_OriginalKORPosition = p_Rect.anchoredPosition;
        m_OriginalKORSize = p_TMP.fontSize;
        GameMgr.GetInstance().p_LangMgr.ChangeLanguage(this);
    }

    [Button]
    public void ResetToDefault()
    {
        p_TMP = GetComponent<TextMeshProUGUI>();
        p_Rect = GetComponent<RectTransform>();
        p_ScriptableLanguage.p_KOR = p_TMP.text;
        p_ScriptableLanguage.p_ENG = p_TMP.text;
        p_ScriptableLanguage.p_JPN = p_TMP.text;

        gameObject.name = p_ScriptableLanguage.name;
        
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(p_ScriptableLanguage);
#endif
        
    }
}