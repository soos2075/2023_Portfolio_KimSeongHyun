using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemBox : MonoBehaviour
{
    public int BoxNumber { get; set; }

    public Item item;

    public Image itemSprite;
    Text itemName;
    Text itemState;

    private void Awake()
    {
        itemSprite = transform.GetChild(0).GetComponent<Image>();
        itemName = transform.GetChild(1).GetComponent<Text>();
        itemState = transform.GetChild(2).GetComponent<Text>();

        itemSprite.color = Color.clear;
        itemSprite.sprite = null;
        itemName.text = "";
        itemState.text = "";
    }
    private void OnEnable()
    {
        if (item)
        {
            ItemDataUpdate();
        }
    }

    void Start()
    {
        gameObject.BindEvent((PointerData) => DragBegin(PointerData), Define.UIEvent.DragBegin);
        gameObject.BindEvent((PointerData) => Drag(PointerData), Define.UIEvent.Drag);
        gameObject.BindEvent((PointerData) => DragEnd(PointerData), Define.UIEvent.DragEnd);
    }

    void Update()
    {

    }


    GameObject temp;
    Vector3 temp_pos;
    void DragBegin(PointerEventData pointer)
    {
        if (item == null) return;
        //Debug.Log("드래그시작");
        temp = itemSprite.gameObject;
        temp_pos = itemSprite.transform.position;
    }
    void Drag(PointerEventData pointer)
    {
        if (item == null) return;
        if (temp != null)
        {
            temp.transform.position = pointer.position;
        }
    }
    void DragEnd(PointerEventData pointer)
    {
        if (item == null) return;
        //Debug.Log("드래그종료");
        if (temp != null)
        {
            temp.transform.position = temp_pos;
        }

        var cast = pointer.pointerCurrentRaycast.gameObject;
        if (cast != null && cast.GetComponent<ItemBox>()) //? 인벤토리 아이템 스위칭
        {
            ChangeBox(item, cast.GetComponent<ItemBox>());
        }
        else if(cast != null && cast.GetComponent<UI_PotionSlot>()) //? 포션 퀵슬롯 등록
        {
            if (item.item_Type == Item.ItemType.Potion)
            {
                var slot = cast.GetComponent<UI_PotionSlot>();
                Item_Potion potion = (Item_Potion)item;
                potion.QuickSlotOverwrite(slot);
            }
        }
    }


    public void ChangeBox(Item currentItem, ItemBox box)
    {
        var boxItem = box.item;


        box.SetItem(currentItem);
        currentItem.transform.parent = box.transform;

        if (boxItem != null)
        {
            SetItem(boxItem);
            boxItem.transform.parent = transform;
        }
        else
        {
            BoxInitialize();
        }
    }

    public void SetItem(Item _item)
    {
        item = _item;
        ItemDataUpdate();
        gameObject.EscapeEvent(Define.UIEvent.Click);
        gameObject.BindEvent((PointerData) => _item.ClickEvent(PointerData));
    }

    void ItemDataUpdate()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        itemSprite.color = Color.white;
        itemSprite.sprite = item.itemSprite;
        itemName.text = item.item_Name;
        itemState.text = item.StateTextUpdate();
    }

    public void SetStateText(string str)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        itemState.text = str;
    }

    public void BoxInitialize()
    {
        item = null;

        itemSprite.color = Color.clear;
        itemSprite.sprite = null;
        itemName.text = "";
        itemState.text = "";
        gameObject.EscapeEvent(Define.UIEvent.Click);
    }
}
