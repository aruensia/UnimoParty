using UnityEngine;

public class Fragment : MonoBehaviour
{
    [SerializeField] Bungpeo Parents;
    [SerializeField] GameObject CrashBunpeoFragment;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Instantiate(CrashBunpeoFragment, transform.position, Quaternion.identity);
        }
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(CrashBunpeoFragment, transform.position, Quaternion.identity);
        }
        Parents.IsActivate();//���߿� �̰ɷ� �����
        //Parents.IsActivateRPC();//�̰Ŵ� �׽�Ʈ��
        gameObject.SetActive(false);
    }

}
