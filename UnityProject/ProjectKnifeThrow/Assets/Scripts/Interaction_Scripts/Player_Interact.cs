using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interact_Button : MonoBehaviour
{
    bool playerInRange = false;
    [SerializeField] GameObject objectToMove;
    [SerializeField] Transform moveTo;
    public bool openSesame = false;

    float moveRate = 1.0f;
    // Update is called once per frame
    void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PerformAction();
        }

        if (openSesame)
        {
            objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, new Vector3(objectToMove.transform.position.x, 
                moveTo.transform.position.y, objectToMove.transform.position.z), Time.deltaTime* moveRate);
        }
    }

    void PerformAction()
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.SetColor("_Color", Color.blue);
        openSesame = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

}
