using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour
{
    public static EnemyManagerScript instance;

    [SerializeField]
    private GameObject boar_prefab, cannibal_prefab;

    public Transform[] csp, bsp;

    [SerializeField]
    private int cannibal_Enemy_Count, boar_Enemy_Count;

    private int initial_Cannibal_count,initial_boar_count;

    public float wait_before_spawn = 10f;

    private void Awake()
    {
        MakeInstance();
    }

    private void Start()
    {
        initial_Cannibal_count = cannibal_Enemy_Count;
        initial_boar_count = boar_Enemy_Count;

        spawnEnemies();

        StartCoroutine("CheckToSpawn");
    }
    void MakeInstance()
    {
        if(instance==null)
        {
            instance = this;
        }
    }

    void spawnEnemies()
    {
        spawnCannibals();
        spawnboar();
    }

    void spawnCannibals()
    {
        int index = 0;

        for(int i=0;i<cannibal_Enemy_Count;i++)
        {
            if(index>=csp.Length)
            {
                index = 0;
            }
            Instantiate(cannibal_prefab, csp[index].position, Quaternion.identity);
            index++;
        }
        cannibal_Enemy_Count = 0;
    }
    void spawnboar()
    {
        int index = 0;

        for (int i = 0; i < boar_Enemy_Count; i++)
        {
            if (index >= bsp.Length)
            {
                index = 0;
            }
            Instantiate(boar_prefab, bsp[index].position, Quaternion.identity);
            index++;
        }
        boar_Enemy_Count = 0;

    }

    IEnumerator CheckToSpawn()
    {
        yield return new WaitForSeconds(wait_before_spawn);
        spawnCannibals();
        spawnboar();

        StartCoroutine("CheckToSpawn");
    }
    public void EnemyDied(bool cannibal)
    {
        if(cannibal)
        {
            cannibal_Enemy_Count++;

            if(cannibal_Enemy_Count>initial_Cannibal_count)
            {
                cannibal_Enemy_Count = initial_Cannibal_count;
            }
        }
        else 
        {
            boar_Enemy_Count++;

            if (boar_Enemy_Count > initial_boar_count)
            {
                boar_Enemy_Count = initial_boar_count;
            }
        }
    }

    public void stopSpawn()
    {
        StopCoroutine("CheckToSpawn");
    }
}
