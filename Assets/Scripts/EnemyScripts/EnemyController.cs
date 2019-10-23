using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    PATROL,
    CHASE,
    ATTACK
}

public class EnemyController : MonoBehaviour
{

    private EnemyAnimator enemy_Anim;
    private NavMeshAgent navAgent;

    private EnemyState enemy_State;

    // ***speed
    public float walk_Speed = 0.5f;
    public float run_Speed = 4f;

    // ***distance
    public float chase_Distance = 7f;
    private float normal_Chase_Distance;
    public float attack_Distance = 1.8f;
    public float chase_After_Attack_Distance = 2f;

    // ***patrol
    public float patrol_Radius_Min = 20f, patrol_Radius_Max = 60f;
    // after this time change a new destination to patrol
    public float patrol_For_This_Time = 15f;
    private float patrol_Timer;

    // ***attack
    public float wait_Before_Attack = 2f;
    private float attack_Timer;

    private Transform target;

    // hit point
    public GameObject attack_Point;


    // enemy audio
    private EnemyAudio enemy_Audio;

    void Awake()
    {
        enemy_Anim = GetComponent<EnemyAnimator>();
        navAgent = GetComponent<NavMeshAgent>();

        enemy_Audio = GetComponentInChildren<EnemyAudio>();

        target = GameObject.FindWithTag(Tag.PLAYER_TAG).transform;
    }

    void Start()
    {
        navAgent.isStopped = false;
        navAgent.speed = walk_Speed;

        enemy_State = EnemyState.PATROL;
        normal_Chase_Distance = chase_Distance;
        patrol_Timer = patrol_For_This_Time;
        attack_Timer = wait_Before_Attack;
        
    }

 
    void Update()
    {
        if(enemy_State == EnemyState.PATROL)
        {
            Patrol();
        }

        if (enemy_State == EnemyState.CHASE)
        {
            Chase();
        }

        if (enemy_State == EnemyState.ATTACK)
        {
            Attack();
        }
    }


    // patrol func
    void Patrol()
    {
        patrol_Timer += Time.deltaTime;
        if (patrol_Timer > patrol_For_This_Time)
        {
            SetNewRandomDestination();
            patrol_Timer = 0;
        }

        // set walk animation
        if(navAgent.velocity.sqrMagnitude > 0)
        {
            enemy_Anim.Walk(true);
        }
        else
        {
            enemy_Anim.Walk(false);
        }

        // calculate the distance between palyer and enemy
        if(Vector3.Distance(transform.position, target.position) <= chase_Distance)
        {
            enemy_Anim.Walk(false);
            enemy_State = EnemyState.CHASE;

            // play spotted audio
            enemy_Audio.PlayScreamSound();
        }
    }
    void SetNewRandomDestination()
    {
        float rand_Radius = Random.Range(patrol_Radius_Min, patrol_Radius_Max);
        Vector3 patrol_Dir = Random.insideUnitSphere * rand_Radius;
        patrol_Dir += transform.position;

        NavMeshHit navHit;
        if(NavMesh.SamplePosition(patrol_Dir, out navHit, rand_Radius, -1))
        {
            navAgent.SetDestination(navHit.position);
        }
        else
        {
            SetNewRandomDestination();
        }
    }

    // chase func
    void Chase()
    {
        navAgent.isStopped = false;
        navAgent.speed = run_Speed;

        navAgent.SetDestination(target.position);

        if(navAgent.velocity.sqrMagnitude > 0)
        {
            enemy_Anim.Run(true);
        }
        else
        {
            enemy_Anim.Run(false);
        }

        // attack when within attack distance
        if(Vector3.Distance(transform.position, target.position) <= attack_Distance)
        {
            enemy_Anim.Run(false);
            enemy_Anim.Walk(false);
            enemy_State = EnemyState.ATTACK;

            if(chase_Distance != normal_Chase_Distance)
            {
                chase_Distance = normal_Chase_Distance;
            }
        }
        // when lose target change to patrol
        else if(Vector3.Distance(transform.position, target.position) > chase_Distance)
        {
            enemy_Anim.Run(false);
            enemy_State = EnemyState.PATROL;
            patrol_Timer = patrol_For_This_Time;

            if (chase_Distance != normal_Chase_Distance)
            {
                chase_Distance = normal_Chase_Distance;
            }
        }
    }

    // attack func
    void Attack()
    {
        // stop enemy
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;

        attack_Timer += Time.deltaTime;
        if(attack_Timer > wait_Before_Attack)
        {
            enemy_Anim.Attack();
            attack_Timer = 0f;
            enemy_Audio.PlayAttackSound();
        }
        
        if(Vector3.Distance(transform.position, target.position) > attack_Distance + chase_After_Attack_Distance)
        {
            enemy_State = EnemyState.CHASE;
        }
    }

    public void Turn_On_AttackPoint()
    {
        attack_Point.SetActive(true);
    }
    public void Turn_Off_AttackPoint()
    {
        if (attack_Point.activeInHierarchy)
        {
            attack_Point.SetActive(false);
        }
    }

    public EnemyState Enemy_State
    {
        get; set;
    }
}
