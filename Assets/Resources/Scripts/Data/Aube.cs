using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aube : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            Debug.Log("�ݶ��̴�1");
            if (collision.gameObject.TryGetComponent<HandHarvest>(out HandHarvest player))
            {
                Debug.Log("�ݶ��̴�2");
                //Manager.Instance.observer.UserPlayer.gamedata.playerFairyType += player.DeliveryAube();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {

    }

}
