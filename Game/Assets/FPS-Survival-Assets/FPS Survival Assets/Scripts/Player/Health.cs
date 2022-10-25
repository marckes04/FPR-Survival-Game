using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyController;

public class Health : MonoBehaviour
{
    private EnemyAnimator enemy_Anim;
    private NavMeshAgent navAgent;
    private EnemyController enemy_Controller;

    public float health = 100f;

    public bool is_Player, is_Boar, is_Cannibal;
    private bool is_Dead;

    private EnemyAudio enemyAudio;
   
    void Awake()
    {
        if(is_Boar || is_Cannibal)
        {
            enemy_Anim = GetComponent<EnemyAnimator>();
            enemy_Controller = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();

            // get enemy Audio 
            enemyAudio = GetComponentInChildren<EnemyAudio>();
        }
        if (is_Player)
        {

        }
    }

    public void ApplyDamage(float damage)
    {
        //if we died don't excute the rest of code
        if(is_Dead)
            return;
        health -= damage;

        if (is_Player)
        {
            //Show the stats(Display the UI value)
        }
          if (is_Boar ||  is_Cannibal)
        {
            if(enemy_Controller.Enemy_State == EnemyState.PATROL)
            {
                enemy_Controller.chase_Distance = 50f;
            }
        }

        if(health <= 0f)
        {
           PlayerDied();

            is_Dead = true;
        }
    }//Apply damage

    void PlayerDied()
        {
            if (is_Cannibal)
            {
                GetComponent < Animator>().enabled = false;
                GetComponent < BoxCollider>().isTrigger = false;
                GetComponent<Rigidbody>().AddTorque(-transform.forward * 50f);

                enemy_Controller.enabled = false;
                navAgent.enabled = false;
                enemy_Anim.enabled = false;
                StartCoroutine(DeadSound());
            //StartCoroutine

            //EnemyManager spawn more enemies
        }

        if (is_Boar)
        {
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            enemy_Controller.enabled = false;

            enemy_Anim.Dead();
            StartCoroutine(DeadSound());
        }

        if (is_Player)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<EnemyController>().enabled = false;
            }

            // call enemy manager to stop spawning enemies

            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);
        }
            if(tag == Tags.PLAYER_TAG)
            {
            Invoke("RestartGame", 3f);
        }
        else
        {
            Invoke("TurnOffGameObject", 3f);
        }
        

     }//Player Died

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
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