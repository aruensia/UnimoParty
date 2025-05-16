using UnityEngine;

public class SettingValues : MonoBehaviour
{
    /// <summary>
    /// ����� �������õȰ͵� ���� ���⼭ �Լ�������ų� ���� �����ϱ� ���� ���� �̱���
    /// ���⼭ ���� �߰��Ҳ��� �׳� ���� 1�� ������ �׸��� SettingControl���� ����ġ �߰� 
    /// </summary>
    public static SettingValues Instance { get; private set; }

    public int Option1 = 1;
    public int Option2 = 2;
    public int Option3 = 3;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
