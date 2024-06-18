using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SceneManagement;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    GameObject player;

    [Header(("States"))]
    [SerializeField] float patternTimer = 5f;
    float patternTimerOrig;
    public bool startOff = false;
    bool pattern1 = false, pattern2 = false, pattern3 = false, patternTrans = false;
    bool canGun = false, canSpawn = false, canLaser = false;
    //bool startLaser = false;
    public bool startSpawn = false;
    bool Death = false;
    [SerializeField] List<GameObject> listEyes;
    [SerializeField] ParticleSystem deathEye;


    //Access Guns
    [Header("Gun Control")]
    [SerializeField] GameObject rightGun, leftGun;
    [SerializeField] float gunTimer = 3f;
    RailTurrets rightScript, leftScript;
    float gunTimerOrig;
    bool gunTransition = false;

    //Access to enemy spawners
    [Header("Spawn Control")]
    [SerializeField] Transform rSpawn1, rSpawn2;
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] int numberToSpawn = 3, spawnCount;
    [SerializeField] float minionSpawnDelay = 1.5f;
    bool isSpawning = false, spawnTransition = false;
    public int enemiesAlive = 0;
    //Access to Laser spawners

    [Header("Laser Control")]
    [SerializeField] Transform lSpawn1, lSpawn2;
    [SerializeField] List<GameObject> laserList;
    [SerializeField] int lasersToSpawn = 3;
    [SerializeField] float laserSpawnDelay = 1.5f;
    int laserToSpawnOrig;
    bool laserOn = false, laserTransition = false;
    bool laserFirst = false;
    public int activeLasers = 0;
    

    //Access to weakpoint containers
    [SerializeField] public List<GameObject> weakspotList;
    [SerializeField] List<GameObject> barrierList;
    float vulTimer = 5f, vulTimerOrig;
    bool vulExpose = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rightScript = rightGun.GetComponent<RailTurrets>();
        leftScript = leftGun.GetComponent<RailTurrets>();

        laserToSpawnOrig = lasersToSpawn;
        gunTimerOrig = gunTimer;
        vulTimerOrig = vulTimer;
        patternTimerOrig = patternTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startOff)
        {
            //Can add "cinematic" code here
            //startOff = true;
            pattern1 = true;
            canGun = true;
            //Death = true;
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
                else
                {
                    rightScript.isActive = false;
                    leftScript.isActive = false;
                    canGun = false;
                    if (!gunTransition)
                        StartCoroutine(patternWaitGunsToSpawn());
                }

                //Spawn minions
                if (canSpawn)
                {
                    SpawnCheck();
                }
                else
                {
                    // canSpawn = false;
                    if (spawnTransition)
                    {
                        if (enemiesAlive == 0)
                            StartCoroutine(patternWaitSpawnToVul());
                    }
                }

                //Open vulnerability
                if (vulExpose)
                {
                    ShowVulnerable();
                }
                //Close pattern
            }
            else if (pattern2)
            {
                //Start Lasers
                if (canLaser)
                {
                    ActivateLasers();
                }
                else
                {
                    if (laserTransition)
                    {
                        if (activeLasers == 0)
                            if (!laserFirst)
                                StartCoroutine(patternWaitLaserToSpawn());
                            else
                                StartCoroutine(patternWaitLaserToVul());
                    }
                }
                //Disable Lasers

                if (canSpawn)
                {
                    rightScript.isActive = true;
                    leftScript.isActive = true;
                    SpawnCheck();
                }
                else
                {
                    if (spawnTransition)
                    {
                        if (enemiesAlive <= 0)
                        {
                            StartCoroutine(patternWaitSpawnNGunsToLaser());
                            rightScript.isActive = false;
                            leftScript.isActive = false;
                        }
                    }
                }

                if (vulExpose)
                {
                    ShowVulnerable();
                }
            }
            else if (pattern3)
            {
                if (canSpawn)
                {
                    rightScript.isActive = true;
                    leftScript.isActive = true;
                    SpawnCheck();
                }
                else
                {
                    if (spawnTransition)
                    {
                        if (enemiesAlive <= 0)
                        {
                            StartCoroutine(patternWaitSpawnNGunsToLaser());
                        }
                    }
                }

                if (canLaser)
                {
                    ActivateLasers();
                }
                else
                {
                    if (laserTransition)
                    {
                        if (activeLasers == 0)
                            StartCoroutine(patternWaitLaserToVul());
                    }
                }
                if (vulExpose)
                {
                    rightScript.isActive = false;
                    leftScript.isActive = false;
                    ShowVulnerable();
                }
            }
            if (Death)
            {
                StartCoroutine(DestroyEyes());
            }
        }
    }

    //Add timer between patterns
    //Add Final Pattern
    //Add Death Logic

    /// <summary>
    /// WEAKSPOT METHODS
    /// </summary>
    void ShowVulnerable()
    {

        for (int i = 0; i < barrierList.Count; ++i)
        {
            if (barrierList[i] != null)
                barrierList[i].SetActive(false);
        }
        if (pattern1)
        {
            StartTimerVul(3);
        }
        else if (pattern2 && laserFirst)
        {
            StartTimerVul(2);
        }
        else
        {
            StartTimerVul(1);
        }
    }
    void StartTimerVul(int weakPointCount)
    {
        if (vulTimer > 0 && weakspotList.Count == weakPointCount)
        {
            vulTimer -= Time.deltaTime;
        }
        else
        {
            for (int i = 0; i < barrierList.Count; ++i)
            {
                if (barrierList[i] != null)
                    barrierList[i].SetActive(true);
            }
            PatternTransitionTimer();
            if (patternTrans)
            {
                vulTimer = vulTimerOrig;
                vulExpose = false;
                if (weakspotList.Count == weakPointCount)
                {
                    PatternLoopReset();
                    switch (weakPointCount)
                    {
                        case 3:
                            canGun = true; 
                            break;
                        case 2:
                            canLaser = true;
                            break;
                        case 1:
                            canSpawn = true;
                            break;
                    }
                }
                else
                {
                    PatternLoopReset();
                    switch (weakPointCount)
                    {
                        case 3:
                            pattern1 = false;
                            canLaser = true;
                            pattern2 = true;
                            numberToSpawn *= 2;
                            break;
                        case 2:
                            pattern2 = false;
                            canSpawn = true;
                            pattern3 = true;
                            numberToSpawn *= 2;
                            break;
                        case 1:
                            pattern3 = false;
                            Death = true;
                            break;
                    }
                }
                spawnCount = 0;
                patternTrans = false;
            }
        }
    }

    /// <summary>
    /// LASER METHODS
    /// </summary>
    void ActivateLasers()
    {
        if (lasersToSpawn > 0 && !laserOn)
        {
            StartCoroutine(spawnLasers());
        }
        else if (lasersToSpawn == 0)
        {
            laserTransition = true;
            canLaser = false;
        }
    }

    IEnumerator spawnLasers()
    {
        laserOn = true;
        int patternIndex = UnityEngine.Random.Range(0, 2);
        Instantiate(laserList[patternIndex], lSpawn1.transform.position, lSpawn1.transform.rotation);
        --lasersToSpawn;
        yield return new WaitForSeconds(laserSpawnDelay);
        patternIndex = UnityEngine.Random.Range(0, 2);
        Instantiate(laserList[patternIndex], lSpawn2.transform.position, lSpawn2.transform.rotation);
        --lasersToSpawn;
        yield return new WaitForSeconds(laserSpawnDelay);
        laserOn = false;
    }
    IEnumerator patternWaitLaserToSpawn()
    {
        yield return new WaitForSeconds(4);
        canSpawn = true;
        laserTransition = false;
        laserFirst = true;
    }
    IEnumerator patternWaitLaserToVul()
    {
        yield return new WaitForSeconds(4);
        laserTransition = false;
        vulExpose = true;
    }
    //**********************************************************

    /// <summary>
    /// GUN METHODS
    /// </summary>
    void ActivateGuns()
    {
        rightScript.isActive = true;
        leftScript.isActive = true;
        StartTimer();
    }
    void StartTimer()
    {
        if (gunTimer > 0)
        {
            gunTimer -= Time.deltaTime;
        }
        else
        {
            canGun = false;
        }
    }
    IEnumerator patternWaitGunsToSpawn()
    {
        yield return new WaitForSeconds(4);
        canSpawn = true;
        gunTransition = true;
    }
    //**********************************************************

    /// <summary>
    /// SPAWN METHODS
    /// </summary>
    void SpawnCheck()
    {
        if (spawnCount < numberToSpawn && !isSpawning)
        {
            StartCoroutine(spawnMinions());
        }
        else if (spawnCount == numberToSpawn)
        {
            spawnTransition = true;
            canSpawn = false;
        }
    }
    IEnumerator spawnMinions()
    {
        isSpawning = true;
        spawnCount++;
        Instantiate(enemyToSpawn, rSpawn1.transform.position, rSpawn1.transform.rotation);
        Instantiate(enemyToSpawn, rSpawn2.transform.position, rSpawn2.transform.rotation);
        yield return new WaitForSeconds(minionSpawnDelay);
        isSpawning = false;
    }
    IEnumerator patternWaitSpawnToVul()
    {
        yield return new WaitForSeconds(4);
        vulExpose = true;
        spawnTransition = false;
    }

    IEnumerator patternWaitSpawnNGunsToLaser()
    {
        yield return new WaitForSeconds(4);
        lasersToSpawn = 2;
        canLaser = true;
        spawnTransition = false;
    }

    void PatternTransitionTimer()
    {
        if (patternTimer > 0)
        {
            patternTimer -= Time.deltaTime;
        }
        else
        {
            patternTrans = true;
        }
    }

    void PatternLoopReset()
    {
        canGun = true;
        gunTimer = gunTimerOrig;
        gunTransition = false;
        canSpawn = false;
        canLaser = false;
        spawnTransition = false;
        laserTransition = false;
        laserFirst = false;
        lasersToSpawn = laserToSpawnOrig*2;
        patternTimer = patternTimerOrig;
    }

    IEnumerator DestroyEyes()
    {
        while(listEyes.Count > 1) 
        {
            int index = UnityEngine.Random.Range(1, listEyes.Count - 1);
            GameObject eyeDestroy = listEyes[index];
            listEyes.Remove(listEyes[index]);
            Instantiate(deathEye, eyeDestroy.transform.position, Quaternion.identity);
            Destroy(eyeDestroy);
            yield return new WaitForSeconds(4);
        }
        yield return new WaitForSeconds(4);
        GameObject lastEye = listEyes[0];
        listEyes.Clear();
        Instantiate(deathEye, lastEye.transform.position, Quaternion.identity);
        Destroy(lastEye);
        GameManager.instance.updateGameGoal(-1);
    }
}