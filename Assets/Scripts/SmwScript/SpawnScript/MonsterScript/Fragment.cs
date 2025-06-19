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
            Parents.IsActivateRPC();
            //Parents.IsActivate();//���߿� �̰ɷ� �����
        }
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(CrashBunpeoFragment, transform.position, Quaternion.identity);
            Parents.IsActivateRPC();
            //Parents.IsActivate();//���߿� �̰ɷ� �����
        }
        gameObject.SetActive(false);
    }

}
