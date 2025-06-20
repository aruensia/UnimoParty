using System.Collections;
using System.Threading.Tasks;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;

public static class SelectedData
{
    public static int characterIndex = 0;
    public static int shipIndex = 0;
}
public class ShopManager : MonoBehaviour
{
    [SerializeField] Button buyButton;
    private int ingameMoney;
    [SerializeField] TextMeshProUGUI money1;

    [Header("�����յ�")]
    [SerializeField] GameObject[] characters;
    [SerializeField] GameObject[] ships;

    [Header("��ũ�� ���")]
    [SerializeField] GameObject[] scrollVeiw;

    [Header("��� ��")]
    [SerializeField] Toggle[] viewToggles;

    public bool isBuy = false;
    [SerializeField] SpaceShip[] spaceShipD;


    [SerializeField] GameObject sellPanel;


    private int selectedCharacterIndex = 0;
    private int selectedShipIndex = 0;
    private IEnumerator Start()
    {
        ingameMoney = Manager.Instance.observer.UserPlayer.gamedata._money;
        money1.text = ingameMoney.ToString();

        PlayerPrefs.GetInt("SelectedCharacterIndex", SelectedData.characterIndex);
        PlayerPrefs.GetInt("SelectedShipIndex", SelectedData.shipIndex);

        sellPanel.SetActive(false);

        for (int i = 0; i < viewToggles.Length; i++)
        {
            if (viewToggles[i].isOn)
            {
                ViewActive(i);
                break;
            }
        }

        foreach (SpaceShip d in spaceShipD)
        {
            Debug.Log(d.SpaceShipData._isbuy);

            Task<bool> loadTask = FirebaseAuthMgr.Instance.LoadUserDataAsync<bool>(FirebaseAuthMgr.user.DisplayName, d.gameObject.name);
            yield return new WaitUntil(() => loadTask.IsCompleted);

            d.SpaceShipData._isbuy = loadTask.Result;

            if (d.SpaceShipData._isbuy)
            {

                // ���ŵ� ��� ����
            }
        }

    }

    public void CharacterPreview(int index)
    {
        SelectedData.characterIndex = index;
        Debug.Log(index);
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(i == index);
        }
    }

    public void ShipPreview(int index)
    {
        SelectedData.shipIndex = index;
        Debug.Log(index);
        for (int i = 0; i < ships.Length; i++)
        {
            ships[i].SetActive(i == index);
        }
    }
    public void ViewActive(int activeIndex)
    {
        for (int i = 0; i < scrollVeiw.Length; i++)
        {
            scrollVeiw[i].SetActive(i == activeIndex);
        }
    }

    public void SaveSelectedIndices()
    {
        // 1. PlayerPrefs ���� (���� �����)
        PlayerPrefs.SetInt("SelectedCharacterIndex", SelectedData.characterIndex);
        PlayerPrefs.SetInt("SelectedShipIndex", SelectedData.shipIndex);
        PlayerPrefs.Save();

        // 2. Photon CustomProperties ����
        PhotonHashTable hash = new PhotonHashTable
        {
            { "CharacterIndex", SelectedData.characterIndex },
            { "ShipIndex", SelectedData.shipIndex }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        Debug.Log($"���� ���� �Ϸ�: ĳ���� {SelectedData.characterIndex}, ���ּ� {SelectedData.shipIndex}");
    }

    public void BuyShipButton(SpaceShip selectedShip)
    {
        buyButton.onClick.AddListener(() => BuyShip(selectedShip));
        selectedShip.shipnamek = selectedShip.gameObject.name;
        sellPanel.SetActive(true);
        //buyButton.gameObject.SetActive(false);
    }

    public void BuyShip(SpaceShip selectedShip)
    {
        Manager.Instance.observer.BuyShip(selectedShip.SpaceShipData);

        ingameMoney = Manager.Instance.observer.UserPlayer.gamedata._money;
        money1.text = ingameMoney.ToString();
        Debug.Log("���� ���� ��");
        StartCoroutine(FirebaseAuthMgr.Instance.SaveUserData(FirebaseAuthMgr.user.DisplayName, selectedShip.shipnamek, selectedShip.SpaceShipData._isbuy = true));

        spaceShipD = null;
        sellPanel.SetActive(false);
    }



}
