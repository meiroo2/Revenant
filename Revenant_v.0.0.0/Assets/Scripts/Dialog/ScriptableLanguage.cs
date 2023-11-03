using UnityEngine;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Dialog", menuName = "ScriptableLanguage", order = 1)]
public class ScriptableLanguage : ScriptableObject
{
    [LabelWidth(50), Multiline(15)]
    public string p_KOR;
    
    [LabelWidth(50), Multiline(15)]
    public string p_ENG;

    [LabelWidth(50), Multiline(15)]
    public string p_JPN;
}