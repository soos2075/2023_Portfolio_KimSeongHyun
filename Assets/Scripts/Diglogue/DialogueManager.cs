using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager
{

    public GameObject DialogueEvent(string dialoge_Name)
    {
        UI_Dialogue dia = Managers.UI.ShowPopupUI<UI_Dialogue>("Dialogue");

        var data = Resources.Load<Dialogue_ScriptableObject>($"Data/Dialogue/{dialoge_Name}");
        if (data == null)
        {
            Debug.Log($"Data/Dialogue/{dialoge_Name} 가 존재하지 않습니다");
        }
        else if (data != null)
        {
            dia.Init_DialogueData(data);
        }

        return dia.gameObject;
    }

}
