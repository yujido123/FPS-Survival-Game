using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Race
{
    HUMAN,
    BOAR,
    CANNIBAL
}

public class HealthScript : MonoBehaviour
{

    private EnemyAnimator enemy_Anim;
    private EnemyController enemy_Controller;
    private NavMeshAgent navAgent;

    private float health;
    public float maxHealth = 100f;
    public Race race;

    private bool is_Dead;

    private EnemyAudio enemy_Audio;

    // show health UI
    private PlayerStats player_Stats;

    void Awake()
    {
        if(IsEnemy())
        {
            enemy_Anim = GetComponent<EnemyAnimator>();
            enemy_Controller = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();

            enemy_Audio = GetComponentInChildren<EnemyAudio>();
        }
        else if(IsPlayer())
        {
            player_Stats = GetComponent<PlayerStats>();
        }
    }

    void Start()
    {
        health = maxHealth;
    }



    public void ApplyDamage(float damage)
    {
        if (is_Dead)
            return;

        health -= damage;

        if (IsPlayer())
        {
            // show health UI
            player_Stats.DisplayHealthStats(health, maxHealth);
        }

        if (IsEnemy())
        {
            if(enemy_Controller.Enemy_State == EnemyState.PATROL)
            {
                enemy_Controller.chase_Distance = 50f;
            }
        }

        if(health <= 0)
        {
            PlayerDied();
            is_Dead = true;
        }
    }

    void PlayerDied()
    {
        if(race == Race.CANNIBAL)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().AddTorque(-transform.forward * 10f);

            enemy_Controller.enabled = false;
            navAgent.enabled = false;
            enemy_Anim.enabled = false;

            EnemyManager.instance.EnemyDied(true);

            StartCoroutine(DeadSound());
        }

        if(race == Race.BOAR)
        {
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            enemy_Controller.enabled = false;

            enemy_Anim.Dead();

            EnemyManager.instance.EnemyDied(false);

            StartCoroutine(DeadSound());
        }

        if (race == Race.HUMAN)
        {

            // player die, stop all enemy
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tag.ENEMY_TAG);
            for(int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<EnemyController>().enabled = false;
            }

            EnemyManager.instance.StopSpawning();

            // stop player action, weapon
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentWeapon().gameObject.SetActive(false);
        }


        if (tag == Tag.PLAYER_TAG)
        {
            Invoke("ReadyRestart", 3f);
        }
        else
        {
            Invoke("TurnOffGameObject", 3f);
        }
    }

    void ReadyRestart()
    {
        GameManager.RestartGame();
    }


    void TurnOffGameObject()
    {
        gameObject.SetActive(false);
    }

    bool IsPlayer()
    {
        return race == Race.HUMAN;
    }

    bool IsEnemy()
    {
        return race == Race.BOAR || race == Race.CANNIBAL; 
    }


    // play dead sound
    IEnumerator DeadSound()
    {
        yield return new WaitForSeconds(0.3f);
        enemy_Audio.PlayDeadSound();
    }

}
