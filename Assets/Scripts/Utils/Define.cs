using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum UIEvent
    {
        Click,
        Drag,
        DragBegin,
        DragEnd,
        All,
    }

    public enum ImageOption
    {
        None,
        Active,
        Inactive,
        AlphaDown,
    }

    public enum QuestState
    {
        Ready,
        Proceed,
        Complete,
    }

    public enum QuestType
    {
        Etc,
        Gather,
    }

    public enum SkillType
    {
        Instant,
        Targeting,
    }

}
