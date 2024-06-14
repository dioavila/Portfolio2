using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMov : MonoBehaviour
{
    [SerializeField] int doorOpenSpeed;
    
    [Header("Right Door Components")]
    [SerializeField] GameObject rightDoor;
    [SerializeField] Transform startingPositionR;
    [SerializeField] Transform finalPositionR;

    [Header("Left Door Components")]
    [SerializeField] GameObject leftDoor;
    [SerializeField] Transform startingPositionL;
    [SerializeField] Transform finalPositionL;

    //[SerializeField] Transform spawnPath;
    //public GameObject enemy;

    [SerializeField] Transform exitDoorPoint;
    public bool charIn = false;

    [Header("Laser Settings")]
    [SerializeField] bool isLaser = false;
    [SerializeField] Transform laserReadyPoint;
    [SerializeField] Transform laserExitPoint;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (charIn)
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

    void OnTriggerEnter(Collider other)
    {
        if (!isLaser)
        {
            if (other.CompareTag("Enemy"))
            {
                //other.GetComponent<enemyAI>().spawnPath = spawnPath;
            }

            if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            {
                if (other.isTrigger)
                {
                    return;
                }
                //other.GetComponent<enemyAI>().spawnPath = exitDoorPoint;
                charIn = true;

            }
        }
        else
        {
            if (other.CompareTag("Laser"))
            {
                if (other.isTrigger)
                {
                    return;
                }
                charIn = true;
                other.GetComponent<LaserWallMovement>().readyPoint = laserReadyPoint;
                other.GetComponent<LaserWallMovement>().endPoint = laserExitPoint;

            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (!isLaser)
        {
            if (other.gameObject.CompareTag("Player") || other.CompareTag("Enemy"))
            {
                charIn = false;
            }
        }
        else
        {
            if (other.CompareTag("Laser"))
            {
                charIn = false;
            }
        }
    }
}
