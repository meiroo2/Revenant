using UnityEngine;



public class ScreenMgr : MonoBehaviour
{
    private bool m_IsFull = true;
    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (m_IsFull)
                {
                    Screen.SetResolution(1920,1080,FullScreenMode.Windowed);
                    m_IsFull = false;
                }
                else
                {
                    Screen.SetResolution(1920,1080,FullScreenMode.ExclusiveFullScreen);
                    m_IsFull = true;
                }
            }
        }
    }
}