using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{

    [Header("Door Components")]
    [SerializeField] int doorOpenSpeed;
    [SerializeField] GameObject door;
    [SerializeField] Transform startingPosition;
    [SerializeField] Transform finalPosition;

    [SerializeField] Transform exitDoorPoint;
    public bool clearToOpen = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (clearToOpen)
        {
            door.transform.position = Vector3.Slerp(door.transform.position, finalPosition.position, Time.deltaTime * doorOpenSpeed);
        }
        else
        {
            door.transform.position = Vector3.Slerp(door.transform.position, startingPosition.position, Time.deltaTime * doorOpenSpeed);
        }
    }
}
