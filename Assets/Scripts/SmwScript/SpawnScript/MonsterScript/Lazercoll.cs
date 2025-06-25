//������ ������ 
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazercoll : MonoBehaviourPunCallbacks
{
    int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;
        if (other.gameObject.tag == "Player")
        {

            damage = 1;
            var otherPV = other.GetComponent<PhotonView>();
            if (otherPV != null && otherPV.Owner != null)
            {
                // ������ ���� RPC
                photonView.RPC("HitPlayerRPC", otherPV.Owner, damage + 1);
            }
            Debug.Log(Manager.Instance.observer.UserPlayer.gamedata.life);
        }

    }

}
