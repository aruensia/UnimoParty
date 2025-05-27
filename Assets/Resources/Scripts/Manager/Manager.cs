using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    static Manager instance;
    public GoalFairyCount goalCount;
    private bool isMaster = false;

    // �ν����ͷ� �׽�Ʈ�� �� �� �ش� ���� Sprit�� private ������ �����Ͽ� ������ ���ƾ���.
    public int tempFairyValue_1 = 10;
    public int tempFairyValue_2 = 10;
    public int tempFairyValue_3 = 10;

    public static Manager Instance
    {
        get { return instance; }
    }

    public DataLoader dataLoader = new DataLoader();
    public IngameObserver observer = new IngameObserver();
    Shop shop;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        dataLoader.DataLoad();
    }
}
