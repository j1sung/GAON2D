using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    EnemyType Type { get; }
}

public enum EnemyType
{
    Normal,
    MidBoss,
    Boss,
}