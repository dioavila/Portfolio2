using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This Trigger/Switch works when player or enemy steps into trigger/pressure plate
public class DoorTrigger : MonoBehaviour
{
    [SerializeField] GameObject Door;

    [SerializeField] Vector3 Position;

    Renderer Model;

    bool Active;


    // Start is called before the first frame update
    private void Start()
    {
        Model = GetComponent<Renderer>();
        Active = false;
    }

    private void Update()
    {
        if (Active)
        {
            Model.material.color = Color.green;
        }

        if (!Active)
        {
            Model.material.color = Color.red;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            Active = true;
            Door.transform.position += new Vector3(0, 4, 0);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            Active = false;
            Door.transform.position -= new Vector3(0, 4, 0);
        }
    }
}
