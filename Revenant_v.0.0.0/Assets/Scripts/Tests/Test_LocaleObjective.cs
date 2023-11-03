using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Test_LocaleObjective : MonoBehaviour
{
    public Objective_TutoLookAt p_00_Objective;
    public ScriptableLanguage p_00_ObjectiveScript;
    public ScriptableLanguage p_00_Script;
    
    [Space(5f)]
    public Objective_TutoMove p_01_Objective;
    public ScriptableLanguage p_01_ObjectiveScript;
    
    [Space(5f)]
    public Objective_TutoPressButton p_02_Objective;
    public ScriptableLanguage p_02_ObjectiveScript;
    public ScriptableLanguage p_02_Script;
    
    [Space(5f)]
    public Objective_TutoEnterDoor p_03_Objective;
    public ScriptableLanguage p_03_ObjectiveScript;
    
    [Space(5f)]
    public Objective_TutoHide p_04_Objective;
    public ScriptableLanguage p_04_ObjectiveScript;
    public ScriptableLanguage p_04_Script;
    
    [Space(5f)]
    public Objective_TutoRoll p_05_Objective;
    public ScriptableLanguage p_05_ObjectiveScript;
    public ScriptableLanguage p_05_Script;
    
    [Space(5f)]
    public Objective_TutoAttack p_06_Objective;
    public ScriptableLanguage p_06_ObjectiveScript;
    public ScriptableLanguage p_06_Script;
    
    [Space(5f)]
    public Objective_TutoBulletTime p_07_Objective;
    public ScriptableLanguage p_07_ObjectiveScript;
    public ScriptableLanguage p_07_Script;
    
    [Space(5f)]
    public Objective_TutoTakeHatch p_08_Objective;
    public ScriptableLanguage p_08_ObjectiveScript;
    public ScriptableLanguage p_08_Script;

    private void Awake()
    {
        var curLocaleFlag = GameMgr.GetInstance().p_LangMgr.p_CurLanguageFlag;
        
        LocaleObjective(p_00_Objective, ref p_00_ObjectiveScript, curLocaleFlag);
        LocaleDialog(ref p_00_Objective.p_DroneDialogTextList, ref p_00_Script, curLocaleFlag);
        
        LocaleObjective(p_01_Objective, ref p_01_ObjectiveScript, curLocaleFlag);
        
        LocaleObjective(p_02_Objective, ref p_02_ObjectiveScript, curLocaleFlag);
        LocaleDialog(ref p_02_Objective.p_DroneDialogTextList, ref p_02_Script, curLocaleFlag);
        
        LocaleObjective(p_03_Objective, ref p_03_ObjectiveScript, curLocaleFlag);
        
        LocaleObjective(p_04_Objective, ref p_04_ObjectiveScript, curLocaleFlag);
        LocaleDialog(ref p_04_Objective.p_DroneDialogTextList, ref p_04_Script, curLocaleFlag);
        
        LocaleObjective(p_05_Objective, ref p_05_ObjectiveScript, curLocaleFlag);
        LocaleDialog(ref p_05_Objective.p_DroneDialogTextList, ref p_05_Script, curLocaleFlag);
        
        LocaleObjective(p_06_Objective, ref p_06_ObjectiveScript, curLocaleFlag);
        LocaleDialog(ref p_06_Objective.p_DroneDialogTextList, ref p_06_Script, curLocaleFlag);
        
        LocaleObjective(p_07_Objective, ref p_07_ObjectiveScript, curLocaleFlag);
        LocaleDialog(ref p_07_Objective.p_DroneDialogTextList, ref p_07_Script, curLocaleFlag);
        
        LocaleObjective(p_08_Objective, ref p_08_ObjectiveScript, curLocaleFlag);
        LocaleDialog(ref p_08_Objective.p_DroneDialogTextList, ref p_08_Script, curLocaleFlag);
    }

    private void LocaleDialog(ref List<string> _textList, ref ScriptableLanguage _scriptableLanguage,
        Language_Flag _languageFlag)
    {
        _textList.Clear();
        string[] _stringArr;
        
        switch (_languageFlag)
        {
            case Language_Flag.KOREAN:
                _stringArr = _scriptableLanguage.p_KOR.Split("\n");
                for (int i = 0; i < _stringArr.Length; i++)
                {
                    _textList.Add(_stringArr[i]);
                }
                break;
            
            case Language_Flag.ENGLISH:
                _stringArr = _scriptableLanguage.p_ENG.Split("\n");
                for (int i = 0; i < _stringArr.Length; i++)
                {
                    _textList.Add(_stringArr[i]);
                }
                break;
            
            case Language_Flag.JAPANESE:
                _stringArr = _scriptableLanguage.p_JPN.Split("\n");
                for (int i = 0; i < _stringArr.Length; i++)
                {
                    _textList.Add(_stringArr[i]);
                }
                break;
        }
    }
    
    private void LocaleObjective(Objective _objective, ref ScriptableLanguage _scriptableLanguage, 
        Language_Flag _languageFlag)
    {
        string[] _stringArr;
        
        switch (_languageFlag)
        {
            case Language_Flag.KOREAN:
                _stringArr = _scriptableLanguage.p_KOR.Split("\n");
                _objective.m_TitleTxt = _stringArr[0];
                _objective.ClearObjectiveTxt();
                
                for (int i = 1; i < _stringArr.Length; i++)
                {
                    _objective.m_ObjectiveTxtArr[i - 1] = _stringArr[i];
                }
                break;
            
            case Language_Flag.ENGLISH:
                _stringArr = _scriptableLanguage.p_ENG.Split("\n");
                _objective.m_TitleTxt = _stringArr[0];
                _objective.ClearObjectiveTxt();
                
                for (int i = 1; i < _stringArr.Length; i++)
                {
                    _objective.m_ObjectiveTxtArr[i - 1] = _stringArr[i];
                }
                break;
            
            case Language_Flag.JAPANESE:
                _stringArr = _scriptableLanguage.p_JPN.Split("\n");
                _objective.m_TitleTxt = _stringArr[0];
                _objective.ClearObjectiveTxt();
                
                for (int i = 1; i < _stringArr.Length; i++)
                {
                    _objective.m_ObjectiveTxtArr[i - 1] = _stringArr[i];
                }
                break;
        }
    }
}