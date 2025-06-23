using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // ���� Input System ���ӽ����̽�

public class DashCooldown : MonoBehaviour
{
    [Header("��Ÿ�� ����")]
    public float cooldownTime = 5f; // ��Ÿ�� ���� �ð�

    [Header("UI ����")]
    public Image cooldownImage;      // ���� �̹��� (fillAmount ������)
    public Image dashIconImage;      // Dash ������ �̹��� (���� �����)

    [Header("��Ÿ�� ����")]
    public Color cooldownColor = new Color32(165, 165, 165, 165); // ��Ÿ�� �� ��
    private Color readyColor = new Color32(255, 255, 255, 255);   // ��Ÿ�� �Ϸ� ��

    private float timer = 0f;
    private bool isCooldown = false;

    // ���� Input System �׼� (C Ű�� �׽�Ʈ)
    private InputAction dashAction;

    void Start()
    {
        // �ʱ� ����: Dash ��� ����
        dashIconImage.color = readyColor;
        cooldownImage.fillAmount = 1f;

        // C Ű �Է� �� StartCooldown() ����
        dashAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/c");
        dashAction.performed += ctx =>
        {
            if (!isCooldown)
                StartCooldown();
        };
        dashAction.Enable();
    }

    void Update()
    {
        if (isCooldown)
        {
            // Ÿ�̸� ����
            timer += Time.deltaTime;

            // ���� �̹��� ���� ä���� (�ޡ��)
            cooldownImage.fillAmount = timer / cooldownTime;

            // ��Ÿ�� �Ϸ� �� ó��
            if (timer >= cooldownTime)
            {
                EndCooldown();
            }
        }
    }

    public void StartCooldown()
    {
        isCooldown = true;
        timer = 0f;

        // ���� �帮��, ������ ������ ���·� ����
        dashIconImage.color = cooldownColor;
        cooldownImage.fillAmount = 0f;
    }

    private void EndCooldown()
    {
        isCooldown = false;

        // ������ ���̴� ���·� ����
        dashIconImage.color = readyColor;
        cooldownImage.fillAmount = 1f;
    }
}
