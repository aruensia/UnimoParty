using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private int ingameMoney;
    [SerializeField] TextMeshProUGUI money1;

    [Header("�� ������")]
    [SerializeField] GameObject riBeePrefab;
    [SerializeField] GameObject catPrefab;

    [Header("���ּ� ������")]
    [SerializeField] GameObject beeSpaceShip;
    [SerializeField] GameObject boxSpaceShip;

    [Header("��ũ�� ���")]
    [SerializeField] GameObject[] scrollVeiw;

    [Header("��� ��")]
    [SerializeField] Toggle[] viewToggles;

    private void Start()
    {
        ingameMoney = Manager.Instance.observer.UserPlayer.gamedata._money;
        money1.text = ingameMoney.ToString();

        Debug.Log(ingameMoney);
        for (int i = 0; i < viewToggles.Length; i++)
        {
            if (viewToggles[i].isOn)
            {
                ViewActive(i);
                break;
            }
        }
    }

    public void ViewActive(int activeIndex)
    {
        for (int i = 0; i < scrollVeiw.Length; i++)
        {
            scrollVeiw[i].SetActive(i == activeIndex);
        }
    }

}
