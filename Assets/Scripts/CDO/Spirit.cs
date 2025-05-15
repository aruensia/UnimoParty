using UnityEngine;

public partial class Spirit : MonoBehaviour
{
    public int SpiritPointRepository;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.tag == "Player")
        {
            Debug.Log("�ݶ��̴�1");
            if (collision.gameObject.TryGetComponent<HandHarvest>(out HandHarvest player))
            {
                Debug.Log("�ݶ��̴�2");
                SpiritPointRepository += player.DeliverySpirit();
            }
        }
    }

  

    private void OnCollisionExit(Collision collision)
    {
        
    }
}

public partial class Spirit : MonoBehaviour
{
    //private int tempFairyValue_1 = 10;
    //private int tempFairyValue_2 = 10;
    //private int tempFairyValue_3 = 10;

    // ä������ �ݳ��� �� ��, 
    // �Ʒ� ��η� ���� �޼ҵ带 �ҷ����� ��;
    private void CallMethod()
    {
        Manager.Instance.observer.DeliveryFairy();
    }

    private void Start()
    {
        SetDefaultCount();
    }

    void SetDefaultCount()
    {
        Manager.Instance.goalCount.GoalFairyValue_1 = Manager.Instance.tempFairyValue_1;
        Manager.Instance.goalCount.GoalFairyValue_2 = Manager.Instance.tempFairyValue_2;
        Manager.Instance.goalCount.GoalFairyValue_3 = Manager.Instance.tempFairyValue_3;
    }
}
