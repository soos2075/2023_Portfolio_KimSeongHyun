using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dialogue : UI_Base
{

    enum StandingImages
    {
        LeftStanding,
        CenterStanding,
        RightStanding,
    }

    enum ConversationTexts
    {
        nameText,
        contentsText,
    }

    Image leftStanding;
    Image centerStanding;
    Image rightStanding;


    public Dialogue_ScriptableObject data;
    public int dialogue_counter;

    PlayerController player;

    public override void Init()
    {
        Bind<Image>(typeof(StandingImages));
        Bind<Text>(typeof(ConversationTexts));

        leftStanding = GetImage((int)StandingImages.LeftStanding);
        centerStanding = GetImage((int)StandingImages.CenterStanding);
        rightStanding = GetImage((int)StandingImages.RightStanding);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void Init_DialogueData(Dialogue_ScriptableObject dialogue_data)
    {
        data = dialogue_data;
    }

    void Start()
    {
        Init();

        ImageReset();

        if (data == null)
        {
            //gameObject.SetActive(false);
            Debug.Log("데이터가 존재하지 않음");
            Managers.Resource.Destroy(gameObject);
            return;
        }

        player._isConversation = true;
        Conversation();

        FindObjectOfType<CameraController>().ChangeVcam2();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (isTyping)
            {
                isSkip = true;
                return;
            }
            dialogue_counter++;
            Conversation();
        }
    }

    void Conversation()
    {
        if (dialogue_counter < data.dialogues.Count)
        {
            //SpeakSomething(data.dialogues[dialogue_counter].name, data.dialogues[dialogue_counter].contents);
            StartCoroutine(TypingEffect(data.dialogues[dialogue_counter].name, data.dialogues[dialogue_counter].contents));
            SetStandingImage(data.dialogues[dialogue_counter].sprite, (StandingImages)data.dialogues[dialogue_counter].indexer, data.dialogues[dialogue_counter].option);
        }
        else
        {
            Debug.Log("남은 대화가 없습니다.");
            player._isConversation = false;
            if (data.startQuest != null)
            {
                FindObjectOfType<QuestManagerEx>().StartQuest(data.startQuest);
            }
            if (data.completeQuest != null)
            {
                FindObjectOfType<QuestManagerEx>().CompleteQuest(data.completeQuest);
            }
            Managers.Resource.Destroy(gameObject);
            FindObjectOfType<CameraController>().ResetVcam();
        }
    }

    bool isSkip = false;
    bool isTyping = false;
    IEnumerator TypingEffect(string name, string contents)
    {
        int charIndexer = 0;
        
        while (!isSkip && contents.Length >= charIndexer)
        {
            isTyping = true;

            string nowText = contents.Substring(0, charIndexer);
            SpeakSomething(name, nowText);

            yield return new WaitForSeconds(0.1f);
            charIndexer++;
        }
        isTyping = false;
        isSkip = false;
        SpeakSomething(name, contents);
        Debug.Log("출력끝");
    }



    void ImageReset()
    {
        leftStanding.sprite = null;
        centerStanding.sprite = null;
        rightStanding.sprite = null;

        leftStanding.enabled = false;
        centerStanding.enabled = false;
        rightStanding.enabled = false;
    }

    void SpeakSomething(string name, string contents)
    {
        GetText((int)ConversationTexts.nameText).text = name;
        GetText((int)ConversationTexts.contentsText).text = contents;
    }


    void SetStandingImage(Sprite sprite, StandingImages location)
    {
        if (sprite == null)
        {
            return;
        }

        switch (location)
        {
            case StandingImages.LeftStanding:
                leftStanding.sprite = sprite;
                leftStanding.color = new Color32(255, 255, 255, 255);
                leftStanding.enabled = true;
                break;

            case StandingImages.CenterStanding:
                centerStanding.sprite = sprite;
                centerStanding.color = new Color32(255, 255, 255, 255);
                centerStanding.enabled = true;
                break;

            case StandingImages.RightStanding:
                rightStanding.sprite = sprite;
                rightStanding.color = new Color32(255, 255, 255, 255);
                rightStanding.enabled = true;
                break;
        }
    }
    void SetStandingImage(Sprite sprite, StandingImages location, Define.ImageOption option)
    {
        switch (option)
        {
            case Define.ImageOption.None:
                break;

            case Define.ImageOption.Active:
                if (leftStanding.sprite != null)
                {
                    leftStanding.color = new Color32(255, 255, 255, 255);
                    leftStanding.enabled = true;
                }
                if (centerStanding.sprite != null)
                {
                    centerStanding.color = new Color32(255, 255, 255, 255);
                    centerStanding.enabled = true;
                }
                if (rightStanding.sprite != null)
                {
                    rightStanding.color = new Color32(255, 255, 255, 255);
                    rightStanding.enabled = true;
                }
                break;

            case Define.ImageOption.Inactive:
                ImageReset();
                break;

            case Define.ImageOption.AlphaDown:
                if (leftStanding.sprite != null)
                {
                    leftStanding.color = new Color32(255, 255, 255, 100);
                    leftStanding.enabled = true;
                }
                if (centerStanding.sprite != null)
                {
                    centerStanding.color = new Color32(255, 255, 255, 100);
                    centerStanding.enabled = true;
                }
                if (rightStanding.sprite != null)
                {
                    rightStanding.color = new Color32(255, 255, 255, 100);
                    rightStanding.enabled = true;
                }
                break;
        }

        SetStandingImage(sprite, location);
    }



}
