using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTurnIn : MonoBehaviour
{
    public bool keyIsInserted = false;
    public bool has3Keys = false;
    wallRun playerInv;
    bool isPlayerInRange = false;

    // Start is called before the first frame update
    void Start()
    {
        playerInv = GameManager.instance.playerScript;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInv.keys.Count == 3) // Check if the player has 3 keys
        {
            has3Keys = true;
        }

        if (isPlayerInRange)
        {
            if (has3Keys)
            {
                GameManager.instance.OpenAcceptPanel("");

                if (Input.GetKeyDown(KeyCode.F))
                {
                    playerInv.keys.Clear();
                    GameManager.instance.CloseAcceptPanel("");
                    keyIsInserted = true;
                }
            }
            else
            {
                GameManager.instance.OpenRejectPanel("");
            }
        }

        if (keyIsInserted)
        {
            Destroy(GameObject.FindWithTag("KeyDoor"));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            GameManager.instance.CloseAcceptPanel("");
            GameManager.instance.CloseRejectPanel("");
        }
    }
}
