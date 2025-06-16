using UnityEngine;

public class Barricade : MonoBehaviour, IItemUse
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
        DestoryPreviewPrefab();
    }



    private void FixedUpdate()
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


    }

    //�̸����� ������ ����
    void DestoryPreviewPrefab()
    {
        if (previewBarricadPrefab != null)
        {
            Destroy(previewBarricadPrefab);
            Destroy(gameObject);
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



                //������
                Vector3 spawnPos = hit.point;
                spawnPos.y += previewBounds.extents.y;
                var tempSpawnPos = spawnPos.y;

                spawnPos += transform.forward * 5f;
                previewBarricadPrefab.transform.position = spawnPos;

                //�����̼�
                Vector3 direction = transform.position - previewBarricadPrefab.transform.position;
                direction.y = 0;

                Quaternion rotation = Quaternion.LookRotation(direction);

                previewBarricadPrefab.transform.rotation = rotation;

                previewBarricadPrefab.transform.position = new Vector3(spawnPos.x, tempSpawnPos, spawnPos.z);



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

    public void Use(Transform firePos, int power)
    {

        if (CanPlace() == false)
        {
            Instantiate(realBarricadPrefab, GroundPos(), previewBarricadPrefab.transform.rotation);
            DestoryPreviewPrefab();
        }
        else
        {
            Debug.Log("���ļ� ��ġ �Ұ�!");
        }



    }
}
