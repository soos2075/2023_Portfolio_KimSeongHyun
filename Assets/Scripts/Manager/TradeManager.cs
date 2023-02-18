using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeManager
{
    //? 얘가 아이템 검색기능도 겸하면 좋을듯. 아이템 ID만 넘겨주면 알아서 데이터 찾아서 이벤트 연결해주는걸로


    public Item_ScriptableObject equipment;
    public Item_ScriptableObject potion;
    public Item_ScriptableObject matter;


    public void Init()
    {
        equipment = Resources.Load<Item_ScriptableObject>("Data/Item/Equipment");
        potion = Resources.Load<Item_ScriptableObject>("Data/Item/Potion");
        matter = Resources.Load<Item_ScriptableObject>("Data/Item/Matter");
    }



    private Item_ScriptableObject.ItemData SearchID(int id)
    {
        foreach (var item in equipment.itemDatas)
        {
            if (item._itemID == id)
            {
                return item;
            }
        }
        foreach (var item in potion.itemDatas)
        {
            if (item._itemID == id)
            {
                return item;
            }
        }
        foreach (var item in matter.itemDatas)
        {
            if (item._itemID == id)
            {
                return item;
            }
        }
        Debug.Log($"{id} : 아이템이 존재하지 않습니다");
        return null;
    }





    public void AddItemEvent(int itemID)
    {
        Item_ScriptableObject.ItemData data = (SearchID(itemID));
        if (data != null)
        {
            AddItemEvent(data);
        }
    }
    public void AddItemEvent(int itemID, int quantity)
    {
        Item_ScriptableObject.ItemData data = (SearchID(itemID));
        if (data != null)
        {
            for (int i = 0; i < quantity; i++)
            {
                AddItemEvent(data);
            }
        }
    }

    public void AddItemEvent(Item_ScriptableObject.ItemData data)
    {
        ItemManager.Instance.NewGetItem(data);
    }



    

    public void SubtractItemEvent(int id)
    {
        var data = SearchID(id);

        var myItem = ItemManager.Instance.SearchMyItem(data._name);

        ItemManager.Instance.SubtractItem(myItem);
    }
    public void SubtractItemEvent(int id, int quantity)
    {
        var data = SearchID(id);
        var myItem = ItemManager.Instance.SearchMyItem(data._name);

        SubtractItemEvent(myItem, quantity);
    }


    public void SubtractItemEvent(Item item)
    {
        ItemManager.Instance.SubtractItem(item);
    }
    public void SubtractItemEvent(Item item, int quantity)
    {
        switch (item.item_Type)
        {
            case Item.ItemType.Equipment:
                //? int번 반복삭제
                break;

            case Item.ItemType.Potion:
                Item_Potion potion = item as Item_Potion;
                if (potion != null)
                {
                    potion.UsePotion(quantity);
                }
                break;

            case Item.ItemType.Matter:
                Item_Matter matter = item as Item_Matter;
                if (matter != null)
                {
                    matter.UseMatter(quantity);
                }
                break;
        }
    }


    public void WeightedRandomDrop(ItemWeighted[] dropList)
    {
        float randomPicker = Random.Range(0, 1f);
        int Pick = -1;

        int weightAll = 0;
        List<float> probabilityList = new List<float>();

        foreach (var item in dropList)
        {
            weightAll += item.Weight;
        }

        foreach (var item in dropList)
        {
            probabilityList.Add((float)item.Weight / (float)weightAll);
        }


        float accumulate = 0;

        for (int i = 0; i < probabilityList.Count; i++)
        {
            accumulate += probabilityList[i];
            if (randomPicker < accumulate)
            {
                Pick = i;
                break;
            }
        }

        Debug.Log($"{dropList[Pick].ID} 가 드롭됐다!");
        if (dropList[Pick].ID <= 0)
        {
            Debug.Log($"꽝!");
        }
        else
        {
            AddItemEvent(dropList[Pick].ID);
        }
    }



    public class ItemWeighted
    {
        public int ID { get; set; }
        public int Weight { get; set; }

        public ItemWeighted(int id, int weight)
        {
            ID = id;
            Weight = weight;
        }
    }
}
