using UnityEngine;

public class Spirit : MonoBehaviour
{
    public int SpiritPointRepository;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.tag == "Player")
        {
            Debug.Log("�ݶ��̴�");
            SpiritPointRepository++;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }


}
