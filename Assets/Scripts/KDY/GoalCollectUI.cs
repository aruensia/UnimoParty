using UnityEngine;
using TMPro;

public class GoalCollectUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI row1;
    [SerializeField] private TextMeshProUGUI row2;
    [SerializeField] private TextMeshProUGUI row3;

    private void Start()
    {
        // ���ӽ��� �ʱⰪ
        //UpdateCollectUI(Manager.Instance.observer.UserPlayer.gamedata);

        // ä�� ���� �ٲ� ������ UI ������Ʈ
        Manager.Instance.observer.OnGameDataChange += UpdateCollectUI;
    }

    private void OnDestroy()
    {
        // ���� ����
        Manager.Instance.observer.OnGameDataChange -= UpdateCollectUI;
    }

    // ä�� �� / ��ǥ �� ����
    private void UpdateCollectUI(DataCenter data)
    {
        // ��ǥ��
        int goal1 = Manager.Instance.tempFairyValue_1;
        int goal2 = Manager.Instance.tempFairyValue_2;
        int goal3 = Manager.Instance.tempFairyValue_3;

        // �÷��̾ ������Ʈ�� �ݳ��� �� �� ���� (UI�� ǥ�õ� ä�� ��)
        // DeliveryFairy �Լ� ȣ�� �ÿ��� ����
        int current1 = Manager.Instance.goalCount.GoalFairyValue_1;
        int current2 = Manager.Instance.goalCount.GoalFairyValue_2;
        int current3 = Manager.Instance.goalCount.GoalFairyValue_3;


        row1.text = $"{current1} / {goal1}";
        row2.text = $"{current2} / {goal2}";
        row3.text = $"{current3} / {goal3}";
    }

}
