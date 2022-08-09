using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class ScenePortal : MonoBehaviour, IUseableObj
{
    // Visible Member Variables
    public bool m_IsOutlineActivated { get; set; }
    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.OBJECT;
    public bool m_isOn { get; set; } = false;
    public string p_LoadSceneName;

    // Member Variables


    // Constructors


    // Updates


    // Physics


    // Functions
    public int useObj(IUseableObjParam _param)
    {
        SceneManager.LoadScene(p_LoadSceneName);
        return 1;
    }

    // 기타 분류하고 싶은 것이 있을 경우
}
