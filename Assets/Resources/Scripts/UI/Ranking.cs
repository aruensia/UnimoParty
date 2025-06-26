using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Ranking : MonoBehaviour
{
    List<int> tempscore = new List<int>();
    List<int> ranks = new List<int>();
    public List<TextMeshProUGUI> scoretexts;
    public List<TextMeshProUGUI> nicknametexts;
    public List<TextMeshProUGUI> rewardtexts;
    public List<GameObject> rankObject;


    // Start is called before the first frame update
    void Start()
    {
        tempscore = Manager.Instance.score;
        DefaultPlayerScoreSetting(tempscore);
        RefreshPlayerScoreRank(tempscore);
        SetPlayerList();
    }

    void SetPlayerList()
    {
        for( int i = 1; i < Manager.Instance.players.Count; i++ )
        {
            rankObject[i+1].gameObject.SetActive(true);
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

        Debug.Log($"���� ���: {{ {string.Join(", ", tempscore)} }}");
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
