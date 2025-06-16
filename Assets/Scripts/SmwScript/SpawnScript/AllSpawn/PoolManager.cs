using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

[System.Serializable]
public class PoolItem
{
    [Tooltip("Ǯ���� ������ ������")]
    public GameObject prefab;
    [Tooltip("Ǯ �ʱ� �뷮")]
    public int defaultCapacity = 10;
    [Tooltip("Ǯ �ִ� ũ��")]
    public int maxSize = 100;
}

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [Header("���⿡ Ǯ���� ���� �����յ��� �߰��ϼ���")]
    [SerializeField] private List<PoolItem> poolItems;


    private Dictionary<GameObject, ObjectPool<GameObject>> pools;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        pools = new Dictionary<GameObject, ObjectPool<GameObject>>();
        foreach (var item in poolItems)
        {
            var prefab = item.prefab;
            var pool = new ObjectPool<GameObject>(

                createFunc: () =>
                {
                    var go = Instantiate(prefab);
                    go.SetActive(false);
                    return go;
                },
                actionOnRelease: go =>
                {
                    go.SetActive(false);
                },

                actionOnDestroy: go =>
                {
                    Destroy(go);
                },
                collectionCheck: false,
                defaultCapacity: item.defaultCapacity,
                maxSize: item.maxSize
            );

            pools.Add(prefab, pool);
        }
    }

    public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (!pools.TryGetValue(prefab, out var pool))
            return Instantiate(prefab, pos, rot);

        // (1) Get�� ��Ȱ�� ���·� ��ȯ
        var go = pool.Get();

            // (2) ��ġ/ȸ�� ����
             go.transform.SetPositionAndRotation(pos, rot);

            // (3) ���⼭ Ȱ��ȭ �� OnEnable()�� ����� pos�� ����
             go.SetActive(true);

        var bd = go.GetComponent<Burnduri>();
        if (bd != null)
            bd.prefab = prefab;

        return go;
    }

    public void Despawn(GameObject prefab, GameObject instance)
    {
        if (pools.TryGetValue(prefab, out var pool))
        {
            Debug.Log("Ǯ�� ���۵��ѵ�?");
            Debug.Log(prefab);
            pool.Release(instance);
        }
        else
        {
            Debug.Log("�ı�!");
            Debug.Log(prefab);
            Destroy(instance);
        }
    }
}
