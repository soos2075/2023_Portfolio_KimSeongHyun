using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat_Base : MonoBehaviour
{
    //? 몬스터와 캐릭터 둘다 필요한 스탯 - HP, MP, ATK, DEF, LEVEL

    //? 플레이어만 필요한 스탯 - EXP

    //? 몬스터만 필요한 스탯 - 

    public int HP { get { return hp; } set { hp = Mathf.Clamp(value, 0, hp_max); if (hp <= 0) Die(); } }
    public int HP_MAX { get { return hp_max; } set { hp_max = value; } }
    public int MP { get { return mp; } set { mp = Mathf.Clamp(value, 0, mp_max); } }
    public int MP_MAX { get { return mp_max; } set { mp_max = value; } }

    protected int hp;
    [SerializeField] protected int hp_max;

    protected int mp;
    [SerializeField] protected int mp_max;

    public int LEVEL { get; set; }
    public int ATK { get; set; }
    public int DEF { get; set; }



    public Action AttackEvent { get; set; }

    protected void InitStat(int _hp, int _mp, int _atk, int _def)
    {
        hp = _hp;
        hp_max = _hp;

        mp = _mp;
        mp_max = _mp;

        ATK = _atk;
        DEF = _def;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public bool SkillCasting(int mp)
    {
        if (mp > MP)
        {
            return false;
        }

        MP -= mp;
        return true;
    }

    public void Hit(int damage)
    {
        int DMG = Mathf.Clamp(damage - DEF, 1, damage);
        HP -= DMG;
    }

    public void Attack(Stat_Base target)
    {
        target.Hit(ATK);
    }

    public void AddAttackEvent(Action action)
    {
        AttackEvent += action;
    }


    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} Die");
    }
}
