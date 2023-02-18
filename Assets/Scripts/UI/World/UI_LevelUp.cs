using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LevelUp : UI_Base
{
    //Text text;

    public override void Init()
    {
        //text = GetComponentInChildren<Text>();
    }

    void Start()
    {
        Init();

        Invoke("SelfDisable", 2);
    }

    void Update()
    {
        transform.position = transform.parent.position;
        transform.rotation = Camera.main.transform.rotation;
    }

    void SelfDisable()
    {
        Managers.Resource.Destroy(gameObject);
    }

}
