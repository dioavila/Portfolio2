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
    [SerializeField] Transform spawn1;
    [SerializeField] Transform spawn2;


    [Header("Right Door Components")]
    [SerializeField] GameObject rightDoor;
    [SerializeField] Transform startingPositionR;
    [SerializeField] Transform finalPositionR;

    [Header("Left Door Components")]
    [SerializeField] GameObject leftDoor;
    [SerializeField] Transform startingPositionL;
    [SerializeField] Transform finalPositionL;

    [SerializeField] int doorOpenSpeed;
    private bool buttonPressed = false;
    //private bool noEnemies = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if(!noEnemies && enemy1 == null)
        //{
        //    noEnemies = true;
        //}
        if (buttonPressed)
        {
            rightDoor.transform.position = Vector3.Slerp(rightDoor.transform.position, finalPositionR.position, Time.deltaTime * doorOpenSpeed);
            leftDoor.transform.position = Vector3.Slerp(leftDoor.transform.position, finalPositionL.position, Time.deltaTime * doorOpenSpeed);
        }
        else
        {
            rightDoor.transform.position = Vector3.Slerp(rightDoor.transform.position, startingPositionR.position, Time.deltaTime * doorOpenSpeed);
            leftDoor.transform.position = Vector3.Slerp(leftDoor.transform.position, startingPositionL.position, Time.deltaTime * doorOpenSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            //destroyer.SetActive(true);
            Instantiate(enemyToSpawn, spawn1.position, spawn1.rotation);
            Instantiate(enemyToSpawn, spawn2.position, spawn2.rotation);
        }
    }
}
