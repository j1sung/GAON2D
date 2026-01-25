using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyBrain
{
    void Init(Enemy enemy);
    void OnEnableBrain();
    void Tick();
    void FixedTick();
}
