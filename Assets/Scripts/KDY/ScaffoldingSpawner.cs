using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ScaffoldingSpawner : MonoBehaviour
{
    public GameObject[] flowerPrefabs; // ������ 3�� ���� �迭
    //public GameObject cubePrefab;    // ������ ť�� ������
    public Transform centerObject;     // �߽� ������ �Ǵ� ������Ʈ

    private void Awake()
    {
        StartCoroutine(WaitForSceneChange());
    }
    IEnumerator WaitForSceneChange()
    {
        yield return new WaitForSeconds(0.5f);
    }
    void Start()
    {
        int gridSize = 6;                 // ���� ũ�� 6x6
        float area = 80f;                 // ����� �� ���� ũ��
        float spacing = area / gridSize;  // ť�� ���� ����

        Vector3 center = centerObject.position;           // ���� �߽� ��ġ
        
        int prefabIndex = 0;

        for (int x = 0; x < gridSize; x++)                // X ���� ����
        {
            for (int z = 0; z < gridSize; z++)            // Z ���� ����
            {
                // �߾� AUBE �ڸ�(2,2), (2,3), (3,2), (3,3)�� �ǳʶڴ�
                if ((x == 2 || x == 3) && (z == 2 || z == 3))
                    continue;

                // �߽� �������� ��ġ ���
                float offsetX = (x - 2.5f) * spacing;
                float offsetZ = (z - 2.5f) * spacing;
                Vector3 spawnPos = center + new Vector3(offsetX, 0f, offsetZ);

                // ť�� ����
                if (PhotonNetwork.IsMasterClient)
                {
                    //GameObject cube = PhotonNetwork.Instantiate("Flower", spawnPos, Quaternion.identity);

                    GameObject prefab = flowerPrefabs[prefabIndex % flowerPrefabs.Length];
                    GameObject flower = PhotonNetwork.Instantiate(prefab.name, spawnPos, Quaternion.identity);
                    prefabIndex++;
                    Renderer rend = flower.GetComponent<Renderer>();
                    if (rend != null)
                        rend.material.color = new Color(Random.value, Random.value, Random.value);
                }

                // ���� ���� ����
            }
        }
    }
}
