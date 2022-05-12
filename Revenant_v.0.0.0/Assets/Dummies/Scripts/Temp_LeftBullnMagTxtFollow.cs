using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Temp_LeftBullnMagTxtFollow : MonoBehaviour
{
    private GameObject m_OriginLeftBulletTxt;
    private Text m_Text;

    private TextMeshProUGUI m_TMP;

    private void Start()
    {
        m_Text = GetComponent<Text>();
        m_OriginLeftBulletTxt = InstanceMgr.GetInstance().m_MainCanvas.GetComponentInChildren<Player_UI>().m_WeaponTexts[0];
        m_TMP = m_OriginLeftBulletTxt.GetComponent<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        m_Text.text = m_TMP.text;
    }
}