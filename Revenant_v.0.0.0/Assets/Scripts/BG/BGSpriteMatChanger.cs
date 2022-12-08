using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;


public class BGSpriteMatChanger : MonoBehaviour, ISpriteMatChange
{
    // Visible Member Variables
    public Material p_LitMat;
    public bool p_UseOriginMat = false;

    // Member Variables
    private List<SpriteRenderer> m_RendererList = new List<SpriteRenderer>();
    private List<Material> m_MatList = new List<Material>();
    
    // Constructors
    private void Awake()
    {
        m_RendererList.AddRange(GetComponentsInChildren<SpriteRenderer>());

        List<SpriteRenderer> exceptionList = new List<SpriteRenderer>();
        foreach (var VARIABLE in m_RendererList)
        {
            if (VARIABLE.gameObject.CompareTag("Exception"))
            {
                exceptionList.Add(VARIABLE);
            }
        }

        foreach (var VARIABLE in exceptionList)
        {
            m_RendererList.Remove(VARIABLE);
        }

        foreach (var VARIABLE in m_RendererList)
        {
            m_MatList.Add(VARIABLE.material);
        }
      
        InitISpriteMatChange();
    }


    // ISpriteMatChange
    [field: SerializeField, BoxGroup("ISpriteMatChange")] public bool m_IgnoreMatChanger { get; set; } = false;
    public SpriteType m_SpriteType { get; set; } = SpriteType.BACKGROUND;
    public SpriteMatType m_CurSpriteMatType { get; set; } = SpriteMatType.ORIGIN;
    public Material p_OriginalMat { get; set; }
    [field: SerializeField, BoxGroup("ISpriteMatChange")] public Material p_BnWMat { get; set; }
    [field: SerializeField, BoxGroup("ISpriteMatChange")] public Material p_RedHoloMat { get; set; }
    [field: SerializeField, BoxGroup("ISpriteMatChange")] public Material p_DisappearMat { get; set; }
    
    
    public void ChangeMat(SpriteMatType _matType)
    {
        if (m_IgnoreMatChanger)
            return;

        m_CurSpriteMatType = _matType;
        switch (_matType)
        {
            case SpriteMatType.ORIGIN:
                for (int i = 0; i < m_RendererList.Count; i++)
                {
                    if (p_UseOriginMat)
                    {
                        m_RendererList[i].material = m_MatList[i];
                    }
                    else
                    {
                        m_RendererList[i].material = p_OriginalMat;
                    }
                }
                break;
            
            case SpriteMatType.BnW:
                for (int i = 0; i < m_RendererList.Count; i++)
                {
                    m_RendererList[i].material = p_BnWMat;
                }
                break;
        }
    }

    public void InitISpriteMatChange()
    {
        m_SpriteType = SpriteType.BACKGROUND;
        m_CurSpriteMatType = SpriteMatType.ORIGIN;
        p_OriginalMat = p_LitMat;
    }
}