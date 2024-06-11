using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserObstacle : MonoBehaviour
{
    [SerializeField] GameObject laserStart;
    [SerializeField] GameObject laserEnd;
    [SerializeField] ParticleSystem laserEffect;

    bool killPlayer = false;

    // Update is called once per frame
    void Update()
    {
        if (!killPlayer)
            playerDetect();
    }

    void playerDetect()
    {
        RaycastHit hit;
        if (Physics.Raycast(laserStart.transform.position, laserStart.transform.forward, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                GameManager.instance.playerScript.TakeDamage(1000);
                killPlayer = true;
            }
        }
    }
}
