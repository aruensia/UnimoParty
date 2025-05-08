using UnityEngine;

public class HorizonView : MonoBehaviour
{
    public Transform target; // �ٶ� �߽���
    public float height = 5f;

    void Start()
    {
        transform.position = new Vector3(0, height, -10);
        transform.LookAt(target);
    }
}