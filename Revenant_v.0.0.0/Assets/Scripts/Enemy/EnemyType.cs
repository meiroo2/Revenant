using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STAGE
{
    NONE,
    TUTORIAL
}

public enum TUTORIAL
{
    NONE,
    TARGETBOARD,
    DRONE,
    TURRET
}

[System.Serializable]
public class EnemyType
{
    public STAGE m_stage;
    public TUTORIAL m_tutorialEnemyType;
}

