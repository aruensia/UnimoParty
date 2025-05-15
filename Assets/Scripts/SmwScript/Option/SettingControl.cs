using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingControl : MonoBehaviour
{
    /// <summary>
    /// ����� ��ư�� ������ ���� �ٲ�԰ڲ� �� Ŭ���� ���� ���� �� �߰��Ҳ��� switch�� ���ִºκ� 2���� �߰��ϰ� ����ϸ��
    /// </summary>
    [Header("UI ����")]
    public TextMeshProUGUI settingNameText;
    public TextMeshProUGUI settingValueText;
    public Button increaseButton;
    public Button decreaseButton;

    [Header("���� Ű�� ����")]
    [SerializeField] private string settingKey = "Option1";
    private int minValue = 0;
    private int maxValue = 100;

    void Start()
    {
        settingNameText.text = settingKey;

        increaseButton.onClick.AddListener(() => ChangeValue(+1));
        decreaseButton.onClick.AddListener(() => ChangeValue(-1));

        UpdateValueText();
    }

    void ChangeValue(int delta)
    {
        int current = GetSettingValue();
        int next = Mathf.Clamp(current + delta, minValue, maxValue);
        SetSettingValue(next);
        UpdateValueText();
    }

    void UpdateValueText()
    {
        settingValueText.text = GetSettingValue().ToString();
    }

    int GetSettingValue()
    {
        switch (settingKey)
        {
            //���� ����ġ �߰��ϸ��1
            case "Option1": return SettingValues.Instance.Option1;
            case "Option2": return SettingValues.Instance.Option2;
            case "Option3": return SettingValues.Instance.Option3;
            default: return 0;
        }
    }

    void SetSettingValue(int value)
    {
        switch (settingKey)
        {
            //���� ����ġ �߰��ϸ��2
            case "Option1": SettingValues.Instance.Option1 = value; break;
            case "Option2": SettingValues.Instance.Option2 = value; break;
            case "Option3": SettingValues.Instance.Option3 = value; break;
        }
    }
}
