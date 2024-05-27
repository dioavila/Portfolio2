using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interact_Button : MonoBehaviour
{
    bool playerInRange = false;
    public bool openSesame = false;
    // Update is called once per frame
    void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PerformAction();
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
