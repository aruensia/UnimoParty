//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//// TeleportWall: �÷��̾ ���� ����� �� �ݴ������� �����̵���Ű�� ��ũ��Ʈ
//// ���� ����(WallType)�� ���� �̵� ������ ������
//// �� ũ��(mapWidth, mapHeight)�� �÷��̾ �󸶳� �ָ� �����̵����� ������
//// XR Origin ��� �÷��̾�� �����ϸ�, Terrain �� ������ ���� Raycast ó�� ����

//public enum WallType
//{
//    Left,    // ���� ��
//    Right,   // ������ ��
//    Top,     // ���� ��
//    Bottom   // �Ʒ��� ��
//}

//public class TeleportWall : MonoBehaviour
//{
//    [Header("���� ���� (Left / Right / Top / Bottom)")]
//    public WallType wallType; // ���� ���� ���� ����

//    [Header("�� ���� ���� (X�� ����)")]
//    public float mapWidth = 38f; // X�� ���� �����̵� �Ÿ�

//    [Header("�� ���� ���� (Z�� ����)")]
//    public float mapHeight = 38f; // Z�� ���� �����̵� �Ÿ�

//    private float teleportCooldown = 0.5f; // ��Ÿ�� ���� (��)
//    private float lastTeleportTime = -999f; // ������ �����̵� �ð� ����

//    private void OnTriggerEnter(Collider other)
//    {
//        // "Player" �±װ� �ƴ� ��� ����
//        if (!other.CompareTag("Player")) return;

//        // ��Ÿ�� �� �ߺ� ���� ����
//        if (Time.time - lastTeleportTime < teleportCooldown) return;

//        // �÷��̾� ������Ʈ�� �ֻ��� ��Ʈ (XR Origin ��ü) �������� �̵� ó��
//        Transform xrOrigin = other.transform.root;

//        // ���� ��ġ ����
//        Vector3 currentPos = xrOrigin.position;
//        Vector3 newPos = currentPos;

//        // ���� ���⿡ ���� �ݴ��� ��ġ ���
//        switch (wallType)
//        {
//            case WallType.Left:
//                newPos = new Vector3(currentPos.x + mapWidth, currentPos.y, currentPos.z);
//                break;
//            case WallType.Right:
//                newPos = new Vector3(currentPos.x - mapWidth, currentPos.y, currentPos.z);
//                break;
//            case WallType.Top:
//                newPos = new Vector3(currentPos.x, currentPos.y, currentPos.z - mapHeight);
//                break;
//            case WallType.Bottom:
//                newPos = new Vector3(currentPos.x, currentPos.y, currentPos.z + mapHeight);
//                break;
//        }

//        // ������ �Ʒ��� Ray�� ���� Terrain �� Y��ǥ Ȯ��
//        Vector3 rayStart = newPos + Vector3.up * 10f;
//        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 50f))
//        {
//            // ���� ���� ��Ȯ�� ����
//            newPos.y = hit.point.y;
//        }

//        // XR Origin ��ġ �̵�
//        //xrOrigin.position = newPos;

//        // CharacterController ���� ��� �浹 ������ ���� �Ͻ������� ��
//        CharacterController cc = xrOrigin.GetComponent<CharacterController>();
//        if (cc != null)
//            cc.enabled = false;

//        // ��ġ �̵�
//        xrOrigin.position = newPos;

//        // �ٽ� �ѱ�
//        if (cc != null)
//            cc.enabled = true;


//        // ȸ�� �ʱ�ȭ (X/Z ȸ�� ���� ����)
//        xrOrigin.rotation = Quaternion.identity;

//        // ��Ÿ�� �ð� ����
//        lastTeleportTime = Time.time;

//        // ����� ���
//        Debug.Log($"{wallType} �� �浹: {currentPos} �� {newPos} �̵� �Ϸ� (XR Origin ȸ�� �ʱ�ȭ��)");

//    }
//}

using System.Collections;
using UnityEngine;

