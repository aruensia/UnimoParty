using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class HeadDash : MonoBehaviourPunCallbacks
{
    [Header("XR Origin")]
    public Transform xrOrigin;            // XR Origin ������Ʈ
    public Transform spaceShip;           // ���� ����

    [Header("Dash Settings")]
    public float dashAngle = 30f;         // ȸ�� Ʈ���� ���� (X/Z)
    public float dashDistance = 7f;       // ��� �Ÿ�
    public float dashCooldown = 5f;       // ��Ÿ��
    public float dashDuration = 0.5f;     // ��� ���� �ð�
       

    private float lastDashTime = -999f;
    private bool isDashing = false;


    void Update()
    {
        if(photonView.IsMine)
        {
            float xRot = transform.eulerAngles.x;
            float zRot = transform.eulerAngles.z;

            // ���Ϸ� ���� ���� (-180 ~ 180)
            if (xRot > 180f) xRot -= 360f;
            if (zRot > 180f) zRot -= 360f;

            bool canDash = Time.time - lastDashTime >= dashCooldown;

            // X��: �յ� ���
            if (canDash)
            {
                if (xRot >= dashAngle)
                {
                    StartCoroutine(SmoothDash(spaceShip.forward));
                    lastDashTime = Time.time;
                }
                else if (xRot <= -dashAngle)
                {
                    StartCoroutine(SmoothDash(-spaceShip.forward));
                    lastDashTime = Time.time;
                }
                else if (zRot >= dashAngle)
                {
                    StartCoroutine(SmoothDash(-spaceShip.right));
                    lastDashTime = Time.time;
                }
                else if (zRot <= -dashAngle)
                {
                    StartCoroutine(SmoothDash(spaceShip.right));
                    lastDashTime = Time.time;
                }
            }
        }
        
    }

    private IEnumerator SmoothDash(Vector3 direction)
    {
        isDashing = true;

        Vector3 startPos = xrOrigin.position;
        Vector3 endPos = startPos + direction.normalized * dashDistance;

        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            float t = elapsed / dashDuration;
            xrOrigin.position = Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        xrOrigin.position = endPos;
        isDashing = false;
    }

}
