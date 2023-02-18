using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillSlot : UI_Base
{
    enum Objects
    {
        SkillSprite,
        Cooltime,
    }

    public Skill_SO data;

    Image skillSprite;
    Image cooldown;

    public bool ready;

    public override void Init()
    {
        Bind<GameObject>(typeof(Objects));
        skillSprite = GetObject((int)Objects.SkillSprite).GetComponent<Image>();
        cooldown = GetObject((int)Objects.Cooltime).GetComponent<Image>();

        gameObject.BindEvent((data) => { RemoveSkill(data); }, Define.UIEvent.Click);
    }

    void Start()
    {
        Init();
        cooldown.fillAmount = 0;
        ready = true;
    }

    void Update()
    {
        if (data == null) return;
    }


    public void RegisterSkill(Skill_SO _data)
    {
        if (data != null)
        {
            data.registerTag = "";
            data.registerQuickSlot = false;
        }
        data = _data;
        skillSprite.sprite = _data.skillSprite;
        skillSprite.color = Color.white;

        if (data.registerQuickSlot && data.registerTag != gameObject.tag)
        {
            Debug.Log("여기냐?");
            GameObject.FindGameObjectWithTag(data.registerTag).GetComponent<UI_SkillSlot>().RemoveSlot();
        }
        
        data.registerTag = gameObject.tag;
        data.registerQuickSlot = true;
    }
    public void RemoveSkill(PointerEventData pointer)
    {
        if (pointer.pointerId == -2)
        {
            RemoveSlot();
        }
    }
    void RemoveSlot()
    {
        if (data != null)
        {
            data.registerQuickSlot = false;
            data = null;
            skillSprite.color = Color.clear;
        }
    }

    public void CooltimeUI(float count)
    {
        ready = false;
        cooldown.fillAmount = 1;
        StartCoroutine(Co_Cooltime(count));
    }
    IEnumerator Co_Cooltime(float count)
    {
        float timer = 0;
        while (timer < count)
        {
            timer += Time.deltaTime;
            cooldown.fillAmount = 1 - (timer / count);
            yield return null;
        }
        cooldown.fillAmount = 0;
        ready = true;
    }

}
