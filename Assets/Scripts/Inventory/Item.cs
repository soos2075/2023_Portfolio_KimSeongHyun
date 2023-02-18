using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : UI_Base
{
    public enum ItemType
    {
        Equipment,
        Potion,
        Matter,
    }


    public ItemType item_Type;
    public string item_Name;

    public int itemID;
    public int rarity;

    public Sprite itemSprite;



    public override void Init()
    {

    }
    void Start()
    {

    }

    void Update()
    {
        
    }

    public virtual void ClickEvent(PointerEventData click)
    {
        Debug.Log($"클릭이벤트 발생중 : {item_Name}");
    }
    public virtual string StateTextUpdate()
    {
        return "";
    }



    public void Init_ItemType(ItemType _type)
    {
        item_Type = _type;
    }
    public void Init_Sprite(Sprite _sprite)
    {
        itemSprite = _sprite;
        //_Image.sprite = _sprite;
    }

    void Init_ItemName(string _name)
    {
        item_Name = _name;
    }
    void Init_ItemID(int _id)
    {
        itemID = _id;
    }
    void Init_ItemRarity(int _rarity)
    {
        rarity = _rarity;
    }
    public void Init_Item(string _name, int _id, int _rarity)
    {
        Init_ItemName(_name);
        Init_ItemID(_id);
        Init_ItemRarity(_rarity);
    }





}
