using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class Skill_SO : ScriptableObject
{
    public string skill_name;
    [TextArea(2,10)]
    public string contents;

    public string prefabName;
    public Sprite skillSprite;


    public int currentLevel { get; set; }
    public int maxLevel { get; set; }
    public int animationID;

    public bool registerQuickSlot;
    public string registerTag;

    public List<LevelData> _data = new List<LevelData>();
    [System.Serializable]
    public class LevelData
    {
        public int mp;
        public float cooltime;
    }
}



[CustomEditor(typeof(Skill_SO))]
public class GuiTest : Editor
{
    Skill_SO value;
    int index;

    private void OnEnable()
    {
        value = (Skill_SO)target;
        index = 0;
        if (value._data.Count == 0)
        {
            AddList();
        }
    }

    public override void OnInspectorGUI()
    {
        //Debug.Log($"index : {index} // List.Count : {value._data.Count}");
        ButtonEvent();
        PageView();
        value.maxLevel = (value._data.Count - 1);
    }


    void ButtonEvent()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        //GUILayout.Space((EditorGUIUtility.currentViewWidth / 2) - 60);


        if (GUILayout.Button("-", GUILayout.Width(30), GUILayout.Height(30)))
        {
            RemoveList();
        }
        if (GUILayout.Button("<<", GUILayout.Width(30), GUILayout.Height(30)))
        {
            PreviousPage();
        }
        if (GUILayout.Button(">>", GUILayout.Width(30), GUILayout.Height(30)))
        {
            NextPage();
        }
        if (GUILayout.Button("+", GUILayout.Width(30), GUILayout.Height(30)))
        {
            AddList();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }


    void PageView()
    {
        EditorGUILayout.BeginVertical();

        value.animationID = (int)EditorGUILayout.IntField("AnimationID", value.animationID);

        EditorGUILayout.LabelField($"{value.skill_name} LV.{index}");

        value.skillSprite = (Sprite)EditorGUILayout.ObjectField("이미지", value.skillSprite, typeof(Sprite), true);
        value.skill_name = (string)EditorGUILayout.TextField("스킬명", value.skill_name);
        EditorGUILayout.LabelField("상세설명");
        value.contents = (string)EditorGUILayout.TextArea(value.contents, GUILayout.MinHeight(50), GUILayout.MinWidth(100));
        value.prefabName = (string)EditorGUILayout.TextField("PrefabName", value.prefabName);

        
        value._data[index].mp = (int)EditorGUILayout.IntField("필요 MP", value._data[index].mp);
        value._data[index].cooltime = (float)EditorGUILayout.FloatField("쿨타임", value._data[index].cooltime);

        EditorGUILayout.EndVertical();

        if (GUI.changed) EditorUtility.SetDirty(target);
    }

    void NextPage()
    {
        if (index + 1 < value._data.Count)
        {
            index++;
        }
    }
    void PreviousPage()
    {
        if (index > 0)
        {
            index--;
        }
    }
    void AddList()
    {
        value._data.Add(new Skill_SO.LevelData());
    }
    void RemoveList()
    {
        if (index < value._data.Count)
        {
            value._data.RemoveAt(index);
            index--;
        }
    }

}