// �� ���� ������: ��� ���⿡�� �����̵������� ����
public enum WallType
{
    Left,
    Right,
    Top,
    Bottom
}

// �� ��ũ��Ʈ�� "Trigger ��"�� �پ�, �÷��̾ ������ �����̵���Ű�� ����
[RequireComponent(typeof(Collider))]
public class TeleportWall : MonoBehaviour
{
    [Header("�̵� ��� ��ġ (�ʼ�)")]
    public Transform teleportTarget; // �̵��� ��ġ (���� ť�� ��ġ�� �Ҵ�)

    [Header("���� ���� (Left / Right / Top / Bottom)")]
    public WallType wallType;

    [Header("�� ���� ���� (X�� ����)")]
    public float mapWidth = 38f;

    [Header("�� ���� ���� (Z�� ����)")]
    public float mapHeight = 38f;

    // �����̵� ��Ÿ�� ����
    private float teleportCooldown = 1f;
    private float lastTeleportTime = -999f;

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾ �ƴ� ��� ����
        if (!other.CompareTag("Player")) return;

        // ��Ÿ�� ���� ����
        if (Time.time - lastTeleportTime < teleportCooldown) return;

        // XR Origin �������� ó�� (�ֻ��� Transform)
        Transform xrOrigin = other.transform.root;
        Vector3 currentPos = xrOrigin.position;
        Vector3 newPos = currentPos;

        // ���� ���� �������� �̵� ���� ���� (Trigger ���� "����" ����)
        Vector3 direction = Vector3.zero;
        float distance = 0f;

        switch (wallType)
        {
            case WallType.Left:
                direction = Vector3.right;
                distance = mapWidth;
                break;
            case WallType.Right:
                direction = Vector3.left;
                distance = mapWidth;
                break;
            case WallType.Top:
                direction = Vector3.back;
                distance = mapHeight;
                break;
            case WallType.Bottom:
                direction = Vector3.forward;
                distance = mapHeight;
                break;
        }

        // ���� * �Ÿ���ŭ �̵�
        newPos = currentPos + direction * distance;

        // CharacterController offset
        CharacterController cc = xrOrigin.GetComponent<CharacterController>();
        float yOffset = cc != null ? cc.center.y : 0f;

        //  ���⼭ X���� �� Y�� ������ ����
        if (wallType == WallType.Left || wallType == WallType.Right)
        {
            newPos.y = 2f; // �װ� �׽�Ʈ�ؼ� �´� Y������ �־�
        }
        else
        {
            Terrain terrain = Terrain.activeTerrain;
            if (terrain != null)
            {
                float terrainY = terrain.SampleHeight(newPos) + terrain.GetPosition().y;
                newPos.y = terrainY + yOffset + 1f;
            }
        }

        // ��ġ ����
        if (cc != null)
        {
            cc.enabled = true;
            Vector3 move = newPos - xrOrigin.position;
            cc.Move(move);                // �ڿ������� �̵�
            cc.SimpleMove(Vector3.zero); // �߷� ���� ����
        }
        else
        {
            xrOrigin.position = newPos;
        }

        // ȸ�� �ʱ�ȭ (�ʿ�� ���� ����)
        //xrOrigin.rotation = Quaternion.identity;

        // �浹 �ݺ� ������ ���� ��Ÿ�� ����
        lastTeleportTime = Time.time;

        // ���������� Trigger �浹 ���α�
       // StartCoroutine(TemporarilyDisableCollider(other));

        // ����� ���
        Debug.Log($"{wallType} �� �浹: {currentPos} �� {newPos} �̵� �Ϸ�");
    }

    //// �浹 �ݺ� ������: ��� Collider ��Ȱ��ȭ
    //private IEnumerator TemporarilyDisableCollider(Collider col)
    //{
    //    col.enabled = false;
    //    yield return new WaitForSeconds(teleportCooldown);
    //    col.enabled = true;
    //}
    
}



