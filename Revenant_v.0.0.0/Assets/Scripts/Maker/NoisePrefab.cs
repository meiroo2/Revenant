using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoiseType
{
    WALK,
    FIREARM
}
public class NoisePrefab : MonoBehaviour
{
    // Visible Member Variables
    public bool m_isPlayer = true;
    public NoiseType m_NoiseType = NoiseType.WALK;
    private BoxCollider2D m_NoiseBoxcollider;

    // Member Variables


    // Constructors
    private void Awake()
    {
        m_NoiseBoxcollider = GetComponent<BoxCollider2D>();
    }

    // Updates


    // Physics


    // Functions
    public void InstantiateNoise(NoiseType _noiseType, Vector2 _noiseSize, Vector2 _noisePos, bool _isPlayer)
    {
        m_NoiseType = _noiseType;
        m_NoiseBoxcollider.size = _noiseSize;
        transform.position = _noisePos;
        m_isPlayer = _isPlayer;
        Invoke(nameof(EndNoise), 0.5f);
    }

    // 기타 분류하고 싶은 것이 있을 경우
    private void EndNoise() {
        m_NoiseBoxcollider.size = Vector2.zero;
        gameObject.SetActive(false); 
    }
}
