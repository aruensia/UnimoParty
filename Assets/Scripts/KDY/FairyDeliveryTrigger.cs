using UnityEngine;

public class FairyDeliveryTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Manager.Instance.observer.AddScore();
            Debug.Log(" �÷��̾ AUBE�� ���� �� �ݳ�");
            Manager.Instance.observer.DeliveryFairy();

        }
    }
}
