using System;
using UnityEngine;


public class ScnScripts_29_Option : MonoBehaviour
{
    private bool check_BtnInput = false;
    
    public void BtnInput_ChangeLanguage(int _languageFlag)
    {
        if (check_BtnInput)
            return;

        var langMgr = GameMgr.GetInstance().p_LangMgr;
       langMgr.ChangeLanguageFlag((Language_Flag) _languageFlag);
       langMgr.ForceResetAllTMP(langMgr.p_CurLanguageFlag);
    }

    public void ToNextScene()
    {
        if (check_BtnInput)
            return;

        check_BtnInput = true;
        GameMgr.GetInstance().RequestLoadScene(1);
    }
}