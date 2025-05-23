using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    static Manager instance;
    public GoalFairyCount goalCount;
    public GameObject _XRDeviceSimulator;
    private bool isMaster = false;
    [SerializeField] private bool isTestMode = false;

    // �ν����ͷ� �׽�Ʈ�� �� �� �ش� ���� Sprit�� private ������ �����Ͽ� ������ ���ƾ���.
    public int tempFairyValue_1 = 10;
    public int tempFairyValue_2 = 10;
    public int tempFairyValue_3 = 10;

    public static Manager Instance
    {
        get { return instance; }
    }

    public IngameObserver observer = new IngameObserver();

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

        _XRDeviceSimulator.SetActive(false);
    }

    private void Start()
    {
        if(isTestMode)
        {
            _XRDeviceSimulator.SetActive(true);
        }
        else
        {
            _XRDeviceSimulator.SetActive(false);
        }
    }
}
