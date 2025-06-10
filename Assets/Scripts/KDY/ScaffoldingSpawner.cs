//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ScaffoldingSpawner : MonoBehaviour
//{
//    public GameObject cubePrefab;     // �ϳ� ���� ť�� ������
//    public Transform centerObject;    // �߾� ���� ������Ʈ
//    public int cubeCount = 10;

//    void Start()
//    {
//        for (int i = 0; i < cubeCount; i++)
//        {
//            // ť�� ����
//            GameObject cube = Instantiate(cubePrefab);

//            // ���� ��ġ (�߾� ����)
//            Vector3 offset = new Vector3(
//                Random.Range(-18f, 18f),
//                0f,
//                Random.Range(-18f, 18f)
//            );

//            Vector3 spawnPos = centerObject.position + offset;

//            // Terrain ���� ����
//            Terrain terrain = Terrain.activeTerrain;
//            if (terrain != null)
//            {
//                float terrainY = terrain.SampleHeight(spawnPos) + terrain.GetPosition().y;
//                spawnPos.y = terrainY;
//            }

//            cube.transform.position = spawnPos;

//            // ���� ���� ����
//            Renderer rend = cube.GetComponent<Renderer>();
//            if (rend != null)
//                rend.material.color = new Color(Random.value, Random.value, Random.value);
//        }
//    }
//}

using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ScaffoldingSpawner : MonoBehaviour
{
    public GameObject cubePrefab;         // ������ ť�� ������
    public Transform centerObject;        // �߽� ������ �Ǵ� ������Ʈ

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
        int gridSize = 6;                 // ���� ũ��: 6x6
        float area = 80f;                 // ����� �� ���� ũ�� (�ͷ����� 100x100������ 80x80 �������� ����)
        float spacing = area / gridSize;  // �� ť�� ���� ����

        Vector3 center = centerObject.position;           // ���� �߽� ��ġ
        //Terrain terrain = Terrain.activeTerrain;          // Terrain ����

        for (int x = 0; x < gridSize; x++)                // X ���� ����
        {
            for (int z = 0; z < gridSize; z++)            // Z ���� ����
            {
                // �߾� AUBE �ڸ�(2,2), (2,3), (3,2), (3,3)�� �ǳʶڴ�
                if ((x == 2 || x == 3) && (z == 2 || z == 3))
                    continue;

                // �߽� �������� ��ġ ������ ���
                float offsetX = (x - 2.5f) * spacing;
                float offsetZ = (z - 2.5f) * spacing;
                Vector3 spawnPos = center + new Vector3(offsetX, 0f, offsetZ);

                // Terrain�� �ִٸ� �ش� ��ġ�� ���� ����
                //if (terrain != null)
                //{
                //   float terrainY = terrain.SampleHeight(spawnPos) + terrain.GetPosition().y;
                //  spawnPos.y = terrainY;
                //}

                // ť�� ����
                if (PhotonNetwork.IsMasterClient)
                {
                    GameObject cube = PhotonNetwork.Instantiate("Flower", spawnPos, Quaternion.identity);
                    Renderer rend = cube.GetComponent<Renderer>();
                    if (rend != null)
                        rend.material.color = new Color(Random.value, Random.value, Random.value);
                }

                // ���� ���� ����
            }
        }
    }
}
