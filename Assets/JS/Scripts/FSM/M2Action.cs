using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M2Action : MonoBehaviour, IEnemyAction
{
    public void Attack(Enemy self, float damage)
    {
        Debug.Log("M2 박치기 공격!");
    }
}
