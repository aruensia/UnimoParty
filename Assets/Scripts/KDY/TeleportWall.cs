using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TeleportWall: �÷��̾ ���� �ε����� �ݴ������� �̵���Ű�� ��ũ��Ʈ
// �� �������� Left, Right, Top, Bottom �����Ͽ� ����
// mapWidth�� mapHeight�� �� ��ü ũ�� (Plane ������ x 10 ����)�� ����

public enum WallType
{
    Left,
    Right,
    Top,
    Bottom
}

public class TeleportWall : MonoBehaviour
{
    [Header("���� ���� (Left / Right / Top / Bottom)")]
    public WallType wallType;

    [Header("�� ���� ���� (��ü X�� ũ��, ��: 400)")]
    public float mapWidth = 400f;

    [Header("�� ���� ���� (��ü Z�� ũ��, ��: 400)")]
    public float mapHeight = 400f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Vector3 currentPos = other.transform.position;
        Vector3 newPos = currentPos;

        switch (wallType)
        {
            case WallType.Left:
                // ���� �� �� ���������� �̵� (X �ݴ���)
                newPos = new Vector3(currentPos.x + mapWidth, currentPos.y, currentPos.z);
                break;
            case WallType.Right:
                // ������ �� �� �������� �̵�
                newPos = new Vector3(currentPos.x - mapWidth, currentPos.y, currentPos.z);
                break;
            case WallType.Top:
                // ���� �� �� �Ʒ������� �̵� (Z �ݴ���)
                newPos = new Vector3(currentPos.x, currentPos.y, currentPos.z - mapHeight);
                break;
            case WallType.Bottom:
                // �Ʒ��� �� �� �������� �̵�
                newPos = new Vector3(currentPos.x, currentPos.y, currentPos.z + mapHeight);
                break;
        }

        other.transform.position = newPos;

        // �������� ȸ���� �����ϰų� Y�ุ �����ϴ� ����� ���������,
        // �̹����� �׻� �ʱ�ȭ(0,0,0 ȸ��)�ϵ��� �����Ͽ� �׽�Ʈ��.
        other.transform.rotation = Quaternion.identity;

        Debug.Log($"{wallType} �� �浹: {currentPos} �� {newPos} �̵� �Ϸ� (ȸ�� �ʱ�ȭ��)");
    }
}

