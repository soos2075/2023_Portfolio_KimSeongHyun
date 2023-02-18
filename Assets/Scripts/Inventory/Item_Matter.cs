using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item_Matter : Item
{
    public int quantity = 0;

    public override void Init()
    {
        base.Init();

        Init_ItemType(ItemType.Matter);
    }


    public override void ClickEvent(PointerEventData click)
    {
        switch (click.pointerId)
        {
            default:
                ItemManager.Instance.nowSelected = this;
                break;
        }
    }
    public override string StateTextUpdate()
    {
        return quantity.ToString();
    }

    public void UseMatter()
    {
        UseMatter(1);
    }
    public void UseMatter(int value)
    {
        if ((quantity - value) < 0)
        {
            Debug.Log("개수부족 / 실행불가");
            return;
        }
        quantity -= value;
        if (gameObject.activeInHierarchy)
        {
            GetComponentInParent<ItemBox>().SetStateText(quantity.ToString());
        }
        //_stateText.text = quantity.ToString();
        Debug.Log($"{item_Name} {value} 개 사용 , 남은 개수 {quantity}");

        if (quantity == 0)
        {
            ItemManager.Instance.SubtractItem(this);
        }
    }

    public void GetMatter()
    {
        GetMatter(1);
    }
    public void GetMatter(int value)
    {
        quantity += value;
        if (gameObject.activeInHierarchy)
        {
            GetComponentInParent<ItemBox>().SetStateText(quantity.ToString());
        }
    }





}
