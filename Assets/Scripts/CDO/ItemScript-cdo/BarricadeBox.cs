using Photon.Pun;
using System.Collections;
using UnityEngine;

public class BarricadeBox : MonoBehaviourPunCallbacks
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(wait());
        }



    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
        photonView.RPC("Explode", RpcTarget.All, true);

        yield return new WaitForSeconds(1);
        photonView.RPC("Explode", RpcTarget.All, false);

        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    void Explode(bool isFreeze)
    {
        JoystickController PlayerMove;
        HeadDash headDash;
        if ((PlayerMove = GetComponentInChildren<JoystickController>()) != null)
        {
            PlayerMove.Freeze(isFreeze);
            Debug.Log("�÷��̾� ������ �");
        }

        if ((headDash = GetComponentInChildren<HeadDash>()) != null)
        {
            headDash.Freeze(isFreeze);
            Debug.Log("�÷��̾� ��� �");
        }

        if (TryGetComponent<HandHarvest>(out HandHarvest PlayerHarvest))
        {
            PlayerHarvest.Freeze(isFreeze);

            Debug.Log("�÷��̾� ä�� �");
        }
    }





}
