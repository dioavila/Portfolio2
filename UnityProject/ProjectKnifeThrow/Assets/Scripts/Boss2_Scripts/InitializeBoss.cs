using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeBoss : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.bossManager.startOff = true;
        }
    }
}
