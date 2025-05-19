using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaffoldingSpawner : MonoBehaviour
{
    public GameObject cubePrefab;     // �ϳ� ���� ť�� ������
    public Transform centerObject;    // �߾� ���� ������Ʈ
    public int cubeCount = 10;

    void Start()
    {
        for (int i = 0; i < cubeCount; i++)
        {
            // ť�� ����
            GameObject cube = Instantiate(cubePrefab);

            // ���� ��ġ (�߾� ����)
            Vector3 offset = new Vector3(
                Random.Range(-18f, 18f),
                0f,
                Random.Range(-18f, 18f)
            );

            Vector3 spawnPos = centerObject.position + offset;

            // Terrain ���� ����
            Terrain terrain = Terrain.activeTerrain;
            if (terrain != null)
            {
                float terrainY = terrain.SampleHeight(spawnPos) + terrain.GetPosition().y;
                spawnPos.y = terrainY;
            }

            cube.transform.position = spawnPos;

            // ���� ���� ����
            Renderer rend = cube.GetComponent<Renderer>();
            if (rend != null)
                rend.material.color = new Color(Random.value, Random.value, Random.value);
        }
    }
}
