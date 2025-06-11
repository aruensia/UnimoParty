using UnityEngine;

public class TEST : MonoBehaviour
{
    public GameObject boundingBoxPrefab; // ������ ť�� ������
    private GameObject boxInstance;

    void Start()
    {
        Collider col = GetComponent<Collider>();
        if (col != null && boundingBoxPrefab != null)
        {
            Bounds b = col.bounds;

            boxInstance = Instantiate(boundingBoxPrefab, b.center, Quaternion.identity);
            boxInstance.transform.localScale = b.size;

        }
    }



}









