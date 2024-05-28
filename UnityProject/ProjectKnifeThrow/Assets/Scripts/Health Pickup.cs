using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour, IPickup
{

    public wallRun player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PickUpItem()
    {
        if (player != null)
        {
            player.HP = player.startingHP;
            gameObject.SetActive(false);
            player.updatePlayerUI();
        }
    }
}
