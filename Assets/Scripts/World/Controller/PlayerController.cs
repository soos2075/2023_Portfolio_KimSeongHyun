using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{

    public bool _isConversation;
    public Quest_SOEx startQuest;

    bool lockOnCheck;
    public GameObject lockOnTarget;
    GameObject lockOnEffect;

    void Start()
    {
        playerAnim = GetComponent<Animator>();

        QuestManagerEx.Instance.StartQuest(startQuest);

    }



    

    void Update()
    {
        if (_isConversation) //? 대화중이면 바로 리턴
            return;

        LockOn();
        Potion();

        if (playerAnim.GetInteger("AnimState") >= 3)
        {
            return;
        }

        Skill();

        if (playerAnim.GetInteger("AnimState") >= 3)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && ItemManager.Instance.nowEquip != null && !EventSystem.current.IsPointerOverGameObject())
        {
            State = PlayerState.Attack;
            return;
        }
        else if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            State = PlayerState.Move;
        }
        else
        {
            State = PlayerState.Idle;
        }

        PlayerMoveKeyboard();
    }


    int counter = 0;
    void LockOn()
    {
        if (lockOnCheck && !lockOnTarget.activeSelf)
        {
            counter = 0;
            lockOnCheck = false;
            lockOnTarget = null;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray dir = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.DrawRay(dir.origin, dir.direction * 30, Color.blue);
            if (Physics.Raycast(dir, out hit, 30, LayerMask.GetMask("Monster")))
            {
                Debug.Log($"LockOnTarget : {hit.collider.gameObject.name}");
                lockOnCheck = true;
                lockOnTarget = hit.collider.gameObject;
                if (lockOnEffect == null)
                {
                    lockOnEffect = Managers.Resource.Instantiate("DefaultEffect/Targeting", lockOnTarget.transform);
                }
                else
                {
                    lockOnEffect.transform.parent = lockOnTarget.transform;
                }
                lockOnEffect.transform.position = new Vector3(lockOnTarget.transform.position.x, 0.1f, lockOnTarget.transform.position.z);
                lockOnEffect.transform.localScale = Vector3.one * (lockOnTarget.GetComponent<CapsuleCollider>().radius * 3);

                //playerAnim.SetBool("Lockon", true);
            }
            else if (lockOnCheck && Physics.Raycast(dir, out hit, 30, ~LayerMask.GetMask("Monster")))
            {
                counter++;
                if (counter >= 2)
                {
                    Debug.Log($"Target Release");
                    counter = 0;
                    lockOnCheck = false;
                    lockOnTarget = null;
                    Managers.Resource.Destroy(lockOnEffect);
                    //playerAnim.SetBool("Lockon", false);
                }
            }
        }

        if (lockOnCheck)
        {
            float dist = (lockOnTarget.transform.position - transform.position).magnitude;
            if (dist > 30)
            {
                Debug.Log($"Target Release");
                counter = 0;
                lockOnCheck = false;
                lockOnTarget = null;
                Managers.Resource.Destroy(lockOnEffect);
            }
        }
    }


    #region PlayerAttack/Skill

    int skillAnimID;
    void Skill()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UI_SkillSlot slot = GameObject.FindGameObjectWithTag("SkillSlot_1").GetComponent<UI_SkillSlot>();
            if (slot.data != null && slot.ready)
            {
                Cast(slot);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UI_SkillSlot slot = GameObject.FindGameObjectWithTag("SkillSlot_2").GetComponent<UI_SkillSlot>();
            if (slot.data != null && slot.ready)
            {
                Cast(slot);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UI_SkillSlot slot = GameObject.FindGameObjectWithTag("SkillSlot_3").GetComponent<UI_SkillSlot>();
            if (slot.data != null && slot.ready)
            {
                Cast(slot);
            }
        }
    }
    void Potion()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UI_PotionSlot slot = GameObject.FindGameObjectWithTag("PotionSlot_1").GetComponent<UI_PotionSlot>();
            if (slot && slot.potion)
            {
                slot.QuickPotionUse();
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            UI_PotionSlot slot = GameObject.FindGameObjectWithTag("PotionSlot_2").GetComponent<UI_PotionSlot>();
            if (slot && slot.potion)
            {
                slot.QuickPotionUse();
            }
        }
    }

    void Cast(UI_SkillSlot slot)
    {
        if (gameObject.GetComponent<PlayerStat>().SkillCasting(slot.data._data[slot.data.currentLevel].mp))
        {
            currentSlot = slot;
            State = (PlayerState)slot.data.animationID;
        }
        else
        {
            Debug.Log("마나부족");
        }
    }

    UI_SkillSlot currentSlot;
    void AnimationHit() //? 스킬이 실제 발동되어야 할 타이밍에 호출되는 ClipEvent
    {
        currentSlot.CooltimeUI(currentSlot.data._data[currentSlot.data.currentLevel].cooltime);
        GameObject skill = Managers.Resource.Instantiate($"Attack/{currentSlot.data.prefabName}");
        skill.GetComponent<SkillBase>().InitSkill(this, GetComponent<PlayerStat>());
        skill.GetComponent<SkillBase>().StartEvent();
    }
    void AnimationOver() //? 스킬과 공격이 끝나는 타이밍에 호출되는 ClipEvent
    {
        State = PlayerState.Idle;
    }

    #endregion


    #region PlayerMove
    public Transform moveCamera;
    public float moveSpeed;
    public float rotaSpeed;


    [SerializeField]
    private float _minSpeed; //? Animator - Move BlendTree의 Threshold와 맞춰주어야함
    [SerializeField]
    private float _MaxSpeed;

    [SerializeField]
    float Shiftcount = 0;
    void PlayerMoveKeyboard()
    {
        if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Shiftcount = Mathf.Clamp01(Shiftcount) + (Time.deltaTime * 2);
                moveSpeed = Mathf.Lerp(_minSpeed, _MaxSpeed, Shiftcount);
            }
            else
            {
                Shiftcount = Mathf.Clamp01(Shiftcount) - (Time.deltaTime * 2);
                moveSpeed = Mathf.Lerp(_minSpeed, _MaxSpeed, Shiftcount);
            }
            Vector3 moveDir =
                Vector3.Scale(moveCamera.forward, new Vector3(1, 0, 1)) * Input.GetAxisRaw("Vertical")
                + Vector3.Scale(moveCamera.right, new Vector3(1, 0, 1)) * Input.GetAxisRaw("Horizontal");

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * rotaSpeed); //? 방향전환속도

            transform.Translate(moveDir.normalized * Time.deltaTime * moveSpeed, Space.World);
        }
    }
    #endregion



    #region PlayerAnimation
    public enum PlayerState
    {
        Idle,
        Move,
        Battle,
        Attack = 3,
        Fireball = 4,
        Earthquake = 5,
        Die,
    }

    [SerializeField]
    private PlayerState _playerState;

    Animator playerAnim;

    public PlayerState State
    { 
        get { return _playerState; }
        set 
        { 
            _playerState = value;
            switch (_playerState)
            {
                case PlayerState.Idle:
                    //playerAnim.CrossFade("WAIT00", 0.2f);
                    Shiftcount = 0;
                    playerAnim.SetInteger("AnimState", (int)PlayerState.Idle);
                    break;
                case PlayerState.Move:
                    playerAnim.SetFloat("MoveSpeed", moveSpeed);
                    playerAnim.SetInteger("AnimState", (int)PlayerState.Move);
                    break;
                case PlayerState.Battle:
                    break;

                case PlayerState.Attack:
                    if (lockOnCheck && lockOnTarget.activeSelf)
                    {
                        transform.LookAt(lockOnTarget.transform);
                    }
                    playerAnim.SetInteger("AnimState", (int)PlayerState.Attack);
                    break;
                case PlayerState.Fireball:
                    if (lockOnCheck && lockOnTarget.activeSelf)
                    {
                        transform.LookAt(lockOnTarget.transform);
                    }
                    playerAnim.SetInteger("AnimState", (int)PlayerState.Fireball);
                    break;
                case PlayerState.Earthquake:
                    playerAnim.SetInteger("AnimState", (int)PlayerState.Earthquake);
                    break;

                case PlayerState.Die:
                    break;
            }
        }
    }




    public void AnimEvent_WaitRandom()
    {
        int randomAni = Random.Range(0, 10);

        if (randomAni >= 1 && randomAni <= 3)
        {
            string str = "WAIT0" + randomAni.ToString();
            playerAnim.CrossFade(str, 0.2f);
        }
    }
    public void AnimEvent_ReturnWait()
    {
        playerAnim.CrossFade("WAIT00", 0.2f);
    }


    #endregion
}
