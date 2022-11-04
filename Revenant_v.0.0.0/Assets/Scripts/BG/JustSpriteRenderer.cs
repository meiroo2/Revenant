using UnityEngine;

public class JustSpriteRenderer : MonoBehaviour, ISpriteMatChange
{
    public bool m_IgnoreMatChanger { get; set; }
    public SpriteType m_SpriteType { get; set; }
    public SpriteMatType m_CurSpriteMatType { get; set; }
    public Material p_OriginalMat { get; set; }
    public Material p_BnWMat { get; set; }
    public Material p_RedHoloMat { get; set; }
    public Material p_DisappearMat { get; set; }
    public void ChangeMat(SpriteMatType _matType)
    {
        throw new System.NotImplementedException();
    }

    public void InitISpriteMatChange()
    {
        throw new System.NotImplementedException();
    }
}