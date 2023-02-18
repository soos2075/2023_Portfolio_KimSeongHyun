using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PotionSlot : UI_Base
{

    public Item_Potion potion;
    Image potionSprite;
    public override void Init()
    {
        potionSprite = transform.GetChild(0).GetComponent<Image>();
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    public void RegisterPotion(Item_Potion po)
    {
        if (potion)
        {
            RemovePotion();
        }
        potion = po;
        potionSprite.sprite = potion.itemSprite;
        potionSprite.color = Color.white;
    }

    public void QuickPotionUse()
    {
        if (potion)
        {
            potion.UsePotion(1);
        }
    }
    
    public void RemovePotion()
    {
        potionSprite.color = Color.clear;
        potionSprite.sprite = null;
        potion.quickSlotRegister = null;
        potion = null;
    }


}
