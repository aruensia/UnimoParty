using UnityEngine;

public class WallTeleporter : MonoBehaviour
{
    public enum WallType { Top, Bottom, Left, Right }

    [Header("���� ��ġ ������ ����")]
    public WallType wallType;

    [Header("�ݴ��� �� ��ġ�� ����")]
    public Transform teleportReference; // �ݴ��� ������

    [Header("LookAt ��� (�÷��̾ �ٶ󺸰� �� ������Ʈ)")]
    public Transform lookAtTarget;


    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ��ü�� "Player" �±׸� ������ ���� ������ ����
        if (!other.CompareTag("Player")) return;

        // �÷��̾� Transform ���� ��������
        Transform player = other.transform;

        // ���� ��ġ ����
        Vector3 currentPos = player.position;

        // ���ο� ��ġ�� ���� ��ġ�� �⺻���� �ϵ�, �Ϻ� �ุ ������
        Vector3 newPos = currentPos;

        // �� ���⿡ ���� �ݴ����� �ش� �� ��ǥ�� �����
        switch (wallType)
        {
            case WallType.Top:    // ���� ��: Z�ุ �̵�
                newPos.z = teleportReference.position.z;
                break;

            case WallType.Bottom: // �Ʒ��� ��: Z�ุ �̵�
                newPos.z = teleportReference.position.z;
                break;

            case WallType.Left:   // ���� ��: X�ุ �̵�
                newPos.x = teleportReference.position.x;
                break;

            case WallType.Right:  // ������ ��: X�ุ �̵�
                newPos.x = teleportReference.position.x;
                break;
        }

        // ���� ��ġ ����
        player.position = newPos;


        // LookAt ����� �Ҵ�Ǿ� �ִٸ�, �ٶ󺸵��� ó��
        if (lookAtTarget != null)
        {
            player.LookAt(lookAtTarget);
        }
    }


}
