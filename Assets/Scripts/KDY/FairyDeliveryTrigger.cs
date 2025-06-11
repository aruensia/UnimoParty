using UnityEngine;

public class FairyDeliveryTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(" �÷��̾ AUBE�� ���� �� �ݳ�");
            Manager.Instance.observer.DeliveryFairy();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log(" �÷��̾ AUBEASDASD�� ���� �� �ݳ�");
            Manager.Instance.observer.DeliveryFairy();
        }
    }

}
