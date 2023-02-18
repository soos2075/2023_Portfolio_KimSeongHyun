using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpBar : UI_Base
{
    float posY;
    Stat_Base stat;
    Slider slider;

    public override void Init()
    {
        posY = transform.parent.GetComponent<Collider>().bounds.size.y;
        posY *= 1.2f;

        stat = transform.parent.GetComponent<Stat_Base>();
        slider = GetComponentInChildren<Slider>();
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        transform.position = transform.parent.position + (Vector3.up * posY);
        transform.rotation = Camera.main.transform.rotation;

        slider.value = stat.HP / (float)stat.HP_MAX;
    }
}
