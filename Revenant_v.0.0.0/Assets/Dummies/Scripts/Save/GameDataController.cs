using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum MapClearData
{
    TUTORIAL, STAGE01, STAGE02, STAGE03, STAGE04, STAGE05, STAGE06, STAGE07, STAGE08
}
// 텍스트에 현재 맵 상황을 표시
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
            if (PlayerPrefs.HasKey("MapClearData")) // 클리어 데이터 키가 존재하는지
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