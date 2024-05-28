using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour, IPickup
{
    wallRun playerInv;

    // Start is called before the first frame update
    void Start()
    {
        playerInv = GameManager.instance.playerScript;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUpItem()
    {
        if (playerInv != null)
        {
            playerInv.keys.Add(gameObject);
            gameObject.SetActive(false);
        }
    }

}
