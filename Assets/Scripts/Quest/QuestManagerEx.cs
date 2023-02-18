using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManagerEx : MonoBehaviour
{
    private static QuestManagerEx _instance;
    public static QuestManagerEx Instance { get { Init(); return _instance; } }

    static void Init()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<QuestManagerEx>();
            if (_instance == null)
            {
                var go = new GameObject(name: "@QuestManagerEx");
                _instance = go.AddComponent<QuestManagerEx>();
                DontDestroyOnLoad(go);
            }
        }
    }

    Dictionary<int, QuestData> questDataDict;


    private void Awake()
    {
        Init_QuestData();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //? AddData 과정에 대해
    /* 
    Case 1. 관리하는 npc + if문 하나로 해결할 수 있는 조건일경우 (누군가와 대화하기, 특정행동하기, 지정위치 도달 등등)
        -AddData함수의 2번째 오버로드를 사용

    Case 2. 관리하는 npc + 개수 지정 퀘스트일 경우 (특정 아이템 모아오기 - 실시간 업데이트 되야함)
        -AddData함수의 3번째 오버로드를 사용

    Case 3. 관리하는 npc + 조건이 여러가지일 경우 (2번과 같이 실시간 업데이트가 되야하는 경우)
        -QuestData를 직접 정의 후 AddData의 첫번째 오버로드를 사용
        -조건 추가는 QuestData.Initialize_Requirement()메서드를 사용. 2가지 오버로딩은 각각 Case 1번과 Case 2번의 경우임.

    Case 4. 관리하는 npc + 조건이 여러가지일 경우 (한번 달성하면 끝인 경우)
        -3번과 같지만 조건 추가할때 Initialize_Requirement 메서드 호출 후 리턴값을 저장해놨다가 
        바로 다음 Initialize_Requirement에 같은조건을 걸고 조건이 만족되면 첫번째 호출했던 메서드리턴값을 그대로 Remove해줌.
        그리고 두번째 Initialize_Requirement 리턴은 언제나 true로 고정 / 어차피 모든 조건이 True여야만 퀘스트 클리어 조건이 만족되므로
        첫번째 조건이 남아있다면 실시간 체크에서 걸러질것이고, 첫번째 조건이 만족됐다면 해당 조건은 사라지고 두번째 조건만 남아있는데 두번째 조건은 항상 트루임으로 문제 없음.
    */

    //? TestCaseExample
    /*
    void CaseTest()
    {
        //? Case 1 - 미사키에게 말걸기 (걍 받자마자 클리어 상태)
        AddData(100, "NPC_Misaki",
            () => { return true; });

        //? Case 2 - 재료 5개 모아가기
        AddData(102, "NPC_Misaki",
            () =>
            {
                return ItemManager.Instance.SearchItem("광석") >= 5 ? true : false;
            },
            () => { return 5; },
            () => { return ItemManager.Instance.SearchItem("광석"); });

        //? Case 3 - W를 누르고 있을때만 True / 이외에는 false
        QuestData temp3 = new QuestData(103, Define.QuestState.Ready, Define.QuestType.Etc, "NPC_Misaki");
        temp3.Initialize_Requirement(
            () =>
            {
                if (Input.GetKey(KeyCode.W))
                {
                    return true;
                }
                return false;
            });
        AddData(103, temp3);

        //? Case 4 - 퀘스트를 받은 뒤 한번이라도 W를 눌렀다면 계속 True / 아예 안누른 경우에만 false
        QuestData temp4 = new QuestData(104, Define.QuestState.Ready, Define.QuestType.Etc, "NPC_Misaki");
        Requirement temp4_0 =
        temp4.Initialize_Requirement(
            () =>
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    return true;
                }
                return false;
            });
        temp4.Initialize_Requirement(
            () =>
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    temp4.Remove_Requirement(temp4_0);
                }
                return true;
            });
        AddData(104, temp4);
    }
    */

    void Init_QuestData() //? 퀘스트 데이터 넣는곳
    {
        questDataDict = new Dictionary<int, QuestData>();
        //? Quest100
        AddData(100, "NPC_Misaki",
            () => { return true; });

        //? Quest101
        QuestData temp101 = new QuestData(101, Define.QuestState.Ready, Define.QuestType.Etc, "NPC_Misaki");
        temp101.Initialize_Requirement(() =>
        {
            if (ItemManager.Instance.nowEquip != null)
            {
                return true;
            }
            return false;
        });
        temp101.SetStartEvent(() =>
        { 
            Managers.Trade.AddItemEvent(1001);
        });
        temp101.SetCompleteEvent(() =>
        {
            //Managers.Trade.SubtractItemEvent(501, 5);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>().EXP += 50;
        });
        AddData(101, temp101);

        //? Quest102
        QuestData temp102 = new QuestData(102, Define.QuestState.Ready, Define.QuestType.Gather, "NPC_Misaki");
        temp102.Initialize_Requirement(
            () => { return ItemManager.Instance.SearchItem("광석") >= 5 ? true : false; },
            () => { return 5; },
            () => { return ItemManager.Instance.SearchItem("광석"); });
        temp102.SetCompleteEvent(() =>
        {
            Managers.Trade.SubtractItemEvent(501, 5);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>().EXP += 250;
            Managers.Trade.AddItemEvent(1003);
        });
        AddData(102, temp102);
    }


    void AddData(int id, QuestData data)
    {
        questDataDict.Add(id, data);
    }

    void AddData(int id, string npc, Func<bool> requirement)
    {
        QuestData data = new QuestData(id, Define.QuestState.Ready, Define.QuestType.Etc, npc);
        data.Initialize_Requirement(requirement);

        questDataDict.Add(id, data);
    }
    void AddData(int id, string npc, Func<bool> requirement, Func<int> needItem, Func<int> nowItem)
    {
        QuestData data = new QuestData(id, Define.QuestState.Ready, Define.QuestType.Gather, npc);
        data.Initialize_Requirement(requirement, needItem, nowItem);

        questDataDict.Add(id, data);
    }







    #region QuestData

    List<Quest_SOEx> currentQuestList = new List<Quest_SOEx>();


    public List<QuestData> GetQuestList(string NPC_name)
    {
        List<QuestData> questDatas = new List<QuestData>();

        foreach (var item in questDataDict)
        {
            if (item.Value.assigned_NPC == NPC_name)
            {
                questDatas.Add(item.Value);
            }
        }

        if (questDatas.Count > 0)
        {
            questDatas.Sort((QuestData x, QuestData y) =>
            {
                if (x.ID > y.ID) return 1;
                else if (x.ID < y.ID) return -1;
                else return 0;
            }
            );

            return questDatas;
        }
        else
            return null;
    }

    public QuestData GetQuestData(int id)
    {
        QuestData data;
        questDataDict.TryGetValue(id, out data);

        if (data != null)
            return data;

        Debug.Log("퀘스트 데이터 없음");
        return null;
    }


    public void StartQuest(Quest_SOEx so_Data)
    {
        QuestData data;
        questDataDict.TryGetValue(so_Data.id, out data);

        if (data == null)
            return;

        data.state = Define.QuestState.Proceed;
        currentQuestList.Add(so_Data);
        //? 퀘스트 시작 이벤트 Action이 있으면 퀘스트 시작과 동시에 실행
        if (data.StartEvent != null)
        {
            data.StartEvent.Invoke();
        }

        ShowMyQuest();
    }
    public void CompleteQuest(Quest_SOEx so_Data)
    {
        QuestData data;
        questDataDict.TryGetValue(so_Data.id, out data);

        if (data == null)
            return;

        data.state = Define.QuestState.Complete;
        currentQuestList.Remove(so_Data);
        //? 재료 가져가고 보상 주기같은 후처리 이벤트함수
        if (data.CompleteEvent != null)
        {
            data.CompleteEvent.Invoke();
        }

        ShowMyQuest();
    }


    public void ShowMyQuest()
    {
        if (GameObject.FindGameObjectWithTag("QuestBox") == null)
        {
            return;
        }

        for (int i = 0; i < GameObject.FindGameObjectWithTag("QuestBox").transform.childCount; i++)
        {
            Managers.Resource.Destroy(GameObject.FindGameObjectWithTag("QuestBox").transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < currentQuestList.Count; i++)
        {
            GameObject go = Managers.Resource.Instantiate("UI/Quest/QuestObject", GameObject.FindGameObjectWithTag("QuestBox").transform);
            go.GetComponent<QuestObjectEx>().Initialize_QuestData(currentQuestList[i]);
        }
    }


    #endregion
}
