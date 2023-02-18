using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : UI_Base
{
    public Item_ScriptableObject equipment;
    public Item_ScriptableObject potion;
    public Item_ScriptableObject matter;

    public override void Init()
    {
        
    }

    void Start()
    {
        
    }


    public void RandomItemCreate()
    {
        int num = Random.Range(0, 3);
        int ran;
        switch (num)
        {
            case 0:
                ran = Random.Range(0, equipment.itemDatas.Count);
                ItemManager.Instance.NewGetItem(equipment.itemDatas[ran]);
                break;

            case 1:
                ran = Random.Range(0, potion.itemDatas.Count);
                ItemManager.Instance.NewGetItem(potion.itemDatas[ran]);
                break;

            case 2:
                ran = Random.Range(0, matter.itemDatas.Count);
                ItemManager.Instance.NewGetItem(matter.itemDatas[ran]);
                break;
        }
    }
}
