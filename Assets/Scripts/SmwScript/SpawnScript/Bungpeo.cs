using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bungpeo : MonoBehaviour
{
    public float explosionForce = 500f;      // ���� ��
    public float explosionRadius = 5f;       // ���� �ݰ�
    public float upwardsModifier = 1f;       // ���� Ƣ������� �ϴ� ��
    public LayerMask explosionMask;          // ���� ��� ��� ���� ����

    public void Explode()
    {
        // ���� �߽� ��ġ
        Vector3 explosionPosition = transform.position;

        // �ش� �ݰ� ���� �ݶ��̴� �˻�
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius, explosionMask);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, upwardsModifier, ForceMode.Impulse);
            }
        }
    }

}
