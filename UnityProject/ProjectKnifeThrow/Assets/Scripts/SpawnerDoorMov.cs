using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerDoorMov : MonoBehaviour
{
    [SerializeField] int doorOpenSpeed;
    
    [Header("Right Door Components")]
    [SerializeField] GameObject spawnDoor;
    [SerializeField] Transform startingPosition;
    [SerializeField] Transform finalPosition;
    public bool charIn = false;
    //bool doorOpen = false;
    bool doorClosed = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (charIn)
        {
            if (doorClosed)
            {
                AudioSource sound = spawnDoor.GetComponent<AudioSource>();
                sound.pitch = 1f;
                sound.PlayOneShot(sound.clip, sound.volume);
                doorClosed = false;
            }
            spawnDoor.transform.position = Vector3.Lerp(spawnDoor.transform.position, finalPosition.position, Time.deltaTime * doorOpenSpeed);
        }
        else
        {
            if(!doorClosed)
            {
                AudioSource sound = spawnDoor.GetComponent<AudioSource>();
                sound.pitch = 0.6f;
                sound.PlayOneShot(sound.clip, sound.volume);
                doorClosed = true;
            }
            spawnDoor.transform.position = Vector3.Lerp(spawnDoor.transform.position, startingPosition.position, Time.deltaTime * doorOpenSpeed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            if (other.isTrigger)
            {
                return;
            }
            charIn = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
            charIn = false;
    }
}
