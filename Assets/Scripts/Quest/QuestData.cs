using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData
{
    public int ID;

    public Define.QuestState state;

    public Define.QuestType type;

    public List<Requirement> requirementList = new List<Requirement>();

    public string assigned_NPC;

    public Action StartEvent;
    public Action CompleteEvent;


    public QuestData(int id, Define.QuestState questState, Define.QuestType questType, string npc_tagName)
    {
        ID = id;
        state = questState;
        type = questType;
        assigned_NPC = npc_tagName;
    }
    public void SetStartEvent(Action action)
    {
        StartEvent = action;
    }
    public void SetCompleteEvent(Action action)
    {
        CompleteEvent = action;
    }


    public Requirement Initialize_Requirement(Func<bool> requirement)
    {
        var re = new Requirement(requirement);
        requirementList.Add(re);
        return re;
    }
    public Requirement Initialize_Requirement( Func<bool> requirement, Func<int> needItem, Func<int> nowItem)
    {
        var re = new Requirement(requirement);
        re.GetRequireItem = needItem;
        re.GetCurrentlyItem = nowItem;

        requirementList.Add(re);
        return re;
    }
    public void Remove_Requirement(Requirement item)
    {
        if (requirementList.Contains(item))
        {
            requirementList.Remove(item);
        }
        else
        {
            Debug.Log("삭제할 조건이 없음");
        }
    }

    public bool GetCheck()
    {
        for (int i = 0; i < requirementList.Count; i++)
        {
            if (requirementList[i].Check() == false) //? 조건중 하나라도 false면 false리턴
            {
                return false;
            }
        }
        return true;
    }

    public int GetRequire(int index = 0)
    {
        if (requirementList[index].GetRequireItem == null)
        {
            return 0;
        }
        return requirementList[index].GetRequireItem();
    }
    public int GetCurrently(int index = 0)
    {
        if (requirementList[index].GetCurrentlyItem == null)
        {
            return 0;
        }
        return requirementList[index].GetCurrentlyItem();
    }




}

public class Requirement
{
    public Func<bool> Check { get; set; }

    public Func<int> GetRequireItem;
    public Func<int> GetCurrentlyItem;

    public Requirement(Func<bool> func)
    {
        Check = func;
    }
}

