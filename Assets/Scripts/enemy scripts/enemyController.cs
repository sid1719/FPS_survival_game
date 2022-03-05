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
public class enemyController : MonoBehaviour
{
    private enemyAnnimator enemy_anim;
    private NavMeshAgent navAgent;

    private EnemyState enemy_state;

    public float walk_speed = 0.5f;
    public float run_speed = 4f;

    public float chase_dist = 12f;
    private float current_chase_dist;
    public float attack_dist = 2.2f;
    public float chase_after_attack_dist = 2f;

    public float patrol_radius_min = 20f, patrol_radius_max = 60f;
    public float patrol_for_this_time = 15f;
    private float patrol_timer;

    public float wait_before_attack = 2f;
    private float attack_timer;

    private Transform target;

    public GameObject attack_Point;

    private enemAudio enemy_Audio;
    private void Awake()
    {
        enemy_anim = GetComponent<enemyAnnimator>();
        navAgent = GetComponent<NavMeshAgent>();

        target=GameObject.FindWithTag(Tags.PLAYER_TAG).transform;

        enemy_Audio = GetComponentInChildren<enemAudio>();
    }
    // Start is called before the first frame update
    void Start()
    {
        enemy_state = EnemyState.PATROL;

        patrol_timer = patrol_for_this_time;

        //when the enemy first gets to the player,attack right away
        attack_timer = wait_before_attack;

        //memorize the value of chase dist so that we can put it back
        current_chase_dist = chase_dist;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy_state==EnemyState.PATROL)
        {
            Patrol();
        }
        if(enemy_state==EnemyState.CHASE)
        {
            Chase();
        }
        if(enemy_state==EnemyState.ATTACK)
        {
            Attack();
        }
    }

    void Patrol()
    {   //Tell agent it can move
        navAgent.isStopped = false;
        navAgent.speed = walk_speed;

        patrol_timer += Time.deltaTime;

        if(patrol_timer>patrol_for_this_time)
        {
            SetNewRandomDest();
            patrol_timer = 0f;
        }

        if(navAgent.velocity.sqrMagnitude>0)
        {
            enemy_anim.Walk(true);
        }
        else
        {
            enemy_anim.Walk(false);
        }

        //test the dist between player and enemy

        if(Vector3.Distance(transform.position,target.position)<= chase_dist)
        {
            enemy_anim.Walk(false);
            enemy_state = EnemyState.CHASE;

            //play spotted audio
            enemy_Audio.Play_ScreamSound();
        }
    }

    void Chase()
    {
        navAgent.isStopped = false;
        navAgent.speed = run_speed;

        //set player position as destination as we are chasing the player
        navAgent.SetDestination(target.position);

        if (navAgent.velocity.sqrMagnitude > 0)
        {
            enemy_anim.Run(true);
        }
        else
        {
            enemy_anim.Run(false);
        }

        //if dist of enemy is less than attack dist
        if(Vector3.Distance(transform.position,target.position)<= attack_dist)
        {
            enemy_anim.Run(false);
            enemy_anim.Walk(false);
            enemy_state = EnemyState.ATTACK;

            if(chase_dist!=current_chase_dist)
            {
                chase_dist = current_chase_dist;
            }
        }
        else if(Vector3.Distance(transform.position,target.position)> chase_dist)
        {
            //stop running
            enemy_anim.Run(false);
            enemy_state = EnemyState.PATROL;


            //reset patrol timer so that is can calculate the new patrol dist
            patrol_timer = patrol_for_this_time;

            if (chase_dist != current_chase_dist)
            {
                chase_dist = current_chase_dist;
            }
        }

    }
    void Attack()
    {
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;

        attack_timer += Time.deltaTime;

        if(attack_timer> wait_before_attack)
        {
            enemy_anim.Attack();

            attack_timer = 0f;

            enemy_Audio.Play_AttackSound();
            //play attack sound
        }

        if(Vector3.Distance(transform.position,target.position) > attack_dist+chase_after_attack_dist)
        {
            enemy_state = EnemyState.CHASE;
        }
    }

    void SetNewRandomDest()
    {
        float ran_radius = Random.Range(patrol_radius_min, patrol_radius_max);

        Vector3 randDir = Random.insideUnitSphere * ran_radius;
        randDir += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDir, out navHit, ran_radius, -1);

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

    public EnemyState Enemy_State
    {
        get; set;
    }
}
