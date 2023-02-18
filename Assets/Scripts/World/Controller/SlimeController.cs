using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeController : MonoBehaviour
{
    
    

    [SerializeField]
    Vector3[] moveList = new Vector3[3];

    GameObject moveArea;
    NavMeshAgent nav;
    public Animator anim;
    GameObject player;

    float sizeX;
    float sizeZ;


    int moveCount;
    float moveWaitCount;

    Vector3 destination;



    public enum SlimeState
    {
        Idle,
        Move,
        Battle,
        Attack,
        Die,
    }
    SlimeState _state;

    public SlimeState State
    { 
        get
        {
            return _state;
        }
        set 
        {
            if (_state == SlimeState.Die)
            {
                return;
            }

            _state = value;
            switch (_state)
            {
                case SlimeState.Idle:
                    anim.CrossFade("IDLE", 0.2f);
                    break;
                case SlimeState.Move:
                    anim.CrossFade("WALK", 0.2f);
                    break;
                case SlimeState.Battle:
                    anim.CrossFade("IDLE_BATTLE", 0.2f);
                    break;
                case SlimeState.Attack:
                    anim.CrossFade("Attack", 0.2f);
                    AttackStart();
                    break;
                case SlimeState.Die:
                    anim.Play("Die");
                    nav.isStopped = true;
                    GetComponent<CapsuleCollider>().enabled = false;
                    break;
            }
        }
    }



    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        moveArea = GameObject.FindGameObjectWithTag("SlimeField");
        player = GameObject.FindGameObjectWithTag("Player");

        sizeX = moveArea.GetComponent<BoxCollider>().size.x / 2;
        sizeZ = moveArea.GetComponent<BoxCollider>().size.z / 2;

        for (int i = 0; i < moveList.Length; i++)
        {
            moveList[i] = new Vector3(Random.Range(-sizeX, sizeX) + moveArea.transform.position.x, 0, Random.Range(-sizeZ, sizeZ) + moveArea.transform.position.z);
        }

        distance_battlePhase = 5;
        distance_attack = 1.5f;
    }

    float distance;
    float distance_battlePhase;
    float distance_attack;

    void Update()
    {
        if (State == SlimeState.Die)
        {
            return;
        }

        if (player != null)
        {
            distance = (player.transform.position - transform.position).magnitude;
            if (distance > distance_attack && distance < distance_battlePhase && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                State = SlimeState.Battle;
            }

            delayCounter += Time.deltaTime;
            if (distance <= distance_attack && !attackCheck)
            {
                if (3 < delayCounter)
                {
                    State = SlimeState.Attack;
                }
            }
        }


        switch (State)
        {
            case SlimeState.Idle:
                moveWaitCount += Time.deltaTime;
                if (moveWaitCount > 3)
                {
                    State = SlimeState.Move;
                    destination = moveList[moveCount % 3];

                    nav.SetDestination(destination);
                    nav.isStopped = false;
                    moveWaitCount = 0;
                    moveCount++;
                }
                break;

            case SlimeState.Move:
                if ((transform.position - destination).magnitude < 0.2f)
                {
                    nav.isStopped = true;
                    State = SlimeState.Idle;
                }
                break;

            case SlimeState.Battle:
                if (distance > distance_battlePhase * 2)
                {
                    nav.isStopped = true;
                    State = SlimeState.Idle;
                    return;
                }
                nav.SetDestination(player.transform.position);
                nav.isStopped = false;
                if (distance < distance_attack)
                {
                    nav.isStopped = true;
                }
                break;

            case SlimeState.Attack:
                break;
        }
    }



    Coroutine active;
    public void LongRangeHit()
    {
        if (State != SlimeState.Die)
        {
            State = SlimeState.Battle;
            distance_battlePhase = 15;
            if (active != null)
            {
                StopCoroutine(active);
            }
            active = StartCoroutine(ReturnDistance());
        }
    }
    
    IEnumerator ReturnDistance()
    {
        yield return new WaitForSeconds(10);
        distance_battlePhase = 5;
        active = null;
    }



    
    float delayCounter;
    bool attackCheck;

    void AttackStart()
    {
        transform.LookAt(player.transform);
        attackCheck = true;
        StartCoroutine(ReturnBattle(3));
        nav.isStopped = true;
    }

    IEnumerator ReturnBattle(float attackDelay)
    {
        delayCounter = 0;

        while (attackDelay > delayCounter)
        {
            yield return null;

            if (distance > distance_attack)
            {
                yield return new WaitUntil(() => !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"));
                break;
            }
        }

        attackCheck = false;
    }

    public void ExitAttack()
    {
        State = SlimeState.Battle;
    }


}
