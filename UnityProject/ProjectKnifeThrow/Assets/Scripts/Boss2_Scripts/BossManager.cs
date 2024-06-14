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
    [SerializeField] float gunTimer = 3f;
    float gunTimerOrig;
    RailTurrets rightScript, leftScript;
    bool gunTransition = false;

    //Access to enemy spawners
    [Header("Spawn Control")]
    [SerializeField] Transform rSpawn1, rSpawn2;
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] int numberToSpawn = 3, spawnCount;
    [SerializeField] float minionSpawnDelay = 1.5f;
    public int enemiesAlive = 0;
    bool isSpawning = false, spawnTransition = false;
    //Access to Laser spawners

    [Header("Laser Control")]
    [SerializeField] Transform lSpawn1, lSpawn2;
    [SerializeField] List<GameObject> laserList;
    [SerializeField] int lasersToSpawn = 3;
    [SerializeField] float laserSpawnDelay = 1.5f;
    bool laserOn = false, laserTransition = false;
    public int activeLasers = 0;
    bool laserFirst = false;

    //Access to weakpoint containers
    [SerializeField] public List<GameObject> weakspotList;
    [SerializeField] List<GameObject> barrierList;
    bool vulExpose = false;
    float vulTimer = 5f, vulTimerOrig;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rightScript = rightGun.GetComponent<RailTurrets>();
        leftScript = leftGun.GetComponent<RailTurrets>();

        gunTimerOrig = gunTimer;
        vulTimerOrig = vulTimer;
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
            //pattern2 = true;
            //canLaser = true;
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
                    if(!gunTransition)
                        StartCoroutine(patternWaitGunsToSpawn());
                }

                //Spawn minions
                if(canSpawn)
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

                //open vulnerability
                if (vulExpose)
                {
                    ShowVulnerable();
                }
                //Close pattern
            }
            //Needs transition timer!!
            if (pattern2)
            {
                //Start Lasers
                if(canLaser)
                {
                    ActivateLasers();
                }
                else
                {
                    if(laserTransition)
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

            if (pattern3)
            {
                //if (vulExpose)
                //{
                //    ShowVulnerable();
                //}
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

        for(int i = 0; i < barrierList.Count; ++i)
        {
            if (barrierList[i] != null)
                barrierList[i].SetActive(false);
        }
        if (pattern1)
        {
            StartTimerVul();
        }
        else if (pattern2 && laserFirst)
        {
            MidTimerVul();
        }
        else
        {
            EndTimerVul();
        }
    }
    void StartTimerVul()
    {
        if (vulTimer > 0 && weakspotList.Count == 3)
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
            if(weakspotList.Count == 3)
            {
                canGun = true;
                gunTimer = gunTimerOrig;
                gunTransition = false;
                spawnTransition = false;
            }
            else
            {
                pattern1 = false;
                canLaser = true;
                pattern2 = true;
                ResetPatterns();
                numberToSpawn *= 2;
            }
            vulTimer = vulTimerOrig;
            spawnCount = 0;
            vulExpose = false;
        }
    }
    void MidTimerVul()
    {
        if (vulTimer > 0 && weakspotList.Count == 2)
        {
            vulTimer -= Time.deltaTime;
            Debug.Log(vulTimer);
        }
        else
        {
            for (int i = 0; i < barrierList.Count; ++i)
            {
                if (barrierList[i] != null)
                    barrierList[i].SetActive(true);
            }
            if (weakspotList.Count == 2)
            {
                canSpawn = false;
                laserFirst = false;
                canLaser = true;
                spawnTransition = false;
                laserTransition = false;
                lasersToSpawn = 4;
            }
            else
            {
                pattern2 = false;
                canSpawn = true;
                pattern3 = true;
                ResetPatterns();
                numberToSpawn *= 2;
            }
            vulTimer = vulTimerOrig;
            spawnCount = 0;
            vulExpose = false;
        }
    }
    void EndTimerVul()
    {
        if (vulTimer > 0 && weakspotList.Count == 2)
        {
            vulTimer -= Time.deltaTime;
            Debug.Log(vulTimer);
        }
        else
        {
            for (int i = 0; i < barrierList.Count; ++i)
            {
                if (barrierList[i] != null)
                    barrierList[i].SetActive(true);
            }
            pattern3 = false;
            ResetPatterns();
        }
    }
    //**********************************************************

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
        int patternIndex = Random.Range(0, 2);
        Instantiate(laserList[patternIndex], lSpawn1.transform.position, lSpawn1.transform.rotation);
        yield return new WaitForSeconds(laserSpawnDelay);
        patternIndex = Random.Range(0, 2);
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
        if(gunTimer > 0)
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

    void ResetPatterns()
    {
        gunTransition = false; 
        spawnTransition = false;
        vulExpose = false;
        canGun = true;
        canSpawn = false;
        gunTimer = 0;
        spawnCount = 0;
        vulTimer = 0;
    }
}