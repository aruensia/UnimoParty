using System.Collections;
using UnityEngine;

public class Bungpeo : MonoBehaviour
{
    public float explosionForce = 20f;
    public float explosionRadius = 20f;
    public float upwardsModifier = 1f;
    public LayerMask explosionMask;

    public GameObject[] Fragment;

    public GameObject explosionFragment;

    public GameObject[] Body;

    Animator animator;

    Collider myCollider;

    private string explodeStateName = "anim_MON003_ready02";

    private Vector3 initialScale;
    private void Awake()
    {
        initialScale = transform.localScale;
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        // �� ����� �κ�: Animator�� Ű������ �������� ���� �������� ���ϵ��� ��Ȱ��ȭ
        animator.enabled = false;

        // �� ����� �κ�: ������Ʈ �������� �׻� �ʱ� ����(initialScale)�� ���� ����
        transform.localScale = initialScale;

        // �� ����� �κ�: Animator ���� ���¸� �ʱ�ȭ
        animator.Rebind();
        animator.Update(0f);

        // �� ����� �κ�: �غ� �������� Animator�� �ٽ� Ȱ��ȭ
        animator.enabled = true;



        myCollider.enabled = true;



        for (int i = 0; i < Fragment.Length; i++)
        {

            if (Fragment[i].GetComponent<Collider>() != null)
            {
                Fragment[i].GetComponent<Collider>().enabled = false;
            }

            if (Fragment[i].GetComponent<Rigidbody>() != null)
            {
                Fragment[i].GetComponent<Rigidbody>().isKinematic = true;
                Fragment[i].GetComponent<Rigidbody>().useGravity = false;
            }
            Fragment[i].transform.localPosition = Vector3.zero;
        }


        for (int i = 0; i < Body.Length; i++)
        {
            Body[i].SetActive(true);
            myCollider.enabled = true;
        }

        StartCoroutine(WaitAndExplode());
    }

    private IEnumerator WaitAndExplode()
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(explodeStateName))
        {
            yield return null;
        }

        // 2) �ش� ����(State)�� ����Ǵ� ����, ����� Ŭ�� ����(�밳 �ϳ�)���� ����(length) ��������
        AnimatorClipInfo[] clips = animator.GetCurrentAnimatorClipInfo(0);
        if (clips.Length == 0)
        {
            Debug.LogWarning($"[{explodeStateName}] ���¿� ����� �ִϸ��̼� Ŭ���� �����ϴ�.");
            yield break;
        }

        float clipLength = clips[0].clip.length;

        yield return new WaitForSeconds(clipLength);

        Explode();
    }


    private void OnDisable()
    {
        for (int i = 0; i < Fragment.Length; i++)
        {

            if (Fragment[i].GetComponent<Collider>() != null)
            {
                Fragment[i].GetComponent<Collider>().enabled = false;
            }

            if (Fragment[i].GetComponent<Rigidbody>() != null)
            {
                Fragment[i].GetComponent<Rigidbody>().isKinematic = true;
                Fragment[i].GetComponent<Rigidbody>().useGravity = false;
            }
            Fragment[i].transform.localPosition = Vector3.zero;
        }
        for (int i = 0; i < Body.Length; i++)
        {
            Body[i].SetActive(true);
        }
    }

    public void Explode()
    {

        Vector3 explosionPosition = transform.position;

        for (int i = 0; i < Fragment.Length; i++)
        {

            if (Fragment[i].GetComponent<Collider>() != null)
            {
                Fragment[i].GetComponent<Collider>().enabled = true;
            }

            if (Fragment[i].GetComponent<Rigidbody>() != null)
            {
                Fragment[i].GetComponent<Rigidbody>().isKinematic = false;
                Fragment[i].GetComponent<Rigidbody>().useGravity = true;
            }
            Fragment[i].transform.localPosition = Vector3.zero;
        }


        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius, explosionMask);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, upwardsModifier, ForceMode.Impulse);
            }
        }

        GameObject inst = Instantiate(explosionFragment, transform.position, Quaternion.identity);

        for (int i = 0; i < Body.Length; i++)
        {
            Body[i].SetActive(false);
            myCollider.enabled = false;
        }


    }

}
