//using System.Collections;
//using UnityEngine;

//// �� ���� ����
//public enum WallType
//{
//    Left,
//    Right,
//    Top,
//    Bottom
//}

//// �� ��ũ��Ʈ�� �÷��̾ ���� ����� ��,
//// �� �ݴ������� �����̵���Ű�� ������ �Ѵ�.
//[RequireComponent(typeof(Collider))]
//public class TeleportWall : MonoBehaviour
//{
//    [Header("���� ���� (Left / Right / Top / Bottom)")]
//    public WallType wallType;

//    [Header("�� ���� ���� (X�� ����)")]
//    public float mapWidth = 38f;

//    [Header("�� ���� ���� (Z�� ����)")]
//    public float mapHeight = 38f;

//    private void OnTriggerEnter(Collider other)
//    {
//        // Player �±װ� �ƴϸ� ����
//        if (!other.CompareTag("Player")) return;

//        // XR Origin = ī�޶� ������ �ֻ��� ��ü
//        Transform xrOrigin = other.transform.root;
//        Vector3 currentPos = xrOrigin.position;
//        Vector3 newPos = currentPos;

//        // �̵� ���� �� �Ÿ� ����
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

//        // ������ Ȯ���� ������������ ����� �ָ� ���� (�Ÿ� + 2.5f �̻�)
//        newPos = currentPos + direction * (distance + 0.5f);

//        // ĳ���� ��Ʈ�ѷ��� �߽� ��ġ(Y�� ������)
//        CharacterController cc = xrOrigin.GetComponent<CharacterController>();
//        float yOffset = cc != null ? cc.center.y : 1.0f;

//        // ������ Terrain�� ���: �ش� ��ġ�� ���̸� ���ø�
//        Terrain terrain = Terrain.activeTerrain;
//        if (terrain != null)
//        {
//            float terrainY = terrain.SampleHeight(newPos) + terrain.GetPosition().y;
//            newPos.y = terrainY + yOffset + 0.01f; // ���� + �߽ɺ��� + �ణ ���
//        }
//        else
//        {
//            // Terrain�� ���� ���: Raycast�� ���� ����
//            if (Physics.Raycast(newPos + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 10f))
//            {
//                newPos.y = hit.point.y + yOffset + 0.01f;
//            }
//            else
//            {
//                // ������ fallback
//                newPos.y = yOffset + 5f;
//            }
//        }

//        // ĳ���� �̵� ����
//        if (cc != null)
//        {
//            // �浹 ���� ������ ���� ��� ���ٰ� �ٽ� ��
//            cc.enabled = false;
//            xrOrigin.position = newPos; // ��ġ ���� ����
//            cc.enabled = true;
//        }
//        else
//        {
//            xrOrigin.position = newPos;
//        }

//        // ��ġ ������ ���� �Ŀ� ȸ�� ����(�̰� �߿���)
//        Vector3 forwardDir = direction.normalized;
//        forwardDir.y = 0f;

//        if (forwardDir != Vector3.zero)
//        {
//            //xrOrigin.rotation = Quaternion.LookRotation(forwardDir);
//            xrOrigin.rotation = Quaternion.LookRotation(-forwardDir);
//        }

//        // �����̵� �� ���鿡 Ȯ���� ���̱�
//         StartCoroutine(SnapToGround(xrOrigin));

//        // ����� ���
//        Debug.Log($"[TeleportWall] {wallType} �� �浹: {currentPos} �� {newPos}");
//    }

//    // ĳ���͸� ���鿡 ������ ���̴� �ڷ�ƾ
//    IEnumerator SnapToGround(Transform target)
//    {
//        yield return new WaitForEndOfFrame();

//        // 1�� �õ�: Raycast�� �Ʒ� ���� ����
//        if (!Physics.Raycast(target.position + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 10f))
//        {
//            // �������� ��� �� ������ �� ��ٷȴٰ� ��õ�
//            yield return new WaitForEndOfFrame();

//            if (Physics.Raycast(target.position + Vector3.up * 2f, Vector3.down, out hit, 10f))
//            {
//                Vector3 pos = target.position;
//                pos.y = hit.point.y;
//                target.position = pos;
//            }
//        }
//        else
//        {
//            // ���������� �ٷ� ���� ��ġ�� ����
//            Vector3 pos = target.position;
//            pos.y = hit.point.y;
//            target.position = pos;
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public WallType wallType;
    public float mapWidth = 38f;
    public float mapHeight = 38f;
    public float offset = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Transform xrOrigin = other.transform.root;
        CharacterController cc = xrOrigin.GetComponent<CharacterController>();
        float yOffset = cc != null ? cc.center.y : 1.0f;

        // ���� ��ġ
        Vector3 currentPos = xrOrigin.position;
        Vector3 newPos = currentPos;

        // �̵� ���� ���
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

        // ��ġ ��� (���� �Ÿ� + ��������ŭ)
        newPos = currentPos + direction * (distance + offset);

        // ������ Terrain ���� ���� (���� �������� �ʰ�)
        float terrainY = GetTerrainY(newPos);
        newPos.y = terrainY + yOffset + 0.01f;

        // �̵� ����
        if (cc != null)
        {
            cc.enabled = false;
            xrOrigin.position = newPos;
            cc.enabled = true;
        }
        else
        {
            xrOrigin.position = newPos;
        }

        // ȸ�� ���� (���� �ݴ� ���� ����)
        Vector3 lookDir = -direction.normalized;
        lookDir.y = 0f;
        if (lookDir != Vector3.zero)
        {
            xrOrigin.rotation = Quaternion.LookRotation(lookDir);
        }

        Debug.Log($"[TeleportWall] {wallType} �� �浹 �� ��ġ: {xrOrigin.position}");
    }

    // Terrain �Ǵ� Raycast�� y�� ���� ����
    private float GetTerrainY(Vector3 pos)
    {
        Terrain terrain = Terrain.activeTerrain;
        if (terrain != null)
        {
            return terrain.SampleHeight(pos) + terrain.GetPosition().y;
        }

        // Terrain�� ���ٸ� Raycast �õ�
        if (Physics.Raycast(pos + Vector3.up * 3f, Vector3.down, out RaycastHit hit, 10f))
        {
            return hit.point.y;
        }

        // ���� fallback
        return 0f;
    }
}




