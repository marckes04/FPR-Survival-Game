using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField]
    private GameObject boar_prefab, cannibal_prefab;

    public Transform[] canibal_SpawnPoints, boar_SpawnPoints;

    [SerializeField]
    private int cannibal_Enemy_Count, boar_Enemy_Count;

    private int initial_Canibal_Count, initial_Boar_Count;

    public float Wait_Before_Spawn_Enemies_Time = 10f;

    void Awake()
    {
        MakeInstance();
    }

    void Start()
    {
        initial_Canibal_Count = cannibal_Enemy_Count;
        initial_Boar_Count = boar_Enemy_Count;

        SpawnEnemies();

        StartCoroutine("CheckToSpawnEnemies");
    }

    void MakeInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void SpawnEnemies()
    {
        SpawnCannibals();
        SpawnBoars();
    }

    void SpawnCannibals()
    {
        int index = 0;

        for(int i =0; i< cannibal_Enemy_Count; i++)
        {
            if(index >= canibal_SpawnPoints.Length)
            {
                index = 0;
            }
            
            Instantiate(cannibal_prefab, canibal_SpawnPoints[index].position, Quaternion.identity);
            index++;
        }

        cannibal_Enemy_Count = 0;
    }

    void SpawnBoars()
    {
        int index = 0;

        for (int i = 0; i < boar_Enemy_Count; i++)
        {
            if (index >= boar_SpawnPoints.Length)
            {
                index = 0;
            }

            Instantiate(boar_prefab, boar_SpawnPoints[index].position, Quaternion.identity);
            index++;
        }

        boar_Enemy_Count = 0;
    }

    IEnumerator CheckToSpawnEnemies()
    {
        yield return new WaitForSeconds(Wait_Before_Spawn_Enemies_Time);

        SpawnCannibals();
        SpawnBoars();

        StartCoroutine("CheckToSpawnEnemies");
    }

    public void EnemyDied(bool cannibal)
    {
        if (cannibal)
        {
            cannibal_Enemy_Count++;

            if(cannibal_Enemy_Count > initial_Canibal_Count)
            {
                cannibal_Enemy_Count = initial_Canibal_Count;
            }
        }
        else
        {
            boar_Enemy_Count++;

            if(boar_Enemy_Count > initial_Boar_Count)
            {
                boar_Enemy_Count = initial_Boar_Count;
            }
        }
    }

    public void StopSpawning()
    {
        StopCoroutine(" CheckToSpawnEnemies");
    }

}
