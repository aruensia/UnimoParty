using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//flower�� Ǯ������
//���� ��ȣ�ۿ��Ҷ� flower1 ��� �������� �߰���
//�ٸ��÷��̾ flower1�� ��� ��Ȱ��ȭ��
//���� ��¼�ٰ� flower1�� ��ȣ�ۿ��ϸ� �ǵ�ó�� �ȵ�
public class PlayerHarvestData : MonoBehaviour
{
    // Flower ä�� ���൵
    private Dictionary<Flower, float> flowerProgress = new Dictionary<Flower, float>();

    public float GetProgress(Flower flower)
    {
        if (flowerProgress.TryGetValue(flower, out float value))
            return value;
        return 0f;
    }

    public void SetProgress(Flower flower, float progress)
    {
        flowerProgress[flower] = progress;
    }
}
