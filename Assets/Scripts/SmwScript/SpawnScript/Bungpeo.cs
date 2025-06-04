using Photon.Pun;
using System.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Bungpeo : EnemyBase
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

    [SerializeField] GameObject CrashBunpeo;

    Terrain terrain;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Manager.Instance.observer.HitPlayer(damage);
            Debug.Log(Manager.Instance.observer.UserPlayer.gamedata.life);

            Vector3 hitPoint = other.ClosestPoint(transform.position);//�浹������ �ִ��� ������

            Vector3 normal = (hitPoint - transform.position).normalized;// ������
            Quaternion rot = Quaternion.LookRotation(normal);// ������

            GameObject inst = Instantiate(CrashBunpeo, hitPoint, rot);


            gameObject.SetActive(false);
        }

    }

    public override void OnEnable()
    {
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();

        myCollider.enabled = true;

        terrain = Terrain.activeTerrain;

        float terrainY = terrain.SampleHeight(transform.position) + transform.localScale.y / 2f;
        transform.position = new Vector3(transform.position.x, terrainY, transform.position.z);

        for (int i = 0; i < Body.Length; i++)
        {
            Body[i].SetActive(true);
        }

        StartCoroutine(WaitAndExplode());
    }

    private IEnumerator WaitAndExplode()
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(explodeStateName))
        {
            yield return null;
        }

        // State�� ����Ǵ� ����, ����� Ŭ������ ��������
        AnimatorClipInfo[] clips = animator.GetCurrentAnimatorClipInfo(0);
        if (clips.Length == 0)
        {
            Debug.LogWarning($"{explodeStateName} Ŭ���� ����");
            yield break;
        }

        float clipLength = clips[0].clip.length;

        yield return new WaitForSeconds(clipLength);

        Explode();
    }


    public override void OnDisable()
    {
        base.OnDisable();
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
            Fragment[i].SetActive(true);
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

    public override void Move(Vector3 direction)
    {
        photonView.RPC("MoveRPC", RpcTarget.All, direction);
    }

    public override void Freeze(Vector3 direction, bool isFreeze)
    {
        photonView.RPC("FreezeRPC", RpcTarget.All, direction);
    }

    public override void CsvEnemyInfo()
    {
        throw new System.NotImplementedException();
    }


    [PunRPC]
    public void MoveRPC(Vector3 direction)
    {
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();

        myCollider.enabled = true;

        terrain = Terrain.activeTerrain;

        float terrainY = terrain.SampleHeight(transform.position) + transform.localScale.y / 2f;
        transform.position = new Vector3(transform.position.x, terrainY, transform.position.z);

        for (int i = 0; i < Body.Length; i++)
        {
            Body[i].SetActive(true);
        }

        StartCoroutine(WaitAndExplode());
    }


    [PunRPC]
    public void FreezeRPC(Vector3 direction, bool isFreeze)
    {

    }

}
