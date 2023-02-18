using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat_Base
{
    public int EXP { get { return exp; } set { exp = value; if (exp >= exp_max) LevelUp(); } }
    public int EXP_MAX { get { return exp_max; } set { exp_max = value; } }

    int exp;
    [SerializeField] int exp_max;



    public int SkillPoint { get; set; }

    public Skill_SO Fireball { get; set; }
    public Skill_SO Earthquake { get; set; }

    void Init_Skill()
    {
        SkillPoint = 3;
        Fireball = Resources.Load<Skill_SO>("Data/Skill/Fireball");
        Earthquake = Resources.Load<Skill_SO>("Data/Skill/earthquake");


        Fireball.maxLevel = (Fireball._data.Count - 1);
        Fireball.currentLevel = -1;
        Earthquake.maxLevel = (Earthquake._data.Count - 1);
        Earthquake.currentLevel = -1;
        Fireball.registerTag = "";
        Fireball.registerQuickSlot = false;
        Earthquake.registerTag = "";
        Earthquake.registerQuickSlot = false;
    }


    void Start()
    {
        Init_Skill();
        InitStat(100, 50, 10, 5);

        LEVEL = 1;
        exp = 0;
        exp_max = 100;

    }

    void Update()
    {
        
    }


    void LevelUp()
    {
        LEVEL++;
        exp = exp - exp_max;
        exp_max = (int)((exp_max * 1.2f) + 30);

        HP_MAX += 10;
        HP = HP_MAX;

        MP_MAX += 5;
        MP = MP_MAX;

        ATK += 2;
        DEF += 1;

        SkillPoint++;

        Managers.Resource.Instantiate_Timer("DefaultEffect/LevelUp", 2.0f, transform);
        Managers.UI.ShowWorldSpaceUI<UI_LevelUp>("UI_LevelUp", transform);
    }


    public void AttackCheck()
    {
        GameObject attacker = Managers.Resource.Instantiate_Timer("Attack/Normal_Attack", 0.3f, transform);
        attacker.GetComponent<NormalAttack>().Init(this);
    }


    public void AttackInvoke(Stat_Base target)
    {
        GameObject effect = Managers.Resource.Instantiate_Timer("AttackEffect/Normal_A", 3, transform);

        if (AttackEvent != null)
        {
            AttackEvent.Invoke();
            AttackEvent = null;
        }
    }


}
