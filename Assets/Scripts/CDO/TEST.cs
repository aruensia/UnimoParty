using UnityEngine;



//bounds
//bounds.center �������� ���߾�
//bounds.extents half�������� 2����1��ũ�� xyz (������)
public class TEST : MonoBehaviour
{
    public GameObject cubePrefab;
    private GameObject previewCube;

    public bool bool1 = false;
    public bool bool2 = false;

    public LayerMask layerMask;

    void Update()
    {
        //������ �̸�����
        if (bool1 == true)
        {
            if (previewCube == null)
            {
                previewCube = Instantiate(cubePrefab);
                Destroy(previewCube.GetComponent<Collider>());
                SetTransparent(previewCube, 0.3f, Color.black);
            }

            Vector3 Player = transform.position + transform.forward * 2f;

            if (Physics.Raycast(Player, Vector3.down, out RaycastHit hit, 10f))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    Bounds bounds = previewCube.GetComponent<MeshRenderer>().bounds;
                    Vector3 spawnPos = hit.point;
                    spawnPos.y += bounds.extents.y;
                    previewCube.transform.position = spawnPos;
                }
            }

            int groundLayer = LayerMask.GetMask("Water"); 
            int mask = ~groundLayer;//�׶��常 ���� ��Ʈ����
            // ���߿� ���̾��ũ �ν�����â���� �����ҵ� public LayerMask layerMask; 
            Bounds previewBounds = previewCube.GetComponent<Renderer>().bounds;

            bool isBlocked = Physics.CheckBox (previewBounds.center,
                                               previewBounds.extents,
                                               previewCube.transform.rotation,
                                               mask);

            if (isBlocked == true)
            {
                Debug.Log("��ġ�� ������Ʈ ����!");
                SetTransparent(previewCube, 0.3f, Color.red);
            }
            else
            {
                SetTransparent(previewCube, 0.3f, Color.green);
            }

        }
        else if (bool1 == false)
        {
            //���� ������ ����
            Destroy(previewCube);

        }



        //������ ����
        if (bool2 == true)
        {
            bool2 = false;

            if (CanPlace() == false)
            {
                Instantiate(cubePrefab, GroundPos(), Quaternion.identity);
            }
            else
            {
                Debug.Log("���ļ� ��ġ �Ұ�!");
            }
        }



    }

    //��ġ ��������
    bool CanPlace()
    {
        if (previewCube == null) { return false; }

        Bounds previewBounds = previewCube.GetComponent<Renderer>().bounds;

        bool isBlocked = Physics.CheckBox(previewBounds.center, previewBounds.extents, previewCube.transform.rotation);

        return isBlocked;
    }

    //�ٴ� ������
    Vector3 GroundPos()
    {
        Vector3 Player = transform.position + transform.forward * 2f;

        if (Physics.Raycast(Player, Vector3.down, out RaycastHit hit, 10f))
        {
            if (hit.collider.gameObject.tag == "Ground")
            {
                Bounds bounds = previewCube.GetComponent<MeshRenderer>().bounds;
                Vector3 spawnPos = hit.point;
                spawnPos.y += bounds.extents.y;
                return spawnPos;
            }
        }

        return Player;
    }


    //��Ƽ���� ����ȯ
    void SetTransparent(GameObject obj, float alpha, Color color)
    {
        var renderer = obj.GetComponent<MeshRenderer>();

        var mat = renderer.materials[0];

        color.a = alpha;
        mat.color = color;
        renderer.material = mat;
    }



}









