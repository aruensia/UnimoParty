using Ilumisoft.RadarSystem;
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

        //if (!other.CompareTag("Player")) return;  ����
        //if (!other.CompareTag("Player")) return;  ����
        
        // �÷��̾� Transform ���� ��������
        Transform player = other.transform;
        
        //Transform player = other.transform;  ����
        //Transform player = other.transform;  ����

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


        // LookAt ����� �����Ǿ� �ִ� ��쿡�� ȸ�� ó�� ����
        if (lookAtTarget != null)
        {
            // LookAt ���� �÷��̾� ������ ���� ���� ���
            // ��, ���� ����(Y��)�� �����Ͽ� XZ ��� �󿡼��� ���⸸ ���
            Vector3 direction = lookAtTarget.position - player.position;
            direction.y = 0f; // Y���� 0���� ����� ������ ȸ��(Pitch, Roll) ����

            // ���� ���Ͱ� �ʹ� ������ ȸ���� �ʿ䰡 �����Ƿ� ����
            if (direction.sqrMagnitude > 0.001f)
            {
                // ��� ������ �ٶ󺸱� ���� ȸ�� ��(Quaternion) ���
                Quaternion targetRot = Quaternion.LookRotation(direction);

                // ���� �÷��̾��� ȸ������ Euler(����) ���·� ������
                Vector3 currentEuler = player.eulerAngles;

                // ��ǥ ȸ������ Euler ������ ��ȯ
                Vector3 targetEuler = targetRot.eulerAngles;

                // ���� ȸ�� ����: ���� X(�� ����)�� Z(�� ����)�� �״�� �����ϰ�,
                // Y(�¿� ȸ��)�� ��ǥ �������� ���
                player.rotation = Quaternion.Euler(currentEuler.x, targetEuler.y, currentEuler.z);

                // ���̴� ȸ�� ��� ����
                player.GetComponentInChildren<Radar>().RefreshRotationImmediately();
            }
        }
    }

    // ���� ���� �� �� ���� 
    //private void OnTriggerExit(Collider other)
    //{
    //    // �浹�� ��ü�� "Player" �±׸� ������ ���� ������ ����
    //    if (!other.CompareTag("Player")) return;
    //    //if (!other.CompareTag("Player")) return;  ���� �±׷� ����
    //    //if (!other.CompareTag("Player")) return;  ���� �±׷� ����

    //    // �÷��̾� Transform ���� ��������
    //    Transform player = other.transform;
    //    //Transform player = other.transform;    ���ͷ� ����
    //    //Transform player = other.transform;    ���ͷ� ����
         
    //    // ���� ��ġ ����//
    //    Vector3 currentPos = player.position;

    //    // ���ο� ��ġ�� ���� ��ġ�� �⺻���� �ϵ�, �Ϻ� �ุ ������
    //    Vector3 newPos = currentPos;

    //    // �� ���⿡ ���� �ݴ����� �ش� �� ��ǥ�� �����
    //    switch (wallType)
    //    {
    //        case WallType.Top:    // ���� ��: Z�ุ �̵�
    //            newPos.z = teleportReference.position.z;
    //            break;

    //        case WallType.Bottom: // �Ʒ��� ��: Z�ุ �̵�
    //            newPos.z = teleportReference.position.z;
    //            break;

    //        case WallType.Left:   // ���� ��: X�ุ �̵�
    //            newPos.x = teleportReference.position.x;
    //            break;

    //        case WallType.Right:  // ������ ��: X�ุ �̵�
    //            newPos.x = teleportReference.position.x;
    //            break;
    //    }

    //    // ���� ��ġ ����
    //    player.position = newPos;
    //}
}
