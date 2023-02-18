using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : UI_Base
{

    enum Buttons
    {
        Quest_Button,
        Inventory_Button,
        Skill_Button,
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
    }


    void Start()
    {
        Init();
        GetButton((int)Buttons.Quest_Button).gameObject.BindEvent((PointerEventData) => OpenQuest());
        GetButton((int)Buttons.Inventory_Button).gameObject.BindEvent((PointerEventData) => OpenInventory());
        GetButton((int)Buttons.Skill_Button).gameObject.BindEvent((PointerEventData) => OpenSkill());
        
    }



    UI_Quest quest;
    void OpenQuest()
    {
        if (quest == null)
        {
            quest = Managers.UI.ShowPopupUI<UI_Quest>("Quest");
            FindObjectOfType<QuestManagerEx>().ShowMyQuest();
        }
        else
        {
            Managers.UI.ClosePopupUI(quest);
        }
    }
    public UI_Inventory inventory;
    void OpenInventory()
    {
        if (inventory.gameObject.activeInHierarchy == false)
        {
            inventory.gameObject.SetActive(true);
            //inventory = Managers.UI.ShowPopupUI<UI_Inventory>("Inventory");
        }
        else
        {
            inventory.gameObject.SetActive(false);
            //Managers.UI.ClosePopupUI(inventory);
        }
    }
    UI_Skill skill;
    void OpenSkill()
    {
        if (skill == null)
        {
            skill = Managers.UI.ShowPopupUI<UI_Skill>("Skill");
        }
        else
        {
            Managers.UI.ClosePopupUI(skill);
        }
        
    }

}
