using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestObjectEx : MonoBehaviour
{
    public Quest_SOEx SO_data;

    public Image QuestImage;
    public Text QuestText;
    public Text BoolText;
    public Text QuantityText;
    public Text QuestTitle;

    public void Initialize_QuestData(Quest_SOEx so)
    {
        SO_data = so;
        SO_data.data = FindObjectOfType<QuestManagerEx>().GetQuestData(SO_data.id);

        QuestTextUpdate();
    }
    private void Awake()
    {
        QuestImage = transform.GetChild(0).GetComponent<Image>();
        QuestText = transform.GetChild(1).GetComponent<Text>();
        BoolText = transform.GetChild(2).GetComponent<Text>();
        QuantityText = transform.GetChild(3).GetComponent<Text>();
        QuestTitle = transform.GetChild(4).GetComponent<Text>();
    }
    void Start()
    {
        //Temp();

        if (SO_data.data.type != Define.QuestType.Gather)
        {
            QuantityText.gameObject.SetActive(false);
        }
    }
    [System.Obsolete]
    void Temp()
    {
        SO_data.data = FindObjectOfType<QuestManagerEx>().GetQuestData(SO_data.id);
        QuestTextUpdate();
    }

    void Update()
    {
        if (SO_data == null)
        {
            return;
        }

        if (SO_data.data.GetCheck())
        {
            BoolText.text = "O";
        }
        else
        {
            BoolText.text = "X";
        }

        if (SO_data.data.type == Define.QuestType.Gather)
        {
            QuantityUpdate();
        }
    }

    public void QuestTextUpdate()
    {
        QuestText.text = SO_data.contents;
        QuestTitle.text = SO_data.title;
    }

    public void QuantityUpdate()
    {
        QuantityText.text = $"({SO_data.data.GetCurrently()} / {SO_data.data.GetRequire()})";
    }

}
