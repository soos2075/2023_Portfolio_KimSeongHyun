using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStat : Stat_Base
{

    void Start()
    {
        InitStat(30,0,9,2);
        Managers.UI.ShowWorldSpaceUI<UI_HpBar>("UI_HpBar", transform);
    }

    void Update()
    {
        
    }


    protected override void Die()
    {
        base.Die();

        FindObjectOfType<SpawningPool>().CurrentConut--;

        //? 아이템 드랍 이벤트
        ItemDrop();
        GetExp();

        gameObject.GetComponent<SlimeController>().State = SlimeController.SlimeState.Die;
    }


    void ItemDrop()
    {
        TradeManager.ItemWeighted[] droplist = new TradeManager.ItemWeighted[]
        {
            new TradeManager.ItemWeighted(1001, 200),
            new TradeManager.ItemWeighted(501, 2000),
            new TradeManager.ItemWeighted(1, 500)
        };

        Managers.Trade.WeightedRandomDrop(droplist);
    }

    void GetExp()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>().EXP += 25;
    }


    void SlimeAttack()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>().Hit(ATK);
        //Debug.Log("슬라임어택");
    }
    void SlimeDestroy()
    {
        //Managers.Resource.Destroy(gameObject);
        gameObject.SetActive(false);
    }


}
