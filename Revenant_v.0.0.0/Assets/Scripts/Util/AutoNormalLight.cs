using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class AutoNormalLight : MonoBehaviour
{
    public GameObject m_NormalLightPrefab;
    [ReadOnly] public List<Light2D> m_LightList;
    [ReadOnly] public List<Light2D> m_NormalLightList;

    [Button]
    public void FindAllLights()
    {
        m_LightList.Clear();
        m_LightList.TrimExcess();

        Light2D[] lightArr = GameObject.FindObjectsOfType<Light2D>();

        for (int i = 0; i < lightArr.Length; i++)
        {
            if (lightArr[i].normalMapQuality != Light2D.NormalMapQuality.Accurate)
            {
                m_LightList.Add(lightArr[i]);
            }
        }
    }

    [Button]
    public void GenNormalLight()
    {
        if (ReferenceEquals(m_NormalLightPrefab, null))
        {
            Debug.Log("ERR : AutoNormalLight에 Prefab 없음");
            return;
        }
        
        foreach (var VARIABLE in m_LightList)
        {
            if (VARIABLE.GetComponentsInChildren<Light2D>().Length == 1)
            {
                if (VARIABLE.GetComponent<Light2D>().lightType == Light2D.LightType.Global) 
                    continue;

                if(VARIABLE.GetComponent<Light2D>().normalMapQuality == Light2D.NormalMapQuality.Accurate)
                    continue;
                
                GameObject copyTarget = VARIABLE.gameObject;
                
                Light2D CopiedObj = GameObject.Instantiate(copyTarget, VARIABLE.transform).GetComponent<Light2D>();
                CopiedObj.transform.localPosition = Vector3.zero;
                CopiedObj.transform.localRotation = Quaternion.identity;

                FieldInfo m_GettingField = typeof(Light2D).GetField("m_ApplyToSortingLayers", BindingFlags.NonPublic | BindingFlags.Instance);

                FieldInfo m_SettingField = typeof(Light2D).GetField("m_NormalMapQuality", BindingFlags.NonPublic | BindingFlags.Instance);
                m_SettingField.SetValue(CopiedObj, Light2D.NormalMapQuality.Accurate);
                
                m_SettingField = typeof(Light2D).GetField("m_NormalMapDistance", BindingFlags.NonPublic | BindingFlags.Instance);
                m_SettingField.SetValue(CopiedObj, 0f);
                
                m_SettingField = typeof(Light2D).GetField("m_ApplyToSortingLayers", BindingFlags.NonPublic | BindingFlags.Instance);
                m_SettingField.SetValue(CopiedObj, m_GettingField.GetValue(m_NormalLightPrefab.GetComponent<Light2D>()));

                CopiedObj.intensity += 1f;
                
                m_NormalLightList.Add(CopiedObj);
            }
        }
    }

    
}