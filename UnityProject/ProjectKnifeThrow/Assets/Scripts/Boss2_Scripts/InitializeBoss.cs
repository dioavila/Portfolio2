using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeBoss : MonoBehaviour
{
    bool soundOn = false;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.bossManager.startOff = true;
            if (!soundOn)
            {
                soundOn = true;
                AudioManager.instance.PlayMusic("Boss");
            }
        }
    }
}
