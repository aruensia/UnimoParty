using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [Header("�ؽ�Ʈ")]
    [SerializeField] private TextMeshProUGUI[] texts = new TextMeshProUGUI[4];

    // ��ǥ���� GoalFairyCount�� ���� (Manager���� �����´ٰ� ����)
    private GoalFairyCount goalCount
    {
        get { return Manager.Instance.goalCount; }
    }

    private void Start()
    {
        Manager.Instance.observer.OnGameDataChange += HpPoint;
        Manager.Instance.observer.OnGameDataChange += FairyType1Score;
        Manager.Instance.observer.OnGameDataChange += FairyType2Score;
        Manager.Instance.observer.OnGameDataChange += FairyType3Score;
    }

    private void OnDestroy()//�̰Ŵ� �����ڵ�
    {
        Manager.Instance.observer.OnGameDataChange -= HpPoint;
        Manager.Instance.observer.OnGameDataChange -= FairyType1Score;
        Manager.Instance.observer.OnGameDataChange -= FairyType2Score;
        Manager.Instance.observer.OnGameDataChange -= FairyType3Score;
    }

    // ü�� ǥ�� (texts[0])
    public void HpPoint(DataCenter data)
    {
        texts[0].text = $"HP: {data.life}";
    }

    // �� FairyType�� ���� ǥ�� (��ǥ���� goalCount���� �ٷ� ������)
    public void FairyType1Score(DataCenter data)
    {
        texts[1].text = $"{data.playerFairyType.FairyDataType_1} / {goalCount.GoalFairyValue_1}";
    }
    public void FairyType2Score(DataCenter data)
    {
        texts[2].text = $"{data.playerFairyType.FairyDataType_2} / {goalCount.GoalFairyValue_2}";
    }
    public void FairyType3Score(DataCenter data)
    {
        texts[3].text = $"{data.playerFairyType.FairyDataType_3} / {goalCount.GoalFairyValue_3}";
    }

    public void ResetScore()
    {
        for (int i = 0; i < texts.Length; i++)
            texts[i].text = $"0 / 0";
    }
}
