using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum MapClearData
{
    TUTORIAL, STAGE01, STAGE02, STAGE03, STAGE04, STAGE05, STAGE06, STAGE07, STAGE08
}
// �ؽ�Ʈ�� ���� �� ��Ȳ�� ǥ��
public class GameDataController : MonoBehaviour
{
    public TextMeshProUGUI stageText;
    MapClearData mapClearData;
    private void Start()
    {
        int a = (int)mapClearData;
        Debug.Log((MapClearData)a);
        
    }
    public void save(int m)
    {
        PlayerPrefs.SetInt("MapClearData", ((int)m));
        mapClearData = (MapClearData)m;
    }
    public void load()
    {
        if(stageText)
        {
            if (PlayerPrefs.HasKey("MapClearData")) // Ŭ���� ������ Ű�� �����ϴ���
            {
                Debug.Log(PlayerPrefs.GetInt("MapClearData"));
                stageText.text = ((MapClearData)PlayerPrefs.GetInt("MapClearData")).ToString();
            }
                

            else
                stageText.text = "no Data";
        }
        
    }
    public void reset()
    {
        PlayerPrefs.DeleteKey("MapClearData");
    }
}