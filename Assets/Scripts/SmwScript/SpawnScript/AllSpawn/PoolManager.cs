using Photon.Pun;
using UnityEngine;

[System.Serializable]
public class PoolItem
{
    [Tooltip("Ǯ���� ������ ������)")]
    public string prefabName;
    [Tooltip("Ǯ �ʱ� �뷮")]
    public int defaultCapacity = 10;
    [Tooltip("Ǯ �ִ� ũ��")]
    public int maxSize = 50;
}

//public class PoolManager : MonoBehaviourPun
//{
//    public static PoolManager Instance { get; private set; }

//    [SerializeField]
//    private List<PoolItem> poolItems;

//    private Dictionary<string, ObjectPool<GameObject>> pools;
//    private Dictionary<GameObject, ObjectPool<GameObject>> instanceToPool;

//    private void Awake()
//    {
//        if (Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }
//        Instance = this;

//        pools = new Dictionary<string, ObjectPool<GameObject>>();
//        instanceToPool = new Dictionary<GameObject, ObjectPool<GameObject>>();

//        foreach (var item in poolItems)
//        {
//            string key = item.prefabName;
//            var pool = new ObjectPool<GameObject>(
//                createFunc: () =>
//                {
//                    var go = PhotonNetwork.Instantiate(key, Vector3.zero, Quaternion.identity);
//                    go.SetActive(false);
//                    return go;
//                },
//                actionOnGet: go => { },
//                actionOnRelease: go => go.SetActive(false),
//                actionOnDestroy: go => PhotonNetwork.Destroy(go),
//                collectionCheck: false,
//                defaultCapacity: item.defaultCapacity,
//                maxSize: item.maxSize
//            );
//            pools.Add(key, pool);
//        }
//    }


//    public GameObject Spawn(string prefabName, Vector3 position, Quaternion rotation)
//    {
//        if (!pools.TryGetValue(prefabName, out var pool))
//        {
//            return PhotonNetwork.Instantiate(prefabName, position, rotation);
//        }

//        var go = pool.Get();
//        instanceToPool[go] = pool;

//        go.transform.SetPositionAndRotation(position, rotation);
//        go.SetActive(true);
//        return go;
//    }


//    public void Despawn(GameObject instance)
//    {
//        if (instanceToPool.TryGetValue(instance, out var pool))
//        {
//            pool.Release(instance);
//            instanceToPool.Remove(instance);
//        }
//        else
//        {
//            PhotonNetwork.Destroy(instance);
//        }
//    }
//}
public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// ��Ʈ��ũ ������Ʈ ���� (RoomObject�̹Ƿ� �� �� ��ü�� ������)
    /// </summary>
    public GameObject SpawnNetworked(string prefabName, Vector3 position, Quaternion rotation)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return null;
        }

        return PhotonNetwork.InstantiateRoomObject(prefabName, position, rotation);
    }

    public void DespawnNetworked(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }

        PhotonView pv = obj.GetComponent<PhotonView>();

        if (pv == null)
        {
            Destroy(obj);
            return;
        }

        if (pv.IsMine || PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(obj);
        }
        else
        {
            Debug.LogWarning($" Destroy �õ� ����: ViewID {pv.ViewID}, Owner: {pv.Owner}, Local: {PhotonNetwork.LocalPlayer}");
        }
    }
}