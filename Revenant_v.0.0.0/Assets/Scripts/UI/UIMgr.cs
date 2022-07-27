using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMgr : MonoBehaviour
{
    // Visible Member Variables
    [field: SerializeField] private GameObject m_GameOverUIPrefab;
    public GameObject m_GameOverUI { get; private set; }
    private Scene m_Scene;

    // Member Variables


    // Constructors
    private void Awake()
    {
        m_GameOverUI = Instantiate(m_GameOverUIPrefab, InstanceMgr.GetInstance().GetComponent<InstanceMgr>().m_MainCanvas.transform);
        Screen.SetResolution(1920, 1080, true);

        m_Scene = SceneManager.GetActiveScene();
    }
    private void Start()
    {
        m_GameOverUI.SetActive(false);
    }


    // Updates


    // Physics


    // Functions
 


    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}
