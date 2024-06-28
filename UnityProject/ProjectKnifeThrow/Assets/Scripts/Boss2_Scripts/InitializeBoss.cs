using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeBoss : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.bossManager.startOff = true;
            AudioManager.instance.PlayMusic("Boss");
        }
    }
}
