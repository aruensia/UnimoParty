using UnityEngine;

public class FairyDeliveryTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(" �÷��̾ AUBE�� ���� �� �ݳ�");
            Manager.Instance.observer.DeliveryFairy();

            Manager.Instance.observer.AddScore();
        }
    }
}
