using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("�� ������")]
    [SerializeField] GameObject riBeePrefab;
    [SerializeField] GameObject catPrefab;
    
    [Header("���ּ� ������")]
    [SerializeField] GameObject beeSpaceShip;
    [SerializeField] GameObject boxSpaceShip;

    [Header("��ũ�� ���")]
    [SerializeField] GameObject[] scrollVeiw;

    [Header("��� ��")]
    [SerializeField] Toggle[] viewToggles;

    private void Start()
    {
        for (int i = 0; i < viewToggles.Length; i++)
        {
            if (viewToggles[i].isOn)
            {
                ViewActive(i);
                break;
            }
        }
    }

    public void ViewActive(int activeIndex)
    {
        for (int i = 0; i < scrollVeiw.Length; i++)
        {
            scrollVeiw[i].SetActive(i == activeIndex);
        }
    }

}
