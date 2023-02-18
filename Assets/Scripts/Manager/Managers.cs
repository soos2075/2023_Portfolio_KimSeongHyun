using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance;
    public static Managers Instance { get { Init(); return _instance; } }

    #region Core
    ResourceManager _resource = new ResourceManager();
    PoolManager _pool = new PoolManager();
    UIManager _ui = new UIManager();
    SoundManager _sound = new SoundManager();

    public static ResourceManager Resource { get { return Instance._resource; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    #endregion




    DialogueManager _dialogue = new DialogueManager();
    public static DialogueManager Dialogue { get { return Instance._dialogue; } }

    TradeManager _trade = new TradeManager();
    public static TradeManager Trade { get { return Instance._trade; } }




    static void Init()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<Managers>();
            if (_instance == null)
            {
                var go = new GameObject(name: "@Managers");
                _instance = go.AddComponent<Managers>();
                DontDestroyOnLoad(go);
            }
        }
    }

    void Start()
    {
        Init();
        Trade.Init();
        Sound.Init();
    }

    void Update()
    {

    }





}
