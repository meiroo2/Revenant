using UnityEngine;


/// <summary>
/// Sound의 종류를 나타냅니다.
/// </summary>
public enum SOUNDTYPE
{
    BULLET,
    EXPLOSION
}

/// <summary>
/// SoundHotBox에 전달할 파라미터입니다. Sound의 정보를 나타냅니다.
/// </summary>
public class SoundHotBoxParam
{
    // Member Variables
    public bool m_IsPlayers { get; private set; } = false;
    public Vector2 m_SoundOriginPos { get; private set; } = Vector2.zero;
    public SOUNDTYPE m_SoundType { get; private set; } = SOUNDTYPE.BULLET;
    
    // Constructors
    public SoundHotBoxParam(bool _isPlayers, Vector2 _soundPos, SOUNDTYPE _soundType)
    {
        m_IsPlayers = _isPlayers;
        m_SoundOriginPos = _soundPos;
        m_SoundType = _soundType;
    }
}