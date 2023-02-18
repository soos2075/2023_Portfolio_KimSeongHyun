using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : MonoBehaviour
{
    Stat_Base attacker;

    public void Init(Stat_Base stat)
    {
        attacker = stat;
    }


    void Start()
    {
        Managers.Sound.Play("NormalAttack", volume: 0.5f);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 4, Space.Self);
    }

    List<Collider> goList = new List<Collider>();


    private void OnTriggerEnter(Collider other)
    {
        foreach (var item in goList)
        {
            if (item == other)
            {
                return;
            }
        }

        goList.Add(other);
        attacker.AddAttackEvent(() => { attacker.Attack(other.GetComponent<Stat_Base>()); });
        Debug.Log($"{other.name} 에게 {attacker.ATK} 데미지로 공격!");
    }


}
