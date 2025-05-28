using UnityEngine;
using Ilumisoft.RadarSystem; // RadarIcon.cs�� ���ǵ� ���ӽ����̽�
/// <summary>
/// ���̴��� ǥ�õ� �� �ִ� ��� ������Ʈ(����, �÷��̾�, ä���� ��)�� �ٴ� ��ũ��Ʈ
/// �� ��ũ��Ʈ�� ���� �ش� ������Ʈ�� ���̴� ��Ʈ�� ������ �� �ְ� �ȴ�.
/// </summary>
public class RadarTarget : MonoBehaviour
{
    /// <summary>
    /// ���̴� �������� �����ϴ� �Լ�
    /// �� �Լ��� Radar���� ȣ��
    /// ������Ʈ�� �±׿� ���� �ش��ϴ� ��Ʈ �������� �ҷ��ͼ� ����
    /// </summary>
    public RadarIcon CreateIcon()
    {
        GameObject iconPrefab = null;

        // ������Ʈ�� �±׿� ���� ����� ��Ʈ ������ ����
        if (CompareTag("Monster"))
            iconPrefab = Resources.Load<GameObject>("RadarDot_Monster"); // ����: ����
        else if (CompareTag("OtherPlayer"))
            iconPrefab = Resources.Load<GameObject>("RadarDot_OtherUser"); // ���: �ٸ��÷��ƾ�
        else if (CompareTag("Object"))
            iconPrefab = Resources.Load<GameObject>("RadarDot_Object"); // ���: ä����

        // �ش� �±׿� �´� �������� ã�� ���� ��� ��� ��� �� null ��ȯ
        if (iconPrefab == null)
        {
            Debug.LogWarning($"[Radar] �±� '{gameObject.tag}'������ �������� ����");
            return null;
        }

        // ��Ʈ ������ �ν��Ͻ� ����
        GameObject iconGO = Instantiate(iconPrefab);

        // ������ ������Ʈ���� RadarIcon ������Ʈ�� ��ȯ
        return iconGO.GetComponent<RadarIcon>();
    }
}
