using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    public GameObject bulletPrefab;
    public int poolSize = 30;

    private List<GameObject> pool = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetBullet()
    {
        foreach (var bullet in pool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        // 풀 부족 시 확장 
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(false);
        pool.Add(newBullet);
        return newBullet;
    }
}