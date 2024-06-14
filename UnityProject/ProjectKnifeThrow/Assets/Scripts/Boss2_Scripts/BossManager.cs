using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    GameObject player;

    [Header(("States"))]

    bool startOff = false, pattern1 = false, pattern2 = false, pattern3 = false;
    public bool startSpawn = false;
    bool startLaser = false;
    bool canGun = false, canSpawn = false, canLaser = false;

    //Access Guns
    [Header("Gun Control")]
    [SerializeField] GameObject rightGun, leftGun;
    RailTurrets rightScript, leftScript;
    [SerializeField] float gunTimer = 3f;
    bool gunTransition = false;

    //Access to enemy spawners
    [Header("Spawn Control")]
    [SerializeField] Transform rSpawn1, rSpawn2;
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] int numberToSpawn = 3, spawnCount;
    [SerializeField] float minionSpawnDelay = 1.5f;
    bool isSpawning = false, spawnTransition = false;
    //Access to Laser spawners

    [Header("Laser Control")]
    [SerializeField] Transform lSpawn1, lSpawn2;
    [SerializeField] List<GameObject> laserList;

    //Access to weakpoint containers
    
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rightScript = rightGun.GetComponent<RailTurrets>();
        leftScript = leftGun.GetComponent<RailTurrets>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!startOff)
        {
            //Can add "cinematic" code here
            startOff = true;
            pattern1 = true;
            canGun = true;
        }
        else
        {
            if (pattern1)
            {
                //Start Guns
                if (canGun)
                {
                    ActivateGuns();
                }
                //Disable Guns
                else
                {
                    rightScript.isActive = false;
                    leftScript.isActive = false;
                    canGun = false;
                    if(!gunTransition)
                        StartCoroutine(patternWaitGuns());
                }

                if(canSpawn)
                {
                    SpawnCheck();
                }
                else
                {
                    if (!spawnTransition)
                        StartCoroutine(patternWaitSpawn());
                }
                //Spawn minions

                //open vulnerability

                //Close pattern
            }

            if (pattern2)
            {
                //Start Lasers
                //Disable Lasers

                //startGuns
                //Spawn minions
                //disableGuns

                //open vulnerability

                //Close pattern
            }

            if (pattern3)
            {

            }
        }
    }

    void ActivateGuns()
    {
            rightScript.isActive = true;
            leftScript.isActive = true;
            StartTimer();
    }

    void SpawnCheck()
    {
        if (spawnCount < numberToSpawn && !isSpawning)
        {
            StartCoroutine(spawnMinions());
        }
        else if (spawnCount == numberToSpawn)
        {
            spawnCount = 0;
            canSpawn = false;
        }
    }
    IEnumerator spawnMinions()
    {
        isSpawning = true;
        Instantiate(enemyToSpawn, rSpawn1.transform.position, rSpawn1.transform.rotation);
        Instantiate(enemyToSpawn, rSpawn2.transform.position, rSpawn2.transform.rotation);
        yield return new WaitForSeconds(minionSpawnDelay);
        ++spawnCount;
        isSpawning = false;
    }

    void StartTimer()
    {
        if(gunTimer > 0)
        {
            gunTimer -= Time.deltaTime;
            Debug.Log(gunTimer);
        }
        else
        {
            canGun = false;
        }
    }

    IEnumerator patternWaitGuns()
    {
        yield return new WaitForSeconds(4);
        canSpawn = true;
        gunTransition = true;
    }

    IEnumerator patternWaitSpawn()
    {
        yield return new WaitForSeconds(4);
        //vulExpose = true;
        spawnTransition = true;
    }
}