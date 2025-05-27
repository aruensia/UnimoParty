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
        // ���� ä����
        int current1 = data.playerFairyType.FairyDataType_1;
        int current2 = data.playerFairyType.FairyDataType_2;
        int current3 = data.playerFairyType.FairyDataType_3;

        // ��ǥ��
        int goal1 = Manager.Instance.tempFairyValue_1;
        int goal2 = Manager.Instance.tempFairyValue_2;
        int goal3 = Manager.Instance.tempFairyValue_3;


        row1.text = $"{current1} / {goal1}";
        row2.text = $"{current2} / {goal2}";
        row3.text = $"{current3} / {goal3}";
    }

}
