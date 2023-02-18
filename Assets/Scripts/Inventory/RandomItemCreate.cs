using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RandomItemCreate : MonoBehaviour
{

    public Item_ScriptableObject equipment;
    public Item_ScriptableObject potion;
    public Item_ScriptableObject matter;

    public ItemManager manager;

    public Image quickSlot;
    public Item_Potion quickSlotPotion;

    public void CreateItem()
    {
        int num = Random.Range(0, 3);
        int ran;
        switch (num)
        {
            case 0:
                ran = Random.Range(0, equipment.itemDatas.Count);
                manager.NewGetItem(equipment.itemDatas[ran]);
                break;

            case 1:
                ran = Random.Range(0, potion.itemDatas.Count);
                manager.NewGetItem(potion.itemDatas[ran]);
                break;

            case 2:
                ran = Random.Range(0, matter.itemDatas.Count);
                manager.NewGetItem(matter.itemDatas[ran]);
                break;
        }
    }

    private void Start()
    {
        quickSlot.gameObject.BindEvent((PointerEventData) => PotionClickEvent(PointerEventData));
    }

    public void RegisterQuickSlot(Sprite sprite, Item_Potion potion)
    {
        quickSlot.sprite = sprite;
        quickSlotPotion = potion;
    }
    void ReleaseQuickSlot()
    {
        quickSlot.sprite = null;
        quickSlotPotion = null;
    }

    public void PotionClickEvent(PointerEventData click)
    {
        if (quickSlotPotion == null)
        {
            return;
        }

        switch (click.pointerId)
        {
            case -2:
                ReleaseQuickSlot();
                break;

            default:
                quickSlotPotion.UsePotion(1);
                break;
        }
    }
}
