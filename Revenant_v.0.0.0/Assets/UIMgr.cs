using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMgr : MonoBehaviour
{
    // Visible Member Variables
    public GameObject m_GameOverUI;

    // Member Variables


    // Constructors
    private void Awake()
    {

    }
    private void Start()
    {
        m_GameOverUI.SetActive(false);
    }
    /*
    <커스텀 초기화 함수가 필요할 경우>
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


    // 기타 분류하고 싶은 것이 있을 경우
}
