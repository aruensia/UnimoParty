using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameObserver
{
    public event Action<DataCenter> OnGameDataChange;
    public event Action<List<int>> OnGameRankChange;
    public event Action OnGameEnd;

    public UserPlayer UserPlayer { get; set; }
    private FairyType tempPlayerFairy;
    private int gameoverTargetScore = 100;
    public int roomInPlayerCount;

    private bool isGameOver = false;
    ItemData _selectItem;
    System.Random _itemRandomNum;

    public void Setting()
    {
        //UserPlayer.gamedata.life = 100;
        var tempUser = UserPlayer.gamedata;

        OnGameDataChange.Invoke(tempUser);
    }

    public void HitPlayer(int damage)
    {
        if (UserPlayer.gamedata._playerState == PlayerState.None)
        {
            UserPlayer.gamedata.life = UserPlayer.gamedata.life - damage;
        }
        else if (UserPlayer.gamedata._playerState == PlayerState.Invincible)
        {
            UserPlayer.gamedata.life = UserPlayer.gamedata.life = 0;
        }

        var templife = UserPlayer.gamedata;

        //���⿡ ���� �߰�.
        OnGameDataChange?.Invoke(templife);

        if (UserPlayer.gamedata.life <= 0)
        {
            isGameOver = true;

            SceneManager.LoadScene(1);

            // ���⿡ ���� �߰�.
            // OnGameEnd?.Invoke();
        }
    }

    public void RecoveryPlayerHP(int RecoveryHP)
    {
        UserPlayer.gamedata.life += RecoveryHP;

        if (UserPlayer.gamedata.life >= 100)
        {
            UserPlayer.gamedata.life = 100;
        }

        var templife = UserPlayer.gamedata;
        OnGameDataChange?.Invoke(templife);
    }

    void GetItem()
    {
        var randomrange = _itemRandomNum.Next((int)ItemName.Potion, (int)ItemName.FreezeBoom, (int)ItemName.end);
        switch (randomrange)
        {
            //������ enum ũ�⸸ŭ case �����Ͽ� userItemDatas�� ���� ���� ����
            case (int)ItemName.Potion:
                Manager.Instance.observer.UserPlayer.gamedata._Inventory.userItemDatas[(int)ItemName.Potion].ItemData.ItemCount++;
                break;

            case (int)ItemName.FreezeBoom:
                Manager.Instance.observer.UserPlayer.gamedata._Inventory.userItemDatas[(int)ItemName.FreezeBoom].ItemData.ItemCount++;
                break;

            case (int)ItemName.end:
                break;
        }

        var tempUser = UserPlayer.gamedata;

        OnGameDataChange.Invoke(tempUser);
    }

    void UseItem(ItemData selectitem)
    {
        switch(selectitem.type)
        {
            case ItemName.Potion:
                //�տ� �������� ��� ��� �ߴٴ� ����� �߰�
                Manager.Instance.observer.UserPlayer.gamedata._Inventory.userItemDatas[(int)ItemName.Potion].ItemData.ItemCount--;
                break;

            case ItemName.FreezeBoom:
                Manager.Instance.observer.UserPlayer.gamedata._Inventory.userItemDatas[(int)ItemName.FreezeBoom].ItemData.ItemCount--;
                break;

            case ItemName.end:
                break;
        }

        var tempUser = Manager.Instance.observer.UserPlayer.gamedata;
        OnGameDataChange.Invoke(tempUser);
    }

    void ChangeItem()
    {

    }

    void SelectItem(ItemData selectitem)
    {
        _selectItem = selectitem;

        var tempuser = Manager.Instance.observer.UserPlayer.gamedata;

        OnGameDataChange.Invoke(tempuser);
    }

    public void GetFairy(FairyType fairytype)
    {
        //�� Ÿ���� �̹� ���� ���¿��� ���� ���� ������.
        UserPlayer.gamedata.playerFairyType = fairytype;
        var tempfairy = UserPlayer.gamedata;

        //���⿡ ���� �߰�.
        OnGameDataChange?.Invoke(tempfairy);
    }

    public void DeliveryFairy()
    {
        Manager.Instance.goalCount.GoalFairyValue_1 += UserPlayer.gamedata.playerFairyType.FairyDataType_1;
        Manager.Instance.goalCount.GoalFairyValue_2 += UserPlayer.gamedata.playerFairyType.FairyDataType_2;
        Manager.Instance.goalCount.GoalFairyValue_3 += UserPlayer.gamedata.playerFairyType.FairyDataType_3;

        tempPlayerFairy.FairyDataType_1 = 0;
        tempPlayerFairy.FairyDataType_2 = 0;
        tempPlayerFairy.FairyDataType_3 = 0;

        UserPlayer.gamedata.playerFairyType = tempPlayerFairy;
        var tempfairy = UserPlayer.gamedata;

        if(Manager.Instance.goalCount.GoalFairyValue_1 == Manager.Instance.tempFairyValue_1 && Manager.Instance.goalCount.GoalFairyValue_2 == Manager.Instance.tempFairyValue_2 && Manager.Instance.goalCount.GoalFairyValue_3 == Manager.Instance.tempFairyValue_3)
        {
            isGameOver = true;
            OnGameEnd?.Invoke();
        }

        //���⿡ ���� �߰�.
        OnGameDataChange?.Invoke(tempfairy);
    }

    public void AddScore()
    {
        var tempscore1 = tempPlayerFairy.FairyDataType_1 * Manager.Instance._FairyScore_1;
        var tempscore2 = tempPlayerFairy.FairyDataType_2 * Manager.Instance._FairyScore_2;
        var tempscore3 = tempPlayerFairy.FairyDataType_3 * Manager.Instance._FairyScore_3;
        var tempscore = tempscore1 + tempscore2 + tempscore3;

        UserPlayer.gamedata.score = UserPlayer.gamedata.score + tempscore;
        var tempuser = UserPlayer.gamedata;

        //���⿡ ���� �߰�.
        OnGameDataChange?.Invoke(tempuser);

        if(UserPlayer.gamedata.score >= gameoverTargetScore)
        {
            isGameOver = true;

            // ���⿡ ���� �߰�.
            OnGameEnd?.Invoke();
        }
    }

    public void BuyItem(ItemData buyitems)
    {
        UserPlayer.gamedata._money -= buyitems.ItemCost;
        UserPlayer.gamedata._Inventory.userItemDatas[0].ItemData.ItemCount++;

        var tempuser = UserPlayer.gamedata;

        OnGameDataChange?.Invoke(tempuser);
    }

    public void ResetPlayer()
    {
        var tempPlayer = UserPlayer.gamedata.Clone();

        //���⿡ ���� �߰�.
        OnGameDataChange?.Invoke(tempPlayer);
    }

    //������ ������ �����Ű�� �޼ҵ�.
    public void EndGame()
    {
        isGameOver = true;

        //���⿡ ���� �߰�.
        OnGameEnd?.Invoke();
    }
}
