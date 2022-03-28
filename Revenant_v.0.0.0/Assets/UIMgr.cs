using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
    // Visible Member Variables
    public GameObject m_GameOverUI;
    public GameObject m_Canvas;

    // Member Variables


    // Constructors
    private void Awake()
    {
        m_Canvas.SetActive(true);
        Screen.SetResolution(1920, 1080, true);
    }
    private void Start()
    {
        m_GameOverUI.SetActive(false);
    }
    /*
    <Ŀ���� �ʱ�ȭ �Լ��� �ʿ��� ���>
    public void Init()
    {

    }
    */

    // Updates
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
            CurSceneReload();
    }
    private void FixedUpdate()
    {

    }

    // Physics


    // Functions
    public void CurSceneReload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}
