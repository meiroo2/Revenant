using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    // Visible Member Variables

    // Member Variables
    private static StageManager m_stagemgrInstance = null;

    //private List<BasicWeapon> m_Weapons;
    //private int leftHp = 0;
    //private int leftRoll = 0;

    // Constructors
    public static StageManager GetStageMgrInstance
    {
        get
        {
            return m_stagemgrInstance;
        }
    }
    private void Awake()
    {
        if(m_stagemgrInstance == null)
        {
            m_stagemgrInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(m_stagemgrInstance != this)
            {
                Destroy(gameObject);
            }
        }
    }


    // Updates
    private void Update()
    {

    }
    private void FixedUpdate()
    {

    }

    // Physics


    // Functions


    // 기타 분류하고 싶은 것이 있을 경우
}
