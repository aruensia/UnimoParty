using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Barricade : MonoBehaviour, IItemUse, InterfaceMethod.IItemData
{
    [SerializeField] int installMaxDistance; //��ġ �Ÿ�

    [SerializeField] GameObject realBarricadPrefab; //���� ����
    GameObject previewBarricadPrefab; //�̸�����

    Vector3 previewPlayerPos;
    Bounds previewBounds;

    public LayerMask noGroundlayerMask; //�׶��常 ���� üũ

    bool isBlocked; //��ġ�� ������Ʈ���ֳ�? �ݶ��̴��Ǵ¾ֵ��� �ֳ�

    private MeshRenderer previewRenderer;

    //�׷�////////////////////////////////////////////////////////////
    private XRGrabInteractable grabInteractable;

    public bool isGrab = false;
    float rotationY = 0;

    public ItemData ItemData { get; set; }

    private void OnEnable()
    {
        if (previewBarricadPrefab == null)
        {
            previewBarricadPrefab = Instantiate(realBarricadPrefab);
            Destroy(previewBarricadPrefab.GetComponent<Collider>());
            GroundPos();

            previewRenderer = previewBarricadPrefab.GetComponent<MeshRenderer>();

        }
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void OnDisable()
    {
        DestoryPreviewPrefab();
        grabInteractable.selectEntered.RemoveListener(OnGrab);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("�׷���");
        isGrab = true;
       
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
        var tempPos = transform.position + transform.forward * installMaxDistance;

        //if (isGrab == true)
        //{
        //    isGrab = false;
        //    Vector3 pos1 = previewBarricadPrefab.transform.position;
        //    pos1.y += 90;

        //    previewBarricadPrefab.transform.Rotate(pos1);

        //}
        if(isGrab == true)
        {
            isGrab = false;
            rotationY += 90;
            if (rotationY >= 360f)
            {
                rotationY -= 360f;
            }
        }

        if (Physics.Raycast(tempPos, Vector3.down, out RaycastHit hit, 10f))
        {
            if (hit.collider.gameObject.tag == "Ground")
            {
                //������
                Vector3 spawnPos = hit.point;
                spawnPos.y += previewBounds.extents.y;
                var tempSpawnPos = spawnPos.y;

                spawnPos += transform.forward * installMaxDistance;
                previewBarricadPrefab.transform.position = spawnPos;

                //�����̼�
                Vector3 direction = transform.position - previewBarricadPrefab.transform.position;
                direction.y = 0;

                Quaternion rotation = Quaternion.LookRotation(direction);

                rotation *= Quaternion.Euler(0, rotationY, 0);
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

    public bool Use(Transform firePos, int power)
    {
        if (CanPlace() == false)
        {
            Instantiate(realBarricadPrefab, GroundPos(), previewBarricadPrefab.transform.rotation);
            DestoryPreviewPrefab();
            return true;
        }
        else
        {
            Debug.Log("���ļ� ��ġ �Ұ�!");
            return false;
        }

    }
}
