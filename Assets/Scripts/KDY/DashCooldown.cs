using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // ���� Input System ���ӽ����̽�  ..�׽�Ʈ�� < �����Ұ���


public class DashCooldown : MonoBehaviour
{
    [Header("��Ÿ�� ����")]
    public float cooldownTime = 5f;

    [Header("UI ����")]
    public Image cooldownImage;      // ���� �̹��� (fillAmount ������)
    public Image dashIconImage;      // ���� ����� ������ �̹���

    [Header("��Ÿ�� ����")]
    public Color cooldownColor = Color.gray;

    private float timer = 0f;
    private bool isCooldown = false;
    private Color originalColor;

    // ���� InputSystem �׼�
    private InputAction dashAction;

    void Start()
    {
        // ���� ���� ���
        originalColor = dashIconImage.color;

        // �׽�Ʈ�� �׼� ���� (Shift Ű) << �����Ұ���
        dashAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/C");
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
            timer -= Time.deltaTime;
            cooldownImage.fillAmount = timer / cooldownTime;

            if (timer <= 0f)
            {
                EndCooldown();
            }
        }
    }

    public void StartCooldown()
    {
        isCooldown = true;
        timer = cooldownTime;

        dashIconImage.color = cooldownColor;
        cooldownImage.fillAmount = 1f;
    }

    private void EndCooldown()
    {
        isCooldown = false;
        cooldownImage.fillAmount = 0f;
        dashIconImage.color = originalColor;
    }
}
