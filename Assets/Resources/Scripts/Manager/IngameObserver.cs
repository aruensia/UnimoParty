using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameObserver
{
    public event Action<DataCenter> OnGameDataChange;
    public event Action OnGameEnd;

    public UserPlayer UserPlayer { get; set; }
    private int gameoverTargetScore = 100;

    private bool isGameOver = false;


    public void HitPlayer(int damage)
    {
        UserPlayer.gamedata.life = UserPlayer.gamedata.life - damage;
        var templife = UserPlayer.gamedata;

        //���⿡ ���� �߰�.
        OnGameDataChange?.Invoke(templife);

        if (UserPlayer.gamedata.life <= 0)
        {
            isGameOver = true;

            // ���⿡ ���� �߰�.
            OnGameEnd.Invoke();
        }
    }

    public void GetFairy(FairyType fairytype)
    {
        //�� Ÿ���� �̹� ���� ���� ���¿��� ���� ���� ������.
        UserPlayer.gamedata.playerFairyType = fairytype;
        var tempfairy = UserPlayer.gamedata;

        //���⿡ ���� �߰�.
        OnGameDataChange?.Invoke(tempfairy);
    }

    public void RetrunFairy()
    {
        FairyType tempPlayerFairy;
        tempPlayerFairy.FairyType1 = 0;
        tempPlayerFairy.FairyType2 = 0;
        tempPlayerFairy.FairyType3 = 0;

        UserPlayer.gamedata.playerFairyType = tempPlayerFairy;
        var tempfairy = UserPlayer.gamedata;

        //���⿡ ���� �߰�.
        OnGameDataChange?.Invoke(tempfairy);
    }

    public void AddScore(int score)
    {
        UserPlayer.gamedata.score = UserPlayer.gamedata.score + score;
        var tempscore = UserPlayer.gamedata;

        //���⿡ ���� �߰�.
        OnGameDataChange?.Invoke(tempscore);

        if(UserPlayer.gamedata.score >= gameoverTargetScore)
        {
            isGameOver = true;

            // ���⿡ ���� �߰�.
            OnGameEnd.Invoke();
        }
    }

    public void ResetPlayer()
    {

    }

    public void EndGame()
    {
        if(isGameOver == true)
        {
            //���⿡ ���� �߰�.
            OnGameEnd.Invoke();
        }
    }
}
