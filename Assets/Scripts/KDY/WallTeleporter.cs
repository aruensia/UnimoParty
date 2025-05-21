using UnityEngine;

public class WallTeleporter : MonoBehaviour
{
    public enum WallType { Top, Bottom, Left, Right }
    public WallType wallType;

    public Transform teleportReference; // �ݴ��� ������ (Empty Object)

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Transform player = other.transform;
        Vector3 currentPos = player.position;
        Vector3 newPos = currentPos; // �����ؼ� ����

        // �� ���� �� �̵�
        switch (wallType)
        {
            case WallType.Top:    // +Z �� -Z
                newPos.z = teleportReference.position.z;
                break;
            case WallType.Bottom: // -Z �� +Z
                newPos.z = teleportReference.position.z;
                break;
            case WallType.Left:   // -X �� +X
                newPos.x = teleportReference.position.x;
                break;
            case WallType.Right:  // +X �� -X
                newPos.x = teleportReference.position.x;
                break;
        }

        // ��ġ �̵�
        player.position = newPos;

        // ���� ���� �Ǵ� (�� ���ؿ��� �÷��̾� forward�� �ڸ� ȸ��)
        Vector3 localForward = transform.InverseTransformDirection(player.forward);
        if (localForward.z < 0f)
        {
            Vector3 euler = player.eulerAngles;
            euler.y += 180f;
            player.rotation = Quaternion.Euler(euler);
        }
    }
}
