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


    public bool enemyIn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyIn)
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
        if (other.CompareTag("Enemy"))
        {
            if (other.isTrigger)
            {
                return;
            }
            enemyIn = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemyIn = false;
        }
    }
}
