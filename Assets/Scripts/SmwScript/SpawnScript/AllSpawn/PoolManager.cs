// PoolManager.cs
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

    // ������ �� ObjectPool ����
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
            var prefab = item.prefab; // ���� ����
            var pool = new ObjectPool<GameObject>(
                // ���� ����
                createFunc: () =>
                {
                    var go = Instantiate(prefab);
                    go.SetActive(false);
                    return go;
                },
                // Get() ��
                actionOnGet: go =>
                {
                    go.SetActive(true);
                },
                // Release() ��
                actionOnRelease: go =>
                {
                    go.SetActive(false);
                },
                // Ǯ �ʰ� �� �ı�
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

    /// <summary>
    /// ���ϴ� ���������κ��� Ǯ���� ���� �����մϴ�.
    /// </summary>
    public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (!pools.TryGetValue(prefab, out var pool))
        {
            Debug.LogWarning($"PoolManager: Ǯ�� ��ϵ��� ���� prefab({prefab.name}) �Դϴ�. Instantiate ���.");
            return Instantiate(prefab, pos, rot);
        }

        var go = pool.Get();
        go.transform.SetPositionAndRotation(pos, rot);


        var bd = go.GetComponent<Burnduri>();
        if (bd != null)
            bd.prefab = prefab;


        return go;
    }

    /// <summary>
    /// ������ ������Ʈ�� �ٽ� Ǯ�� ��ȯ�մϴ�.
    /// </summary>
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
