using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CurvedTerrainGenerator : MonoBehaviour
{
    [Header("���� ����")]
    public int resolution = 50;          // ����/���� ����ȭ
    public float radius = 30f;           // ���� ������ (ũ��)
    public float angle = 180f;            // ��� ���� (��: 90���� 1/4��)

    void Start()
    {
        GenerateCurvedMesh();
    }

    void GenerateCurvedMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[(resolution + 1) * (resolution + 1)];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangles = new int[resolution * resolution * 6];

        int vertIndex = 0;
        int triIndex = 0;

        for (int y = 0; y <= resolution; y++)
        {
            float v = (float)y / resolution;
            float thetaV = Mathf.Lerp(0, Mathf.Deg2Rad * angle, v);

            for (int x = 0; x <= resolution; x++)
            {
                float u = (float)x / resolution;
                float thetaU = Mathf.Lerp(-Mathf.Deg2Rad * angle / 2, Mathf.Deg2Rad * angle / 2, u);

                float xPos = Mathf.Sin(thetaU) * Mathf.Cos(thetaV) * radius;
                float yPos = Mathf.Sin(thetaV) * radius;
                float zPos = Mathf.Cos(thetaU) * Mathf.Cos(thetaV) * radius;

                vertices[vertIndex] = new Vector3(xPos, yPos, zPos);
                uvs[vertIndex] = new Vector2(u, v);

                if (x < resolution && y < resolution)
                {
                    int a = vertIndex;
                    int b = vertIndex + resolution + 1;
                    int c = vertIndex + 1;
                    int d = vertIndex + resolution + 2;

                    triangles[triIndex++] = a;
                    triangles[triIndex++] = b;
                    triangles[triIndex++] = c;

                    triangles[triIndex++] = c;
                    triangles[triIndex++] = b;
                    triangles[triIndex++] = d;
                }

                vertIndex++;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}