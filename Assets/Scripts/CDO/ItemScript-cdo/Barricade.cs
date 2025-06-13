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

    private MeshRenderer previewRenderer;
    private void OnEnable()
    {
        if (previewBarricadPrefab == null)
        {
            previewBarricadPrefab = Instantiate(realBarricadPrefab);
            Destroy(previewBarricadPrefab.GetComponent<Collider>());
            GroundPos();

            previewRenderer = previewBarricadPrefab.GetComponent<MeshRenderer>();

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

        //�ٴ�üũ �� ������
        GroundPos();

        previewBounds = previewRenderer.bounds;

        isBlocked = Physics.CheckBox(previewBounds.center,
                                     previewBounds.extents,
                                     previewBarricadPrefab.transform.rotation,
                                     noGroundlayerMask);
        if (isBlocked == true)
        {
            Debug.Log("��ġ�� ������Ʈ ������");
            SetTransparent(0.3f, Color.red);
        }
        else
        {
            SetTransparent(0.3f, Color.green);
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
        var tempPos = transform.position + transform.forward * 5f;

        if (Physics.Raycast(tempPos, Vector3.down, out RaycastHit hit, 10f))
        {
            if (hit.collider.gameObject.tag == "Ground")
            {

                //�����̼�
                Vector3 direction = transform.position - previewBarricadPrefab.transform.position;
                direction.y = 0f; 
                Quaternion rotation = Quaternion.LookRotation(direction);

                //������
                Vector3 spawnPos = hit.point;
                spawnPos.y += previewBounds.extents.y;
                spawnPos += transform.forward * 5f;
                previewBarricadPrefab.transform.position = spawnPos;

                previewBarricadPrefab.transform.rotation = rotation;

                previewPlayerPos = previewBarricadPrefab.transform.position;

            }
        }


        return previewPlayerPos;
    }

    //��Ƽ���� ����ȯ
    void SetTransparent(float alpha, Color color)
    {
        if (previewRenderer == null)
        {
            return;
        }

        var mat = previewRenderer.materials[0];

        color.a = alpha;
        mat.color = color;
        previewRenderer.material = mat;
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
