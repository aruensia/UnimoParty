using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    Dictionary<string, List<InterfaceMethod.TableData>> dataDic;
    ItemData selectedItem;
    public Sprite shipImage;
    int selectMenuNum;

    private void Start()
    {
        GetItemData();
    }

    void GetItemData()
    {
        var tempdatas = Manager.Instance.dataLoader.data;
        dataDic = tempdatas;
    }

    void ShowShopItemList()
    {
        switch(selectMenuNum)
        {
            case 0:
              
                for (int i = 0; i < dataDic["Enemy"].Count; i++)
                {
                    var temp = dataDic["Enemy"][i];

                }
                break;

            case 1:

                break;
            
            case 2:

                break;

        }
    }

    void SelectItem()
    {
        // ���� Ʈ���ŵ� ��ư�̵� ���� ������ ��, ��ȯ�Ǵ� �ڷḦ ����. 
    }

    void ButItemButton()
    {
        Manager.Instance.observer.BuyItem(selectedItem);
    }

}
