using UnityEngine;

public enum SpriteType
{
    PLAYER,
    ENEMY,
    BACKGROUND
}
public enum SpriteMatType
{
    ORIGIN,
    BnW,
    REDHOLO,
    DISAPPEAR
}

/// <summary>
/// SpriteRenderer의 Material 변경을 손쉽게 하기 위한 인터페이스입니다.
/// 프로퍼티를 반드시 [field: SerializeField] 해주세요
/// </summary>
public interface ISpriteMatChange
{
    public bool m_IgnoreMatChanger { get; set; }
    public SpriteType m_SpriteType { get; set; }
    public SpriteMatType m_CurSpriteMatType { get; set; }
    public Material p_OriginalMat { get; set; }
    public Material p_BnWMat { get; set; }
    public Material p_RedHoloMat { get; set; }
    public Material p_DisappearMat { get; set; }

    public void ChangeMat(SpriteMatType _matType);
    public void InitISpriteMatChange();
}