using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LifeUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textmeshpro;

    public void Start()
    {
        Manager.Instance.observer.OnGameDataChange += OnChangeLife;
        //textmeshpro.text = "��x 100";
        Debug.Log("����� �ؽ�Ʈ: " + textmeshpro.text);
        DefaultSeting();
    }


    void OnChangeLife(DataCenter damage)
    {
        textmeshpro.text = $"x {damage.life}";
    }

    void DefaultSeting()
    {
        
    }
}
