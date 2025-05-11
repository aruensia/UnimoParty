using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeadDash : MonoBehaviour
{
    [Header("XR Origin")]
    public Transform xrOrigin; // XR Origin�� �ν����Ϳ��� �Ҵ�

    [Header("Dash Settings")]
    public float dashAngle = 30f;           // Xȸ�� Ʈ���� ����
    public float dashDistance = 2f;         // �뽬 �Ÿ�
    public float dashCooldown = 5f;         // ��Ÿ�� (��)
    public float dashDuration = 0.5f;       // �뽬 ���� �ð� (��)

    [Header("Dash Events")]
    public UnityEvent onDashForward;
    public UnityEvent onDashBackward;
    public UnityEvent onDashReady;
    public UnityEvent onDashBlocked;

    private float lastDashTime = -999f;     // ������ �뽬 �ð�
    private bool isDashing = false;         // �ߺ� ����

    void Update()
    {
        if (isDashing || xrOrigin == null)
            return;

        float xRot = transform.eulerAngles.x;

        // 360�� �� -180 ~ 180 ������ ����
        if (xRot > 180f) xRot -= 360f;

        // ȸ�� ���� ���� ���� ��
        if (Mathf.Abs(xRot) >= dashAngle)
        {
            if (Time.time - lastDashTime >= dashCooldown)
            {
                onDashReady?.Invoke();

                if (xRot >= dashAngle)
                {
                    StartCoroutine(SmoothDash(xrOrigin.forward));
                    onDashForward?.Invoke();
                }
                else if (xRot <= -dashAngle)
                {
                    StartCoroutine(SmoothDash(-xrOrigin.forward));
                    onDashBackward?.Invoke();
                }

                lastDashTime = Time.time;
            }
            else
            {
                onDashBlocked?.Invoke();
            }
        }
    }

   

    public void DashForward()
    {
        if (!isDashing && xrOrigin != null)
        {
            Debug.Log("������ �뽬");
            StartCoroutine(SmoothDash(xrOrigin.forward));
            lastDashTime = Time.time;
        }
    }

    public void DashBackward()
    {
        if (!isDashing && xrOrigin != null)
        {
            Debug.Log("�ڷ� �뽬");
            StartCoroutine(SmoothDash(-xrOrigin.forward));
            lastDashTime = Time.time;
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

        xrOrigin.position = endPos; // ��ġ ����
        isDashing = false;
    }
}
