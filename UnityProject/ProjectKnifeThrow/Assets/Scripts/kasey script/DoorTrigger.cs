using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] GameObject Door;
    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            Door.transform.position += new Vector3(0, 4, 0);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            Door.transform.position -= new Vector3(0, 4, 0);
        }
    }
}
