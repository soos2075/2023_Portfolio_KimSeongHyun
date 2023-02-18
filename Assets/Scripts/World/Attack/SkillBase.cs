using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    protected PlayerController player;
    protected Stat_Base caster;

    
    protected bool hitCheck;

    public void InitSkill(PlayerController pl, Stat_Base stat)
    {
        player = pl;
        caster = stat;
    }


    public virtual void StartEvent()
    {
        hitCheck = false;
    }
    protected virtual void UpdateEvent()
    {

    }
    void Start()
    {
        //StartEvent();
    }
    void Update()
    {
        UpdateEvent();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (hitCheck)
        {
            return;
        }

        if (other.gameObject.layer == LayerMask.GetMask("Monster"))
        {
            Debug.Log($"{other.gameObject.name} 접촉");
        }
    }



    protected virtual void Explosion()
    {
        //? 폭발 이펙트, 사운드, 오브젝트관리
        Managers.Resource.Destroy(gameObject);
    }
}
