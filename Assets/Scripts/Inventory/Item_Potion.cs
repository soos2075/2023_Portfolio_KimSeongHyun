using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item_Potion : Item
{
    public int quantity = 0;

    public UI_PotionSlot quickSlotRegister;

    public override void Init()
    {
        base.Init();

        Init_ItemType(ItemType.Potion);
    }


    public override void ClickEvent(PointerEventData click)
    {
        switch (click.pointerId)
        {
            case -2:
                SaveQuickSlot(this);
                break;

            default:
                UsePotion(1);
                ItemManager.Instance.nowSelected = this;
                break;
        }
    }
    public override string StateTextUpdate()
    {
        return quantity.ToString();
    }

    public void SaveQuickSlot(Item_Potion potion)
    {
        //Debug.Log($"{item_Name} 퀵슬롯 등록");
        foreach (var item in FindObjectsOfType<UI_PotionSlot>())
        {
            if (!item.potion)
            {
                QuickSlotOverwrite(item);
                return;
            }
        }
        //Debug.Log("슬롯없음");
    }
    void QuickSlotCheck()
    {
        if (quickSlotRegister)
        {
            quickSlotRegister.RemovePotion();
        }
    }
    public void QuickSlotOverwrite(UI_PotionSlot slot)
    {
        QuickSlotCheck();
        slot.RegisterPotion(this);
        quickSlotRegister = slot;
    }




    public void UsePotion(int value)
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
        //Debug.Log($"{item_Name} {value} 개 사용 , 남은 개수 {quantity}");
        PotionEffect(item_Name);

        if (quantity == 0)
        {
            if (quickSlotRegister)
            {
                quickSlotRegister.RemovePotion();
            }
            ItemManager.Instance.SubtractItem(this);
        }
    }

    public void GetPotion()
    {
        quantity++;
        if (gameObject.activeInHierarchy)
        {
            GetComponentInParent<ItemBox>().SetStateText(quantity.ToString());
        }
        //_stateText.text = quantity.ToString();
    }




    void PotionEffect(string str)
    {
        PlayerStat player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>();
        switch (str)
        {
            case "빨강포션":
                player.HP += 50;
                break;
            case "파랑포션":
                player.MP += 30;
                break;
            case "엘릭서":
                player.HP += 100;
                player.MP += 50;
                break;
        }
    }

    
}
