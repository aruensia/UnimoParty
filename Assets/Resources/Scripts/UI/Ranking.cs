using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranking : MonoBehaviour
{
    List<int> score = new List<int> { 8, 4, 4, 1, 3, 3 };
    List<int> ranks = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        //Manager.Instance.observer.OnGameRankChange += RefreshPlayerScoreRank;
        //InitPlayerCount();
        DefaultPlayerScoreSetting(score);
        RefreshPlayerScoreRank(score);
    }

    void InitPlayerCount()
    {
        //�÷��̾� �� ��ŭ ��ũ List�� �߰���.
        for (int i = 0; 0 < Manager.Instance.observer.roomInPlayerCount; i++)
        {
            score.Add(Manager.Instance.observer.UserPlayer.gamedata.score);
        }
    }

    //�����Ʈ�� ���� ������ ������������ ����
    void DefaultPlayerScoreSetting(List<int> arr)
    {
        int n = arr.Count;

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (arr[j] < arr[j + 1])
                {
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                }
            }
        }

        Debug.Log($"���� ���: {{ {string.Join(", ", score)} }}");
    }

    //1��� ������ ���Ͽ�, ����� �ο�
    void RefreshPlayerScoreRank(List<int> arr)
    {
        var temprank = 1;
        ranks.Clear();
        ranks.Add(temprank);

        for(int i = 0; i < arr.Count-1; i++)
        {
            if( arr[i] == arr[i + 1] )
            {
                ranks.Add(temprank);
            }
            else
            {
                temprank++;
                ranks.Add(temprank);
            }
        }
        Debug.Log($"���� ���: {{ {string.Join(", ", ranks)} }}");
    }
}
