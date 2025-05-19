using UnityEngine;
using UnityEngine.UI;

public class CanvasToggleButton : MonoBehaviour
{
    /// <summary>
    /// ����� �׳� ĵ���� ��Ȱ��ȭ �ϴ� Ŭ���� 
    /// </summary>
    [Header("���� ���� ĵ���� ������Ʈ")]
    public GameObject targetCanvas;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ToggleCanvasOff);
    }

    void ToggleCanvasOff()
    {
        if (targetCanvas != null)
            targetCanvas.SetActive(false);
    }


}
