using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour
{
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos1;
    [SerializeField] Transform shootPos2;
    [SerializeField] Transform shootPos3;
    [SerializeField] Transform shootPos4;
    [SerializeField] Transform headPos;

    [SerializeField] GameObject muzzleFlash1;
    [SerializeField] GameObject muzzleFlash2;
    [SerializeField] GameObject muzzleFlash3;
    [SerializeField] GameObject muzzleFlash4;
    [SerializeField] GameObject critPoint;

    [SerializeField] int HP;
    [SerializeField] int viewAngle;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int roamDistance;
    [SerializeField] int roamTimer;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    [SerializeField] public Transform spawnPath;
    private Transform startingSpawn;

    public NavMeshAgent agent;
    private Transform spawnArea;

    bool isShooting;
    bool playerInRange;
    bool destChosen;
    bool finishedStartup = false;
    //bool isReadyToOrbit = false;

    Vector3 playerDir;
    Vector3 startingPos;

    float angleToPlayer;
    float stoppingDistOrig;

    bool lookPlayer = false;
    public int turnRate;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.updateGameGoal(1);
        startingPos = transform.position;
        startingSpawn = spawnPath;
        stoppingDistOrig = agent.stoppingDistance;

    }

    // Update is called once per frame
    void Update()
    {

        if (finishedStartup)
        {
            if (lookPlayer)
            {
                faceTarget();
            }

            if (playerInRange && !canSeePlayer())
            {
                if (lookPlayer)
                    lookPlayer = false;
                Debug.Log("Roam Forgot");
                StartCoroutine(roam());
            }
            else if (!playerInRange)
            {
                if (lookPlayer)
                    lookPlayer = false;
                Debug.Log("Roam General");
                StartCoroutine(roam());
            }

            if (critPoint == null)
            {
                Destroy(agent.gameObject);
            }
        }
        else
        {
            agent.stoppingDistance = 1;
            agent.destination = startingSpawn.position;
            if (agent.remainingDistance <= stoppingDistOrig)
                finishedStartup = true;
        }
    }

    IEnumerator roam()
    {
        if (!destChosen && agent.remainingDistance < 1f)
        {
            destChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamTimer);
            Vector3 ranPos = Random.insideUnitSphere * roamDistance;
            ranPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(ranPos, out hit, roamDistance, 1);
            agent.SetDestination(hit.position);

            Quaternion resetRot = Quaternion.LookRotation(agent.transform.forward, Vector3.up);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, resetRot, 1);

            destChosen = false;
        }
    }

    bool canSeePlayer()
    {
        playerDir = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, playerDir.y, playerDir.z), transform.forward);
        //Debug.Log(angleToPlayer);
        //Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer < viewAngle)
            { 
                agent.stoppingDistance = stoppingDistOrig;

                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }


                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    lookPlayer = true;
                }

                return true;
            }
        }

        agent.stoppingDistance = 0;
        return false;
    }

    void faceTarget()
    {
        Vector3 direction = GameManager.instance.player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnRate * Time.deltaTime);
    }

    //void orbiting()
    //{
    //    Vector3 orbit = Vector3.Cross(transform.forward, Vector3.up);
    //    transform.position = Vector3.Lerp(transform.position, orbit, Time.deltaTime * 5);
    //}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lookPlayer = false;
            playerInRange = false;
            agent.stoppingDistance = 0;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        if (shootPos1 != null)
        {
            StartCoroutine(flashMuzzle(muzzleFlash1));
            createBullet(shootPos1);
        }
        if (shootPos2 != null)
        {
            StartCoroutine(flashMuzzle(muzzleFlash2));
            createBullet(shootPos2);
        }
        if (shootPos3 != null)
        {
            StartCoroutine(flashMuzzle(muzzleFlash3));
            createBullet(shootPos3);
        }
        if (shootPos4 != null)
        {
            StartCoroutine(flashMuzzle(muzzleFlash4));
            createBullet(shootPos4);
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void createBullet(Transform shootPos)
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }

    IEnumerator flashMuzzle(GameObject muzzleFlash)
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.1f);
        muzzleFlash.SetActive(false);
    }
}