using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStat : UI_Base
{
    enum Stats
    {
        HpBar,
        MpBar,
        ExpBar,
        Level,
    }

    PlayerStat playerStat;


    public override void Init()
    {
        playerStat = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerStat>();
        Bind<GameObject>(typeof(Stats));
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        ViewStatBar(GetObject((int)Stats.HpBar), playerStat.HP, playerStat.HP_MAX);
        ViewStatBar(GetObject((int)Stats.MpBar), playerStat.MP, playerStat.MP_MAX);
        ViewStatBar(GetObject((int)Stats.ExpBar), playerStat.EXP, playerStat.EXP_MAX);

        GetObject((int)Stats.Level).GetComponent<Text>().text = $"LV.{playerStat.LEVEL}";
    }


    void ViewStatBar(GameObject go, int valueNow, int valueMax)
    {
        go.transform.GetChild(0).GetComponent<Image>().fillAmount = (float)valueNow / (float)valueMax;
        go.transform.GetChild(1).GetComponent<Text>().text = $"{valueNow} / {valueMax}";

    }
}
