//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public enum WallType
//{
//    Left,
//    Right,
//    Top,
//    Bottom
//}

//[RequireComponent(typeof(Collider))]
//public class TeleportWall : MonoBehaviour
//{
//    public WallType wallType;
//    public float mapWidth = 38f;
//    public float mapHeight = 38f;
//    public float offset = 0.5f;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (!other.CompareTag("Player")) return;

//        Transform xrOrigin = other.transform.root;
//        CharacterController cc = xrOrigin.GetComponent<CharacterController>();
//        float yOffset = cc != null ? cc.center.y : 1.0f;

//        // ���� ��ġ
//        Vector3 currentPos = xrOrigin.position;
//        Vector3 newPos = currentPos;

//        // �̵� ���� ���
//        Vector3 direction = Vector3.zero;
//        float distance = 0f;

//        switch (wallType)
//        {
//            case WallType.Left:
//                direction = Vector3.right;
//                distance = mapWidth;
//                break;
//            case WallType.Right:
//                direction = Vector3.left;
//                distance = mapWidth;
//                break;
//            case WallType.Top:
//                direction = Vector3.back;
//                distance = mapHeight;
//                break;
//            case WallType.Bottom:
//                direction = Vector3.forward;
//                distance = mapHeight;
//                break;

//        }

//        // ��ġ ��� (���� �Ÿ� + ��������ŭ)
//        newPos = currentPos + direction * (distance + offset);

//        // ������ Terrain ���� ���� (���� �������� �ʰ�)
//        float terrainY = GetTerrainY(newPos);
//        newPos.y = terrainY + yOffset + 0.01f;

//        // �̵� ����
//        if (cc != null)
//        {
//            cc.enabled = false;
//            xrOrigin.position = newPos;
//            cc.enabled = true;
//        }
//        else
//        {
//            xrOrigin.position = newPos;
//        }

//        // ȸ�� ���� (���� �ݴ� ���� ����)
//        Vector3 lookDir = -direction.normalized;
//        lookDir.y = 0f;
//        if (lookDir != Vector3.zero)
//        {
//            xrOrigin.rotation = Quaternion.LookRotation(lookDir);
//        }

//        Debug.Log($"[TeleportWall] {wallType} �� �浹 �� ��ġ: {xrOrigin.position}");
//    }

//    // Terrain �Ǵ� Raycast�� y�� ���� ����
//    private float GetTerrainY(Vector3 pos)
//    {
//        Terrain terrain = Terrain.activeTerrain;
//        if (terrain != null)
//        {
//            return terrain.SampleHeight(pos) + terrain.GetPosition().y;
//        }

//        // Terrain�� ���ٸ� Raycast �õ�
//        if (Physics.Raycast(pos + Vector3.up * 3f, Vector3.down, out RaycastHit hit, 10f))
//        {
//            return hit.point.y;
//        }

//        // ���� fallback
//        return 0f;
//    }
//}

using UnityEngine;
using System.Collections;

public enum WallType
{
    Left,
    Right,
    Top,
    Bottom
}

[RequireComponent(typeof(Collider))]
public class TeleportWall : MonoBehaviour
{
    [Header("������")]
    public WallType wallType;

    [Tooltip("�÷��̾ �����̵��� �ݴ��� ���� Transform")]
    public Transform oppositeWall;

    [Tooltip("�� �������� ���� ���� �Ÿ�")]
    public float offset = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        StartCoroutine(TeleportRoutine(other));
    }

    private IEnumerator TeleportRoutine(Collider other)
    {
        Transform xrOrigin = other.transform.root;
        CharacterController cc = xrOrigin.GetComponent<CharacterController>();
        float yOffset = cc != null ? cc.center.y : 1.0f;

        Vector3 currentPos = xrOrigin.position;
        Vector3 newPos = currentPos;

        if (oppositeWall == null)
        {
            Debug.LogWarning("[TeleportWall] oppositeWall�� �ν����Ϳ� �������� �ʾҽ��ϴ�!");
            yield break;
        }

        // �ݴ��� ���� ��ġ �������� �����̵� (X �Ǵ� Z�� ����)
        switch (wallType)
        {
            case WallType.Left:
                newPos.x = oppositeWall.position.x - offset;
                break;
            case WallType.Right:
                newPos.x = oppositeWall.position.x + offset;
                break;
            case WallType.Top:
                newPos.z = oppositeWall.position.z - offset;
                break;
            case WallType.Bottom:
                newPos.z = oppositeWall.position.z + offset;
                break;
        }

        // Terrain ���� ����
        float terrainY = GetTerrainY(new Vector3(newPos.x, 0, newPos.z));
        newPos.y = terrainY + yOffset + 0.01f;

        // ��ġ �̵�
        if (cc != null)
        {
            cc.enabled = false;
            xrOrigin.position = newPos;
            yield return null;
            cc.enabled = true;
        }
        else
        {
            xrOrigin.position = newPos;
        }

        // ���� ȸ�� (���� ���� �ݴ��)
        Vector3 lookDir = (currentPos - newPos).normalized;
        lookDir.y = 0f;
        if (lookDir != Vector3.zero)
        {
            xrOrigin.rotation = Quaternion.LookRotation(lookDir);
        }

        Debug.Log($"[TeleportWall] {wallType} Ʈ���� �� {xrOrigin.position}");
    }

    private float GetTerrainY(Vector3 pos)
    {
        Terrain terrain = Terrain.activeTerrain;
        return terrain ? terrain.SampleHeight(pos) : pos.y;
    }
}

