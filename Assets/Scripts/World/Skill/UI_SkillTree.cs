using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTree : UI_Base
{
    enum Skill
    {
        SkillPoint,
        Fireball,
        Earthquake,
    }

    PlayerStat playerStat;

    public override void Init()
    {
        Bind<GameObject>(typeof(Skill));
        playerStat = FindObjectOfType<PlayerStat>();

        AddEvent_Skill(Skill.Fireball, playerStat.Fireball, "Fireball");
        AddEvent_Skill(Skill.Earthquake, playerStat.Earthquake, "Earthquake");
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        GetObject((int)Skill.SkillPoint).GetComponent<Text>().text = $"스킬포인트 : {playerStat.SkillPoint}";

        TextStateView(GetObject((int)Skill.Fireball), playerStat.Fireball);
        TextStateView(GetObject((int)Skill.Earthquake), playerStat.Earthquake);
    }


    void TextStateView(GameObject go, Skill_SO data)
    {
        if (data.currentLevel < 0)
        {
            go.GetComponentInChildren<Text>().text = $"<b><color=red>{data.name}</color></b> : 미습득";
        }
        else
        {
            go.GetComponentInChildren<Text>().text = $"<b><color=red>{data.name}</color></b> : LV {data.currentLevel + 1} / {data.maxLevel + 1}";
        }

        go.GetComponentInChildren<Text>().text += $"\n<color=black>{data.contents}</color>";
    }







    void AddEvent_Skill(Skill skill, Skill_SO skill_Data, string skill_Name) //? 스킬트리 UIEVENT 일괄추가
    {
        var target = GetObject((int)skill);

        target.BindEvent((PointerEventData) => { SkillLevelUp(skill_Data); }, Define.UIEvent.Click);
        target.BindEvent((PointerEventData) => { DragBeginEvent(skill, skill_Data); }, Define.UIEvent.DragBegin);
        target.BindEvent((PointerEventData) => { DragEndEvent(PointerEventData, skill_Name, skill_Data); }, Define.UIEvent.DragEnd);
        target.BindEvent((PointerEventData) => { DragEvent(PointerEventData, skill_Data); }, Define.UIEvent.Drag);
    }
    bool SkillCheck(Skill_SO skill_Data) //? 스킬레벨이 -1이면 이벤트 실행 안되게
    {
        return skill_Data.currentLevel >= 0 ? true : false;
    }





    void SkillLevelUp(Skill_SO skill)
    {
        if (playerStat.SkillPoint > 0 && skill.currentLevel < skill.maxLevel)
        {
            skill.currentLevel++;
            playerStat.SkillPoint--;
            Debug.Log($"스킬 레벨업 {skill.name} 레벨 : {skill.currentLevel} / {skill.maxLevel}");
        }
        else
        {
            Debug.Log("스킬업 실패");
        }
    }

    GameObject temp;
    Vector3 temp_pos;
    void DragBeginEvent(Skill skill, Skill_SO skill_Data)
    {
        if (!SkillCheck(skill_Data)) return;

        Debug.Log("드래그시작");
        temp = GetObject((int)skill).transform.GetChild(0).gameObject;
        temp.GetComponent<Image>().raycastTarget = false;
        temp_pos = temp.transform.position;
    }
    void DragEndEvent(PointerEventData pointer, string skill_Name, Skill_SO skill_Data)
    {
        if (!SkillCheck(skill_Data)) return;

        temp.GetComponent<Image>().raycastTarget = true;
        temp.transform.position = temp_pos;
        temp = null;

        var a = pointer.pointerCurrentRaycast.gameObject;
        if (a != null)
        {
            if (a.CompareTag("SkillSlot_1") || a.CompareTag("SkillSlot_2") || a.CompareTag("SkillSlot_3"))
            {
                var slot = a.GetComponent<UI_SkillSlot>();
                var skill = Resources.Load<Skill_SO>($"Data/Skill/{skill_Name}");
                slot.RegisterSkill(skill);
            }
        }
    }
    void DragEvent(PointerEventData pointer, Skill_SO skill_Data)
    {
        if (!SkillCheck(skill_Data)) return;

        if (temp != null)
        {
            temp.transform.position = pointer.position;
        }
    }
}

