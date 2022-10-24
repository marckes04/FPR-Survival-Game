using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public enum EnemyState
    {
        PATROL,
        CHASE,
        ATTACK
    }


    private EnemyAnimator enemy_Anim;
    private NavMeshAgent navAgent;

    private EnemyState enemy_State;

    public float walk_speed = 0.5f;
    public float run_speed = 4f;


    public float chase_Distance = 7f;
    private float current_Chase_Distance;
    public float attack_Distance = 1.8f;
    public float chase_After_Attack_Distance = 2f;

    public float patrol_Radius_Min = 20f, patrol_Radius_Max = 60f;
    public float patrol_For_This_Time = 15f;
    private float patrol_Timmer;

    public float wait_Before_Attack = 2f;
    private float Attack_Timmer;

    private Transform target;

    public GameObject attack_Point;

    void Awake()
    {
        enemy_Anim = GetComponent<EnemyAnimator>();
        navAgent = GetComponent<NavMeshAgent>();

        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;

    }

    void Start()
    {
        enemy_State = EnemyState.PATROL;

        patrol_Timmer = patrol_For_This_Time;

        //When the enemy gets first to the player
        //Attack right away
        Attack_Timmer = wait_Before_Attack;

        //Memorize the value of chase distance
        //So that we could put it back
        current_Chase_Distance = chase_Distance;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy_State == EnemyState.PATROL)
        {
            Patrol();
        }

        if(enemy_State == EnemyState.CHASE)
        {
            Chase();
        }

        if (enemy_State == EnemyState.ATTACK)
        {
            Attack();
        }
    }

    void Patrol()
    {
        //Tell Nav Agent that he can move
        navAgent.isStopped = false;
        navAgent.speed = walk_speed;

        //add to the patrol timmer
        patrol_Timmer += Time.deltaTime;

        if(patrol_Timmer > patrol_For_This_Time)
        {
            SetNewRandomDestination();

            patrol_Timmer = 0f;
        }

        if(navAgent.velocity.sqrMagnitude > 0)
        {
            enemy_Anim.Walk(true);
        }
        else
        {
            enemy_Anim.Walk(false);
        }

        // test the distance between player and enemy

        if(Vector3.Distance(transform.position, target.position) <= chase_Distance)
        {
            enemy_Anim.Walk(false);

            enemy_State = EnemyState.CHASE;

            //Play spotted audio
        }

    }//Patrol

    void Chase()
    {
        //Enable the agent to move again
        navAgent.isStopped = false;
        navAgent.speed = run_speed;
        //Set player's position as destination
        //Because we are chasing(running towards) player
        navAgent.SetDestination(target.position);

        if (navAgent.velocity.sqrMagnitude > 0)
        {
            enemy_Anim.Run(true);
        }
        else
        {
            enemy_Anim.Run(false);
        }

        //if the distance between player and enemy is less than attack distance
        if (Vector3.Distance(transform.position, target.position) <= attack_Distance)
        {
            //Stop the animations

            enemy_Anim.Run(false);
            enemy_Anim.Walk(false);
            enemy_State = EnemyState.ATTACK;


            // reset the chase distance to previous value
            if (chase_Distance != current_Chase_Distance)
            {
                chase_Distance = current_Chase_Distance;
            }
        }
        else if (Vector3.Distance(transform.position, target.position) > chase_Distance)
        {
            //Player run away from enemy

            // Stop Running 
            enemy_Anim.Run(false);

            enemy_State = EnemyState.PATROL;

            // reset the patrol timmer so that function
            //can calculate the new patrol destination right way
            patrol_Timmer = patrol_For_This_Time;

            if(chase_Distance != current_Chase_Distance)
            {
                chase_Distance = current_Chase_Distance;
            }
        }

    }//chase

    void Attack()
    {
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;

        Attack_Timmer += Time.deltaTime;

        if(Attack_Timmer > wait_Before_Attack)
        {
            enemy_Anim.Attack();

            Attack_Timmer = 0f;

            // Play Attack sound
        }

        if(Vector3.Distance(transform.position, target.position) > attack_Distance + chase_After_Attack_Distance)
        {
            enemy_State = EnemyState.CHASE;
        }
    }

    void SetNewRandomDestination()
    {
        float ran_radius = Random.Range(patrol_Radius_Min, patrol_Radius_Max);

        Vector3 ranDir = Random.insideUnitSphere * ran_radius;
        ranDir += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(ranDir, out navHit, ran_radius,-1);

        navAgent.SetDestination(navHit.position);

    }

    void Turn_On_AttackPoint()
    {
        attack_Point.SetActive(true);
    }

    void Turn_Off_AttackPoint()
    {
        if (attack_Point.activeInHierarchy)
        {
            attack_Point.SetActive(false);
        }
    }

}
