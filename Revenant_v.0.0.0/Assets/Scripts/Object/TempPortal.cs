using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempPortal : MonoBehaviour, IUseableObj
{
    // Visible Member Variables
    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.OBJECT;
    public bool m_isOn { get; set; } = false;
    public int m_LoadSceneIdx = 0;

    // Member Variables


    // Constructors


    // Updates


    // Physics


    // Functions
    public bool useObj()
    {
        SceneManager.LoadScene("Stage01_02");
        return true;
    }

    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}