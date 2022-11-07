using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CW_RageGauge : OdinEditorWindow
{
    // Const Variables
    private const float m_LabelWidth = 170f;

    
    // Constructors
    [MenuItem(("에디터/광분 게이지"))]
    private static void OpenWindow()
    {
        GetWindow<CW_RageGauge>().Show();
        LoadRageGaugeData();
    }

    
    // Member Variables
    [TabGroup("RageGauge"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    private static float R_Gauge_Max;
    
    [TabGroup("RageGauge"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    private static float R_Gauge_Refill_Nature;

    [TabGroup("RageGauge"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    private static float R_Gauge_Refill_Attack_Multi;

    [TabGroup("RageGauge"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    private static float R_Gauge_Refill_Evade;

    [TabGroup("RageGauge"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    private static float R_Gauge_Refill_JustEvade;

    [TabGroup("RageGauge"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    private static float R_Gauge_Refill_Limit;

    [TabGroup("RageGauge"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    private static float R_Gauge_Consume_Nature;

    [TabGroup("RageGauge"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    private static float R_Gauge_Consume_Roll;

    [TabGroup("RageGauge"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    private static float R_Gauge_Consume_Melee;
    
    
    // Functions
    [PropertySpace(20f), Button(ButtonSizes.Large), TabGroup("RageGauge")]
    private static void RageGauge_적용하기()
    {
        var gauge = Resources.Load("UIs/RageGauge").GetComponent<RageGauge>();

        gauge.p_Gauge_Max = R_Gauge_Max;
        gauge.p_Gauge_Refill_Nature = R_Gauge_Refill_Nature;
        gauge.p_Gauge_Refill_Attack_Multi = R_Gauge_Refill_Attack_Multi;
        gauge.p_Gauge_Refill_Evade = R_Gauge_Refill_Evade;
        gauge.p_Gauge_Refill_JustEvade = R_Gauge_Refill_JustEvade;
        gauge.p_Gauge_Refill_Limit = R_Gauge_Refill_Limit;

        gauge.p_Gauge_Consume_Nature = R_Gauge_Consume_Nature;
        gauge.p_Gauge_Consume_Roll = R_Gauge_Consume_Roll;
        gauge.p_Gauge_Consume_Melee = R_Gauge_Consume_Melee;
        
        
        #if UNITY_EDITOR
            EditorUtility.SetDirty(gauge);
        #endif
    }

    private static void LoadRageGaugeData()
    {
        var gauge = Resources.Load("UIs/RageGauge").GetComponent<RageGauge>();

		R_Gauge_Max = gauge.p_Gauge_Max;
        R_Gauge_Refill_Nature = gauge.p_Gauge_Refill_Nature;
        R_Gauge_Refill_Attack_Multi = gauge.p_Gauge_Refill_Attack_Multi;
        R_Gauge_Refill_Evade = gauge.p_Gauge_Refill_Evade;
        R_Gauge_Refill_JustEvade = gauge.p_Gauge_Refill_JustEvade;
        R_Gauge_Refill_Limit = gauge.p_Gauge_Refill_Limit;

        R_Gauge_Consume_Nature = gauge.p_Gauge_Consume_Nature;
        R_Gauge_Consume_Roll = gauge.p_Gauge_Consume_Roll;
        R_Gauge_Consume_Melee = gauge.p_Gauge_Consume_Melee;
    }
}
