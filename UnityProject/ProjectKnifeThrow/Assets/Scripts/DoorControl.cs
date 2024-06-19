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

    [SerializeField] Light lightDoor;
    [SerializeField] Renderer lightPrefab;
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
            door.transform.position = Vector3.Lerp(door.transform.position, finalPosition.position, Time.deltaTime * doorOpenSpeed);
            lightDoor.color = Color.green;
            lightPrefab.material.SetColor("_Color", Color.green);
            lightPrefab.material. SetColor("_EmissionColor", Color.green);
        }
        else
        {
            door.transform.position = Vector3.Lerp(door.transform.position, startingPosition.position, Time.deltaTime * doorOpenSpeed);
        }
    }
}
