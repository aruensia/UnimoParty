using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    [SerializeField] private float gameDuration = 120f;

   
    private float currentTime;

   
    [SerializeField] private TextMeshProUGUI timeText;

    private void Start()
    {
        currentTime = gameDuration;
    }

    private void Update()
    {
        if (currentTime <= 0f)
        {
            currentTime = 0f;

            //���� ���� ȣ��  isGameOver ó�� ���Ե� (������� �ݿ���)
            Manager.Instance.observer.EndGame();

            //Ÿ�̸� ��Ȱ��ȭ�� ����
            enabled = false;
            return;
        }

        currentTime -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timeText.text = $"{minutes:00}:{seconds:00}";
    }
}
