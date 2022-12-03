using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

public class JustSpriteRenderer : MonoBehaviour, ISpriteMatChange
{
    // Member Variables
    private SpriteRenderer p_Renderer;
    
    // Constructors
    private void Awake()
    {
        p_Renderer = GetComponent<SpriteRenderer>();
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
                p_Renderer.material = p_OriginalMat;
                break;
            
            case SpriteMatType.BnW:
                p_Renderer.material = p_BnWMat;
                break;
            
            case SpriteMatType.REDHOLO:
                p_Renderer.material = p_RedHoloMat;
                break;
            
            case SpriteMatType.DISAPPEAR:
                p_Renderer.material = p_DisappearMat;
                break;
        }
    }

    public void InitISpriteMatChange()
    {
        m_SpriteType = SpriteType.BACKGROUND;
        m_CurSpriteMatType = SpriteMatType.ORIGIN;
        p_OriginalMat = p_Renderer.material;
    }
}