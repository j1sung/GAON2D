using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EIEnemy
{
    public interface IEnemy
    {
        EnemyType Type { get; }
    }
}


public enum EnemyType
{
    Normal,
    MidBoss,
    Boss,
}