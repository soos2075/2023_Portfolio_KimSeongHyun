using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public string NPC_name;


    [SerializeField]
    bool playerContact;

    [SerializeField]
    GameObject dialogue;

    public GameObject questIcon;

    private void Awake()
    {
        NPC_name = gameObject.tag;
    }
    void Start()
    {
        StartCoroutine(Co_QuestUpdate());
        QuestUpdate();
    }

    void Update()
    {
        if (!playerContact || FindObjectOfType<CameraController>().cinemachine)
        {
            return;
        }

        NPCEventCall();
    }


    void NPCEventCall()
    {
        if (Input.GetKey(KeyCode.F) && dialogue == null)
        {
            SelectDialogue();
        }
    }


    IEnumerator Co_QuestUpdate()
    {
        yield return new WaitForSeconds(0.5f);
        QuestUpdate();
        
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            QuestUpdate();
        }
        //StartCoroutine(Co_QuestUpdate());
    }
    void QuestUpdate()
    {
        if (questIcon != null)
        {
            Managers.Resource.Destroy(questIcon);
        }
        var dataList = QuestManagerEx.Instance.GetQuestList(NPC_name);

        if (dataList == null)
        {
            Debug.Log("퀘스트데이터 없음");
            return;
        }

        //? 진행중인 퀘스트 검사
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].state == Define.QuestState.Proceed)
            {
                if (dataList[i].GetCheck())
                {
                    questIcon = Managers.Resource.Instantiate("UI/Quest/Icon_Complete", transform);
                }
                else
                {
                    questIcon = Managers.Resource.Instantiate("UI/Quest/Icon_Proceed", transform);
                }
                return;
            }
        }

        //? 진행중인 퀘스트가 없다면
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].state == Define.QuestState.Ready)
            {
                questIcon = Managers.Resource.Instantiate("UI/Quest/Icon_Start", transform);
                return;
            }
        }
    }


    void SelectDialogue()
    {
        var dataList = QuestManagerEx.Instance.GetQuestList(NPC_name);

        if (dataList == null)
        {
            Debug.Log("퀘스트데이터 없음");
            return;
        }

        Debug.Log("진행중인 퀘스트 검사");
        //? 진행중인 퀘스트 검사
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].state == Define.QuestState.Proceed)
            {
                if (dataList[i].GetCheck())
                {
                    dialogue = Managers.Dialogue.DialogueEvent($"Q_{dataList[i].ID}_Complete");
                }
                else
                {
                    dialogue = Managers.Dialogue.DialogueEvent($"Q_{dataList[i].ID}_Proceed");
                }
                return;
            }
        }

        Debug.Log("새로시작 퀘스트 검사");
        //? 진행중인 퀘스트가 없다면
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].state == Define.QuestState.Ready)
            {
                dialogue = Managers.Dialogue.DialogueEvent($"Q_{dataList[i].ID}_Start");
                return;
            }
        }

        Debug.Log("새로 시작할 퀘스트가 없음");
        //? 새로 시작할 퀘스트도 없다면
        dialogue = Managers.Dialogue.DialogueEvent("Finish");
    }






    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        playerContact = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        playerContact = false;
    }
}
