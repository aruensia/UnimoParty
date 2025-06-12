using UnityEngine;

public class Barricade : MonoBehaviour
{
    [SerializeField] GameObject realBarricadPrefab; //���� ����
    GameObject previewBarricadPrefab; //�̸�����

    //[SerializeField] Transform rightControllerPos; //�÷��̾� 
    Vector3 previewPlayerPos;
    Bounds previewBounds;

    public LayerMask noGroundlayerMask; //�׶��常 ���� üũ

    public bool bool2 = false;

    bool isBlocked; //��ġ�� ������Ʈ���ֳ�? �ݶ��̴��Ǵ¾ֵ��� �ֳ�

    private void OnEnable()
    {
        if (previewBarricadPrefab == null)
        {
            previewBarricadPrefab = Instantiate(realBarricadPrefab);
            Destroy(previewBarricadPrefab.GetComponent<Collider>());
            //SetTransparent(previewBarricadPrefab, 0.3f, Color.black);
            previewBounds = previewBarricadPrefab.GetComponent<MeshRenderer>().bounds;
        }
    }

    private void OnDisable()
    {
        DestotyPreviewPrefab();
    }

    private void Update()
    {
        if (previewBarricadPrefab == null)
        {
            return;
        }

        //�ٴ�üũ ������
        GroundPos();
        previewBounds = previewBarricadPrefab.GetComponent<MeshRenderer>().bounds;
        isBlocked = Physics.CheckBox(previewBounds.center,
                                     previewBounds.extents,
                                     previewBarricadPrefab.transform.rotation,
                                     noGroundlayerMask);
        if (isBlocked == true)
        {
            Debug.Log("��ġ�� ������Ʈ ������");
            SetTransparent(previewBarricadPrefab, 0.3f, Color.red);
        }
        else
        {
            SetTransparent(previewBarricadPrefab, 0.3f, Color.green);
        }



        //������ ����
        if (bool2 == true)
        {
            bool2 = false;

            if (CanPlace() == false)
            {
                Instantiate(realBarricadPrefab, GroundPos(), Quaternion.identity);
                DestotyPreviewPrefab();
            }
            else
            {
                Debug.Log("���ļ� ��ġ �Ұ�!");
            }
        }






    }

    //�̸����� ������ ����
    void DestotyPreviewPrefab()
    {
        if (previewBarricadPrefab != null)
        {
            Destroy(previewBarricadPrefab);
            previewBarricadPrefab = null;
        }

    }



    //�ٴ� ������
    Vector3 GroundPos()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y += previewBounds.extents.y;
        spawnPos += transform.forward * 5f;
        previewBarricadPrefab.transform.position = spawnPos;

        previewPlayerPos = previewBarricadPrefab.transform.position;

        //if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10f))
        //{
        //    if (hit.collider.gameObject.tag == "Ground")
        //    {
        //        Vector3 spawnPos = previewBarricadPrefab.transform.position;
        //        spawnPos.y += previewBounds.extents.y;
        //        spawnPos += transform.forward * 5f;
        //        previewBarricadPrefab.transform.position = spawnPos;
        //    }
        //}
        return previewPlayerPos;
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



    //��ġ ��������
    bool CanPlace()
    {
        if (previewBarricadPrefab == null) { return false; }

        isBlocked = Physics.CheckBox(previewBounds.center,
                                     previewBounds.extents,
                                     previewBarricadPrefab.transform.rotation,
                                     noGroundlayerMask);

        return isBlocked;
    }












}
