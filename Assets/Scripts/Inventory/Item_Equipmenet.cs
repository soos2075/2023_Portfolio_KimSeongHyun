using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item_Equipmenet : Item
{
    public enum EquipmentState
    {
        Non_Equip,
        Equip,
    }

    public EquipmentState equipmentState;

    public int getNumber;


    public override void Init()
    {
        base.Init();

        Init_ItemType(ItemType.Equipment);
    }


    public override void ClickEvent(PointerEventData click)
    {
        equipmentState = EquipmentState.Equip;
        GetComponentInParent<ItemBox>().SetStateText("E");
        //_stateText.text = "E";

        ItemManager.Instance.Equip(this);

        ItemManager.Instance.nowSelected = this;
    }

    public override string StateTextUpdate()
    {
        if (equipmentState == EquipmentState.Equip)
        {
            return "E";
        }
        else
        {
            return "";
        }
    }

    public void TakeOffEquip()
    {
        equipmentState = EquipmentState.Non_Equip;
        GetComponentInParent<ItemBox>().SetStateText("");
        //_stateText.text = "";
    }
}
