using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1Action : MonoBehaviour, IEnemyAction
{
    public void Attack(Enemy self)
    {
        Debug.Log("M1 박치기 공격!");
    }
}