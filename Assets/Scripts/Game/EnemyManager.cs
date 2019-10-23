using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager instance;

    [SerializeField]
    private GameObject boar_Prefab, cannibal_Prefab;

    public Transform[] cannibal_Spawn_Points, boar_Spawn_Points;

    [SerializeField]
    private int cannibal_Count, boar_Count;

    private int initial_Cannibal_Count, initial_Boar_Count;

    public float wait_Before_Spwan_Enemies_Time = 10f;
    
    void Awake()
    {
        MakeInstance();
    }

    void Start()
    {
        initial_Cannibal_Count = cannibal_Count;
        initial_Boar_Count = boar_Count;

        SpawnEnemies();

        StartCoroutine("CheckToSpawnEnemies");
    }

    
    IEnumerator CheckToSpawnEnemies()
    {

        yield return new WaitForSeconds(wait_Before_Spwan_Enemies_Time);

        SpawnCannibals();
        SpawnBoars();

        StartCoroutine("CheckToSpawnEnemies");
    }

    public void StopSpawning()
    {
        StopCoroutine("CheckToSpawnEnemies");
    }


    void SpawnEnemies()
    {
        SpawnCannibals();
        SpawnBoars();
    }

    void SpawnCannibals()
    {
        int index = 0;

        for(int i = 0; i < cannibal_Count; i++)
        {

            if(index >= cannibal_Spawn_Points.Length)
            {
                index = 0;
            }

            Instantiate(cannibal_Prefab, cannibal_Spawn_Points[index].position, Quaternion.identity);

            index++;
        }

        // spawn后就置零，若死了N个敌人就会变成N（EnemyDied），下次spawn时就会spawn N个出来
        cannibal_Count = 0;
    }

    void SpawnBoars()
    {
        int index = 0;
        for (int i = 0; i < boar_Count; i++)
        {

            if (index >= boar_Spawn_Points.Length)
            {
                index = 0;
            }

            Instantiate(boar_Prefab, boar_Spawn_Points[index].position, Quaternion.identity);

            index++;
        }

        boar_Count = 0;
    }


    public void EnemyDied(bool isCannibal)
    {
        if (isCannibal)
        {
            cannibal_Count++;
            if(cannibal_Count > initial_Cannibal_Count)
            {
                cannibal_Count = initial_Cannibal_Count;
            }
        }
        else
        {
            boar_Count++;
            if (boar_Count > initial_Boar_Count)
            {
                boar_Count = initial_Boar_Count;
            }
        }
    }

    void MakeInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    
}
