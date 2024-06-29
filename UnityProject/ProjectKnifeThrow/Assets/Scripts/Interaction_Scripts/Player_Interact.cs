using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interact_Button : MonoBehaviour
{
    [Header("Object Settings")]
    [SerializeField] bool movesObject = true;
    [SerializeField] bool startStopLaser = false;
    [SerializeField] bool isConsole = false;

    [Header("Movement Settings")]
    [SerializeField] GameObject doorToOpen;
    //[SerializeField] Transform moveTo;
    public bool openSesame = false;

    [Header("Laser Settings")]
    [SerializeField] public List<GameObject> laserSet;
    bool playerInRange = false;

    //float moveRate = 1.0f;
    // Update is called once per frame
    void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            PerformAction();
            GameManager.instance.CloseMessagePanel("");
        }

        if (openSesame)
        {
            //objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, new Vector3(objectToMove.transform.position.x, 
            //    moveTo.transform.position.y, objectToMove.transform.position.z), Time.deltaTime* moveRate);
            doorToOpen.GetComponent<DoorControl>().clearToOpen = true;
            openSesame = false;
        }
    }

    void PerformAction()
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        if (!isConsole)
        {
            rend.material.SetColor("_Color", Color.blue);
        }
        else
        {
            rend.material.SetColor("_EmissionColor", Color.black);
        }

        if(movesObject) 
        { 
            openSesame = true;
        }

        if (startStopLaser)
        {
            for(int laserListiter = 0; laserListiter < laserSet.Count; ++laserListiter)
            {
                laserSet[laserListiter].SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            GameManager.instance.OpenMessagePanel("");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            GameManager.instance.CloseMessagePanel("");
        }
    }

}
