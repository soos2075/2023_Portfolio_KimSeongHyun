using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    #region Singleton
    private static ItemManager _instance;
    public static ItemManager Instance { get { Init(); return _instance; } }
    static void Init()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<ItemManager>();
            if (_instance == null)
            {
                var go = new GameObject(name: "@ItemManager");
                _instance = go.AddComponent<ItemManager>();
                DontDestroyOnLoad(go);
            }
        }
    }

    void Start()
    {
        Init();

        InstantiateBox();
    }
    #endregion



    Dictionary<string, Item> item_Dict = new Dictionary<string, Item>();


    public Transform create_Root;

    public Item_Equipmenet nowEquip;
    public Image nowEquipSprite;

    public Item nowSelected;
    GameObject hand;

    public GameObject EquipObject;

    public void SelectItem(Item item)
    {
        nowSelected = item;
        if (hand == null || !hand.activeSelf)
        {
            hand = Managers.Resource.Instantiate("UI/Inventory/Selected");
            hand.transform.SetParent(nowSelected.transform, false);
            hand.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
        else
        {
            hand.transform.SetParent(nowSelected.transform, false);
            hand.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
    }
    public void SellItem()
    {
        if (nowSelected != null)
        {
            Item_Equipmenet eq = nowSelected as Item_Equipmenet;
            if (eq != null)
            {
                eq.TakeOffEquip();
                nowEquip = null;
                nowEquipSprite.sprite = null;
            }
            SubtractItem(nowSelected);
        }
    }


    public void Equip(Item_Equipmenet equip)
    {
        if (equip == nowEquip)
        {
            return;
        }

        if (nowEquip != null)
        {
            nowEquip.TakeOffEquip();
            Managers.Resource.Destroy(EquipObject);
        }
        
        nowEquip = equip;
        nowEquipSprite.sprite = nowEquip.itemSprite;
        
        if (GameObject.FindGameObjectWithTag("EquipSocket") != null)
        {
            EquipObject = Managers.Resource.Instantiate($"Equipment/{equip.item_Name}", GameObject.FindGameObjectWithTag("EquipSocket").transform);
        }
    }


    public int SearchItem(string itemName)
    {
        Item item;
        item_Dict.TryGetValue(itemName, out item);

        if (item == null)
        {
            return 0;
        }
        else
        {
            switch (item.item_Type)
            {
                case Item.ItemType.Potion:
                    Item_Potion potion = (Item_Potion)item;
                    return potion.quantity;

                case Item.ItemType.Matter:
                    Item_Matter matter = (Item_Matter)item;
                    return matter.quantity;

                default:
                    return 0;
            }
        }
    }

    public Item SearchMyItem(string itemName)
    {
        Item item;
        item_Dict.TryGetValue(itemName, out item);

        if (item == null)
        {
            return null;
        }
        else
        {
            return item;
        }
    }


    //? GetItem이랑 RemoveItem 함수 새로만들기(인벤토리 칸 미리 정해져있는 버전으로)

    public List<ItemBox> boxList = new List<ItemBox>();

    void InstantiateBox()
    {
        for (int i = 0; i < 21; i++)
        {
            GameObject go = Managers.Resource.Instantiate("UI/Inventory/ItemBox", create_Root);
            var item = go.GetComponent<ItemBox>();
            item.BoxNumber = i;
            boxList.Add(item);
        }
    }

    void InputToBox(GameObject newItem, Item_ScriptableObject.ItemData data)
    {
        foreach (var obj in boxList)
        {
            if (obj.item == null)
            {
                newItem.transform.parent = obj.transform;
                var item1 = newItem.GetComponent<Item>();
                item1.Init();
                item1.Init_Sprite(data._sprite);
                item1.Init_Item(data._name, data._itemID, data._rarity);
                obj.SetItem(item1);
                return;
            }
        }
        Debug.Log("오류 : 빈공간이 없음");
    }

    public void NewGetItem(Item_ScriptableObject.ItemData data)
    {
        Item tempItem;
        item_Dict.TryGetValue(data._name, out tempItem);

        switch (data._type)
        {
            case Item.ItemType.Equipment:
                item_Dict.TryGetValue(data._name + 0, out tempItem);
                break;

            default:
                item_Dict.TryGetValue(data._name, out tempItem);
                break;
        }

        if (tempItem == null)
        {
            switch (data._type)
            {
                case Item.ItemType.Equipment:
                    GameObject go = new GameObject(data._name);
                    Item_Equipmenet item1 = go.AddComponent<Item_Equipmenet>();
                    InputToBox(go, data);
                    item_Dict.Add(data._name + 0, item1);
                    break;

                case Item.ItemType.Potion:
                    GameObject go2 = new GameObject(data._name);
                    Item_Potion item2 = go2.AddComponent<Item_Potion>();
                    InputToBox(go2, data);
                    item2.GetPotion();
                    item_Dict.Add(data._name, item2);
                    break;

                case Item.ItemType.Matter:
                    GameObject go3 = new GameObject(data._name);
                    Item_Matter item3 = go3.AddComponent<Item_Matter>();
                    InputToBox(go3, data);
                    item3.GetMatter();
                    item_Dict.Add(data._name, item3);
                    break;
            }
        }
        else
        {
            switch (data._type)
            {
                case Item.ItemType.Equipment:
                    GameObject go = new GameObject(data._name);
                    Item_Equipmenet equip = go.AddComponent<Item_Equipmenet>();
                    InputToBox(go, data);

                    Item searchItem;
                    int count = 0;
                    string _name;
                    while (true)
                    {
                        count++;
                        _name = data._name + count.ToString();
                        item_Dict.TryGetValue(_name, out searchItem);
                        if (searchItem == null)
                        {
                            break;
                        }
                    }
                    equip.getNumber = count;
                    item_Dict.Add(_name, (Item)equip);
                    break;

                case Item.ItemType.Potion:
                    Item_Potion potion = (Item_Potion)tempItem;
                    potion.GetPotion();
                    break;

                case Item.ItemType.Matter:
                    Item_Matter matter = (Item_Matter)tempItem;
                    matter.GetMatter();
                    break;
            }
        }
    }



    [System.Obsolete]
    public void GetItem(Item_ScriptableObject.ItemData item)
    {
        Item tempItem;
        item_Dict.TryGetValue(item._name, out tempItem);

        switch (item._type)
        {
            case Item.ItemType.Equipment:
                item_Dict.TryGetValue(item._name + 0, out tempItem);
                break;

            default:
                item_Dict.TryGetValue(item._name, out tempItem);
                break;
        }

        if (tempItem == null)
        {
            switch (item._type)
            {
                case Item.ItemType.Equipment:
                    GameObject go = Managers.Resource.Instantiate("UI/Inventory/Item", create_Root);
                    Item_Equipmenet item1 = go.AddComponent<Item_Equipmenet>();
                    item1.Init();
                    item1.Init_Sprite(item._sprite);
                    item1.Init_Item(item._name, item._itemID, item._rarity);
                    item_Dict.Add(item._name + 0, item1);
                    break;

                case Item.ItemType.Potion:
                    GameObject go2 = Managers.Resource.Instantiate("UI/Inventory/Item", create_Root);
                    Item_Potion item2 = go2.AddComponent<Item_Potion>();
                    item2.Init();
                    item2.Init_Sprite(item._sprite);
                    item2.Init_Item(item._name, item._itemID, item._rarity);
                    item2.GetPotion();
                    item_Dict.Add(item._name, item2);
                    break;

                case Item.ItemType.Matter:
                    GameObject go3 = Managers.Resource.Instantiate("UI/Inventory/Item", create_Root);
                    Item_Matter item3 = go3.AddComponent<Item_Matter>();
                    item3.Init();
                    item3.Init_Sprite(item._sprite);
                    item3.Init_Item(item._name, item._itemID, item._rarity);
                    item3.GetMatter();
                    item_Dict.Add(item._name, item3);
                    break;
            }
        }
        else
        {
            switch (item._type)
            {
                case Item.ItemType.Equipment:
                    GameObject go = Managers.Resource.Instantiate("UI/Inventory/Item", create_Root);
                    Item_Equipmenet equip = go.AddComponent<Item_Equipmenet>();
                    equip.Init();
                    equip.Init_Sprite(item._sprite);
                    equip.Init_Item(item._name, item._itemID, item._rarity);

                    Item searchItem;
                    int count = 0;
                    string _name;
                    while (true)
                    {
                        count++;
                        _name = item._name + count.ToString();
                        item_Dict.TryGetValue(_name, out searchItem);
                        if (searchItem == null)
                        {
                            break;
                        }
                    }

                    equip.getNumber = count;
                    item_Dict.Add(_name, (Item)equip);
                    break;

                case Item.ItemType.Potion:
                    Item_Potion potion = (Item_Potion)tempItem;
                    potion.GetPotion();
                    break;

                case Item.ItemType.Matter:
                    Item_Matter matter = (Item_Matter)tempItem;
                    matter.GetMatter();
                    break;
            }
        }
    }

    public void SubtractItem(Item item)
    {
        Item tempItem;

        switch (item.item_Type)
        {
            case Item.ItemType.Equipment:
                Item_Equipmenet equip = (Item_Equipmenet)item;
                string _name = item.item_Name + equip.getNumber.ToString();
                item_Dict.TryGetValue(_name, out tempItem);
                break;

            default:
                item_Dict.TryGetValue(item.item_Name, out tempItem);
                break;
        }

        if (tempItem == null)
        {
            Debug.Log("삭제할 아이템이 없음");
            return;
        }
        else
        {
            switch (item.item_Type)
            {
                case Item.ItemType.Equipment:
                    Item_Equipmenet equip = (Item_Equipmenet)tempItem;
                    string _name = item.item_Name + equip.getNumber.ToString();
                    item_Dict.Remove(_name);
                    item.gameObject.GetComponentInParent<ItemBox>().BoxInitialize();
                    Managers.Resource.Destroy(item.gameObject);
                    break;

                case Item.ItemType.Potion:
                    Item_Potion potion = (Item_Potion)tempItem;
                    item_Dict.Remove(item.item_Name);
                    item.gameObject.GetComponentInParent<ItemBox>().BoxInitialize();
                    Managers.Resource.Destroy(item.gameObject);
                    break;

                case Item.ItemType.Matter:
                    Item_Matter matter = (Item_Matter)tempItem;
                    item_Dict.Remove(item.item_Name);
                    item.gameObject.GetComponentInParent<ItemBox>().BoxInitialize();
                    Managers.Resource.Destroy(item.gameObject);
                    break;
            }
        }
    }

    public enum ComparerType
    {
        Name,
        Rarity,
        ItemType,
        ItemID,
    }
    public class ItemListComparer : IComparer<Item> //? 아이템 정렬용 비교자
    {
        ComparerType ComparerType;

        public ItemListComparer(ComparerType comparerType)
        {
            ComparerType = comparerType;
        }

        public int Compare(Item x, Item y)
        {
            switch (ComparerType)
            {
                case ComparerType.Name:
                    return string.Compare(x.item_Name, y.item_Name);

                case ComparerType.Rarity: //? 내림차순 (숫자가 클수록 우선순위)
                    if (x.rarity > y.rarity)
                    {
                        return -1;
                    }
                    else if (x.rarity < y.rarity)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                case ComparerType.ItemType: 
                    if (x.itemID > y.itemID)
                    {
                        return -1;
                    }
                    else if (x.itemID < y.itemID)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                case ComparerType.ItemID: //? 오름차순
                    if (x.itemID < y.itemID)
                    {
                        return -1;
                    }
                    else if (x.itemID > y.itemID)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                default:
                    if (x.itemID < y.itemID)
                    {
                        return -1;
                    }
                    else if (x.itemID > y.itemID)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
            }
        }
    } 


    public List<Item> SortList(List<Item> newList, ItemListComparer itemListComparer)
    {
        if (newList == null)
        {
            return null;
        }

        newList.Sort(itemListComparer);

        return newList;
    }
    private List<Item> MakeNewItemList(Item.ItemType type)
    {
        List<Item> newItemList = new List<Item>();

        switch (type)
        {
            case Item.ItemType.Equipment:
                foreach (var item in item_Dict)
                {
                    if (item.Value.item_Type == Item.ItemType.Equipment)
                    {
                        newItemList.Add(item.Value);
                    }
                }
                break;
            case Item.ItemType.Potion:
                foreach (var item in item_Dict)
                {
                    if (item.Value.item_Type == Item.ItemType.Potion)
                    {
                        newItemList.Add(item.Value);
                    }
                }
                break;
            case Item.ItemType.Matter:
                foreach (var item in item_Dict)
                {
                    if (item.Value.item_Type == Item.ItemType.Matter)
                    {
                        newItemList.Add(item.Value);
                    }
                }
                break;
        }

        return newItemList;
    }




    //? Obsolete
    /*
    public void ShowAll()
    {
        List<Item> newItemList = new List<Item>();
        foreach (var item in item_Dict)
        {
            newItemList.Add(item.Value);
        }

        var list = SortList(newItemList, new ItemListComparer(ComparerType.ItemType));
        ShowList(list);
    }
    public void ShowEquipment()
    {
        var list = SortList(MakeNewItemList(Item.ItemType.Equipment), new ItemListComparer(ComparerType.Rarity));
        ShowList(list);
    }
    public void ShowPotion()
    {
        var list = SortList(MakeNewItemList(Item.ItemType.Potion), new ItemListComparer(ComparerType.ItemID));
        ShowList(list);
    }
    public void ShowMatter()
    {
        var list = SortList(MakeNewItemList(Item.ItemType.Matter), new ItemListComparer(ComparerType.ItemID));
        ShowList(list);
    }
    void ShowList(List<Item> items)
    {
        for (int i = 0; i < create_Root.childCount; i++)
        {
            create_Root.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < items.Count; i++)
        {
            items[i].gameObject.SetActive(true);
            items[i].transform.SetSiblingIndex(i);
        }
    }
    */


    List<Item> GetCurrentItemList()
    {
        List<Item> newItemList = new List<Item>();
        for (int i = 0; i < create_Root.childCount; i++)
        {
            var bbb = create_Root.GetChild(i).gameObject.GetComponent<ItemBox>();
            if (bbb.item != null)
            {
                newItemList.Add(bbb.item);
            }
        }
        return newItemList;
    }
    public void SortDefault()
    {
        var list = SortList(GetCurrentItemList(), new ItemListComparer(ComparerType.ItemType));
        ShowListEx(list);
    }

    public void SortName()
    {
        var list = SortList(GetCurrentItemList(), new ItemListComparer(ComparerType.Name));
        ShowListEx(list);
    }
    public void SortRarity()
    {
        var list = SortList(GetCurrentItemList(), new ItemListComparer(ComparerType.Rarity));
        ShowListEx(list);
    }
    public void SortID()
    {
        var list = SortList(GetCurrentItemList(), new ItemListComparer(ComparerType.ItemID));
        ShowListEx(list);
    }


    void ShowListEx(List<Item> items)
    {
        for (int i = 0; i < create_Root.childCount; i++)
        {
            create_Root.GetChild(i).gameObject.GetComponent<ItemBox>().BoxInitialize();
        }

        for (int i = 0; i < items.Count; i++)
        {
            items[i].transform.parent = create_Root.GetChild(i);
            create_Root.GetChild(i).gameObject.GetComponent<ItemBox>().SetItem(items[i]);
        }
    }

}
