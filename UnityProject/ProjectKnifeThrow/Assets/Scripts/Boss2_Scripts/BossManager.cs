using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    GameObject player;

    [Header(("States"))]
    bool startSpawn = false;
    bool startLaser = false;
    bool startGuns = false;
    //Access to enemy spawners
    //Access to Laser spawners
    //Access to weakpoint containers
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
