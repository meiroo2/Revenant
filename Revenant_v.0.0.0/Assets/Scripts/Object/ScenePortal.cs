using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour, IUseableObj
{
    // Visible Member Variables
    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.OBJECT;
    public bool m_isOn { get; set; } = false;
    public string m_LoadSceneString;

    // Member Variables


    // Constructors


    // Updates


    // Physics


    // Functions
    public int useObj(IUseableObjParam _param)
    {
        SceneManager.LoadScene(m_LoadSceneString);
        return 1;
    }

    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}
