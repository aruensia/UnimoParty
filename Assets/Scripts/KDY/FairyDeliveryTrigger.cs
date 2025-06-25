using UnityEngine;
using Photon.Pun;

public class FairyDeliveryTrigger : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&photonView.IsMine)
        {
            Manager.Instance.observer.AddScore();
            Debug.Log(" �÷��̾ AUBE�� ���� �� �ݳ�");
            Manager.Instance.observer.DeliveryFairy();

        }
    }
}
