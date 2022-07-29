using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader_Signal : MonoBehaviour
{
    // Visible Member Variables
    public string m_LoadSceneName;
    
    
    // Functions
    public void LoadSceneUsingName()
    {
        SceneManager.LoadScene(m_LoadSceneName);
    }
}