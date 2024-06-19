using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainFabricatorElevator : MonoBehaviour
{
    [Header("Spawner")]
    //[SerializeField] GameObject destoryer;
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] GameObject enemyToSpawn2;
    [SerializeField] Transform spawn1;
    [SerializeField] Transform spawn2;
    [SerializeField] Transform spawn3;

    bool enemySpawned;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawned = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enemySpawned)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            //destroyer.SetActive(true);
            Instantiate(enemyToSpawn, spawn1.position, spawn1.rotation);
            Instantiate(enemyToSpawn, spawn2.position, spawn2.rotation);
            Instantiate(enemyToSpawn2, spawn3.position, spawn3.rotation);
            enemySpawned=false;
        }
    }
}
