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
                Random.Range(-120f, 120f),
                0f,
                Random.Range(-120f, 120f)
            );
            cube.transform.position = centerObject.position + offset;

            // ���� ���� ����
            Renderer rend = cube.GetComponent<Renderer>();
            if (rend != null)
                rend.material.color = new Color(Random.value, Random.value, Random.value);
        }
    }
}
