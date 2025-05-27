using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bungpeo : MonoBehaviour
{
    float BombTimer;

    [SerializeField] Animator animator;
    [SerializeField] string stateName;
    [SerializeField] AnimationClip clip;      //�ش� ������Ʈ�� ����� Ŭ��


    // stateName ���� �ִϸ��̼��� duration�ʿ� �� ���� ���
    public void PlayTimed(float duration)
    {
        // 1) Ŭ�� ���� ���ϰ�
        float clipLength = clip.length;
        // 2) ��� �ӵ� ��� (�ӵ��� �������� ������)
        float speed = clipLength / duration;
        // 3) Animator ��ü �ӵ��� ����
        animator.speed = speed;
        // 4) ������Ʈ ��� (0�� ���̾�, 0�����ӿ���)
        animator.Play(stateName, 0, 0f);
    }



    IEnumerator Bomb()
    {


        



        yield return null;
    }

}
