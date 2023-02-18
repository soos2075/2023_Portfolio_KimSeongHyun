using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Dialogue_ScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class Dialogue
    {
        public string name;
        [TextArea(3, 10)]
        public string contents;

        [Header("ImageOption")]
        public Sprite sprite;
        public int indexer;
        public Define.ImageOption option;
    }


    public List<Dialogue> dialogues = new List<Dialogue>();

    public Quest_SOEx startQuest;
    public Quest_SOEx completeQuest;
}
