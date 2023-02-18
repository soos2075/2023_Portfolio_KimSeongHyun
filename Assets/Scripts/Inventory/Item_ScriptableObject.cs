using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class Item_ScriptableObject : ScriptableObject
{

    [System.Serializable]
    public class ItemData
    {
        public Sprite _sprite;

        public string _name;

        public Item.ItemType _type;

        //? 1~100 = 소비아이템, 501~1000 = 재료아이템, 1001~ = 장비아이템
        public int _itemID;
        //? 1~5
        public int _rarity;
    }
    public List<ItemData> itemDatas = new List<ItemData>();



#if UNITY_EDITOR
    [CustomEditor(typeof(Item_ScriptableObject))]
    public class GuiTest : Editor
    {
        Item_ScriptableObject value;

        private void OnEnable()
        {
            value = (Item_ScriptableObject)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();


            for (int i = 0; i < value.itemDatas.Count; i++)
            {
                value.itemDatas[i]._sprite = (Sprite)EditorGUILayout.ObjectField("이미지", value.itemDatas[i]._sprite, typeof(Sprite), true);
                value.itemDatas[i]._type = (Item.ItemType)EditorGUILayout.EnumPopup("Type", value.itemDatas[i]._type);
                value.itemDatas[i]._name = (string)EditorGUILayout.TextField("Name", value.itemDatas[i]._name);
                value.itemDatas[i]._itemID = (int)EditorGUILayout.IntField("ID", value.itemDatas[i]._itemID);
                value.itemDatas[i]._rarity = (int)EditorGUILayout.IntField("rarity", value.itemDatas[i]._rarity);

                EditorGUILayout.Space();
                EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 5), new Color(0.5f, 0.5f, 0.5f, 1));
                EditorGUILayout.Space();
            }

            base.OnInspectorGUI();

            EditorGUILayout.EndVertical();

            if (GUI.changed) EditorUtility.SetDirty(target);
        }

    }

#endif
}
