using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMgr : MonoBehaviour
{
    // Visible Member Variables
    [field: SerializeField] private GameObject m_GameOverUIPrefab;
    public GameObject m_GameOverUI { get; private set; }

    // Member Variables


    // Constructors
    private void Awake()
    {
        m_GameOverUI = Instantiate(m_GameOverUIPrefab, GameManager.GetInstance().GetComponent<GameManager>().m_MainCanvas.transform);
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
