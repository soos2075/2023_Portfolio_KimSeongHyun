using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Earthquake : SkillBase
{

    public override void StartEvent()
    {
        transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        GetComponent<SphereCollider>().enabled = true;
        disable = StartCoroutine(SelfDisable(5));
        Invoke("ColliderOff", 2);

        Managers.Sound.Play("Earthquake", volume: 0.5f);
    }

    void ColliderOff()
    {
        GetComponent<SphereCollider>().enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            float damage = caster.ATK * 0.8f;
            other.GetComponent<Stat_Base>().Hit((int)damage);
            Debug.Log($"{other.name} 에게 {caster.ATK} 데미지로 공격!");

            other.GetComponent<SlimeController>().LongRangeHit();
        }
    }

    protected override void Explosion()
    {
        StopCoroutine(disable);
        base.Explosion();
    }

    Coroutine disable;
    IEnumerator SelfDisable(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Explosion();
    }
}
