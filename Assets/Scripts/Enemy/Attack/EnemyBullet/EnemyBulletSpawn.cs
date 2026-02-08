using Attack.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletSpawn : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private EnemyBulletPooling bulletPool;
    [SerializeField] private float bulletSpeed = 10;

    [SerializeField] private Enemy enemy;
    public void Shoot(AttackValue value)
    {
        if (enemy == null || enemy.Target == null) return;

        Vector2 dir = (enemy.Target.position - (Vector2)firePoint.position).normalized;

        // ÃÑÅº Ç®¸µ ²¨³»±â
        GameObject b = bulletPool.GetBullet(); 
        b.transform.position = firePoint.position;
        
        b.GetComponent<EnemyBullet>().Fire(dir, bulletSpeed, value.damage);
    }
}
