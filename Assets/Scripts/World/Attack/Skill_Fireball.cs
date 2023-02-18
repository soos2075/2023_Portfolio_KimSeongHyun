using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Fireball : SkillBase
{
    float speed;
    Vector3 direction;


    public override void StartEvent()
    {
        base.StartEvent();
        transform.position = new Vector3(player.transform.position.x, 0.5f, player.transform.position.z);

        speed = 15;
        if (player.lockOnTarget != null)
        {
            direction = (player.lockOnTarget.transform.position - transform.position);
        }
        else
        {
            direction = player.transform.forward;
        }

        disable = StartCoroutine(SelfDisable());
        Managers.Sound.Play("Fire", volume: 0.5f);
    }

    protected override void UpdateEvent()
    {
        transform.Translate(direction.normalized * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hitCheck)
        {
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            hitCheck = true;
            float damage = caster.ATK * 1.5f;
            other.GetComponent<Stat_Base>().Hit((int)damage);
            Debug.Log($"{other.name} 에게 {caster.ATK} 데미지로 공격!");

            Explosion();
            GameObject go = Managers.Resource.Instantiate_Timer("AttackEffect/FireballExplosion", 3);
            go.transform.position = transform.position;

            other.GetComponent<SlimeController>().LongRangeHit();
        }
    }

    protected override void Explosion()
    {
        StopCoroutine(disable);
        base.Explosion();
    }

    Coroutine disable;
    IEnumerator SelfDisable()
    {
        yield return new WaitForSeconds(2);
        Explosion();
    }
}
