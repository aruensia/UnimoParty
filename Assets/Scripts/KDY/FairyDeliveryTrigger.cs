using UnityEngine;
using Photon.Pun;

public class FairyDeliveryTrigger : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent<PhotonView>(out var dd) && dd.IsMine)
        {
            Manager.Instance.observer.AddScore();
            Debug.Log(" �÷��̾ AUBE�� ���� �� �ݳ�");
            Manager.Instance.observer.DeliveryFairy();

        }
    }

    ////�浹 ó����
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player") && photonView.IsMine)
    //    {
    //        Manager.Instance.observer.AddScore();
    //        Debug.Log(" �÷��̾ AUBE�� ���� �� �ݳ�");
    //        Manager.Instance.observer.DeliveryFairy();

//    }
}



