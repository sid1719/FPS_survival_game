using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class healthScript : MonoBehaviour
{
    private enemyAnnimator enemy_anim;
    private NavMeshAgent navAgent;
    private enemyController enemy_controller;

    public float health = 100f;

    public bool is_player, is_boar, is_cannibal;

    private bool is_Dead;

    private enemAudio enemyAudio;

    private playerStats player_stats;

    // Start is called before the first frame update
    void Awake()
    {
        if(is_boar || is_cannibal)
        {
            enemy_anim = GetComponent<enemyAnnimator>();
            enemy_controller = GetComponent<enemyController>();
            navAgent = GetComponent<NavMeshAgent>();

            //enemy audio
            enemyAudio = GetComponentInChildren<enemAudio>();
        }
        if(is_player)
        {
            player_stats = GetComponent<playerStats>();
        }

    }

   public void ApplyDamage(float damage)
    {
        if (is_Dead)
            return;
        health -= damage;
        Debug.Log(health);
        if(is_player)
        {
            player_stats.Display_HealthStats(health);
        }
        if(is_boar || is_cannibal)
        {
            if(enemy_controller.Enemy_State ==EnemyState.PATROL)
            {
                enemy_controller.chase_dist = 50f;
            }
        }
        if(health<=0f)
        {
            playerdied();

            is_Dead = true;
        }
    }

    void playerdied()
    {
        if(is_cannibal)
        {
            
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
            //GetComponent<Rigidbody>().AddTorque(-transform.forward * 10f);

         
            enemy_anim.enabled = false;
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            enemy_controller.enabled = false;
            //start coroutine
            EnemyManagerScript.instance.EnemyDied(true);
            //enemy manager spawn more enemies
        }
        if(is_boar)
        {
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            enemy_controller.enabled = false;

            enemy_anim.Dead();

            //start coroutine
            StartCoroutine(DeadSound());
            //enemy manager spawn more enemies
            EnemyManagerScript.instance.EnemyDied(false);
        }

        if(is_player)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);

            for(int i=0;i<enemies.Length;i++)
            {
                enemies[i].GetComponent<enemyController>().enabled = false;

            }
            //call enemy manager to stop spawing eneimes
            EnemyManagerScript.instance.stopSpawn();
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<weaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);
        }
        if(tag==Tags.PLAYER_TAG)
        {
            Invoke("RestartGame", 3f);
        }
        else
        {
            Invoke("TurnOffGameObject", 3f);
        }
    }//player died

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("scene1");
    }
    void TurnOffGameObject()
    {
        gameObject.SetActive(false);
    }
    IEnumerator DeadSound()
    {
        yield return new WaitForSeconds(0.3f);
        enemyAudio.Play_DeadSound();
    }
}
