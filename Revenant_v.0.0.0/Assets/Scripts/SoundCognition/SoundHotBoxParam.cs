using UnityEngine;


/// <summary>
/// Sound의 종류를 나타냅니다.
/// </summary>
public enum SOUNDTYPE
{
    BULLET,
    EXPLOSION,
    PLAYERNOISE
}

/// <summary>
/// SoundHotBox에 전달할 파라미터입니다. Sound의 정보를 나타냅니다.
/// </summary>
public class SoundHotBoxParam
{
    // Member Variables
    public Vector2 m_SoundPos { get; private set; } = Vector2.zero;
    public bool m_IsPlayers { get; private set; } = false;
    public SOUNDTYPE m_SoundType { get; private set; } = SOUNDTYPE.BULLET;
    public Vector2 m_SoundSize { get; private set; } = Vector2.zero;
    public float m_LifeTime { get; private set; } = 0.5f;

    // Constructors
    public SoundHotBoxParam(Vector2 _soundPos, bool _isPlayers, SOUNDTYPE _soundType,
        Vector2 _soundSize, float _lifeTime)
    {
        m_SoundPos = _soundPos;
        m_IsPlayers = _isPlayers;
        m_SoundType = _soundType;
        m_SoundSize = _soundSize;
        m_LifeTime = _lifeTime;
    }
}