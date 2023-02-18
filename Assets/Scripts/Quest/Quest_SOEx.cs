using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Quest_SOEx : ScriptableObject
{
    public int id;

    [TextArea(1,10)]
    public string title;

    [TextArea(3, 10)]
    public string contents;

    public QuestData data;


    public string NPC_tagName;


}
