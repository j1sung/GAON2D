using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPooling : MonoBehaviour
{
    private enum PoolType
    {
        Bullet,
        VFX
    }

    int Bullet;
    int VFX;

    // bullet 프리펩 선택
    [SerializeField] GameObject bullet;

    // effect 프리펩 선택
    [SerializeField] GameObject vfx;

    private GameObject[] prefabs;

    // 1. 비활성 넣는 스택
    private Stack<GameObject>[] pool; // 0: bullet / 1: vfx

    // 2. 반환 선택용 딕셔너리
    private Dictionary<GameObject, int> reverse;

    // 3. 풀링 오브젝트 계층 위치 그룹
    private Transform[] poolGroups;



    private void Awake()
    {
        Bullet = Idx(PoolType.Bullet);
        VFX = Idx(PoolType.VFX);

        prefabs = new GameObject[2];

        prefabs[Bullet] = bullet;
        prefabs[VFX] = vfx;

        int n = prefabs.Length;
        pool = new Stack<GameObject>[n];
        reverse = new Dictionary<GameObject, int>();
        poolGroups = new Transform[n];

        for(int i = 0; i <n; ++i)
        {
            // 스택 배열 인덱스마다 종류별로 최대 풀 갯수 공간 만들기.
            pool[i] = new Stack<GameObject>();

            // 프리펩 이름으로 그룹 오브젝트 생성.
            GameObject group = new GameObject(prefabs[i].name + "Pool");

            // 생성한 그룹 오브젝트를 BulletPool 하위로 계층 이동.
            group.transform.SetParent(transform, false);

            // 그룹 배열에 만든 그룹 오브젝트 인덱스마다 대입
            poolGroups[i] = group.transform;
        }
    }

    private int Idx(PoolType t) => (int)t;

    public GameObject GetBullet()
    {

        GameObject select = null;

        if (pool[Bullet].Count > 0)
        {
            select = pool[Bullet].Pop();
        }
        else
        {
            select = Instantiate(prefabs[Bullet], poolGroups[Bullet]);
            reverse[select] = Bullet;
        }

        // 생성과 동시에 풀링 소유권 주입
        EnemyBullet bullet = select.GetComponent<EnemyBullet>();
        if(bullet != null) bullet.SetOwnerPool(this);
        
        select.SetActive(true);

        return select;
    }

    public GameObject GetVFX()
    {

        GameObject select = null;

        if (pool[VFX].Count > 0)
        {
            select = pool[VFX].Pop();
        }
        else
        {
            select = Instantiate(prefabs[VFX], poolGroups[VFX]);
            reverse[select] = VFX;
        }

        // 생성과 동시에 풀링 소유권 주입
        EnemyBulletVFX vfx = select.GetComponent<EnemyBulletVFX>();
        if (vfx != null) vfx.SetOwnerPool(this);

        select.SetActive(true);

        return select;
    }

    public void Release(GameObject gameObject)
    {
        if (gameObject == null) return;

        if(!reverse.TryGetValue(gameObject, out int idx))
        {
            Destroy(gameObject);
            return;
        }

        gameObject.SetActive(false);
        pool[idx].Push(gameObject);
    }
}
