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

        // �浹 �ݺ� ������ ���� ��Ÿ�� ����
        lastTeleportTime = Time.time;

        // ����� ���
        Debug.Log($"{wallType} �� �浹: {currentPos} �� {newPos} �̵� �Ϸ�");
    }
}



