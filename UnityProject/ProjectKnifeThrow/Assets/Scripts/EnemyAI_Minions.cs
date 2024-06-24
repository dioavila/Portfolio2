using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IFreeze
{
    [Header("Boss Toggle")]
    [SerializeField] bool belongsToGORE = false;

    [Header("Grunt")]
    public NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] float spawnMoveTime;
    [SerializeField] float turnRate;
    [SerializeField] int viewAngle;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int roamDistance;
    [SerializeField] int roamTimer;
    [SerializeField] GameObject IceCap;

    [Header("Weak Point")]
    [SerializeField] GameObject critPoint;

    [Header("Gun")]
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] AudioSource gunSound;


    [Header("Dead")]
    [SerializeField] float deathTimer;
    [SerializeField] GameObject dropOnDeath;
    [SerializeField] ParticleSystem robotExplosion;
    [SerializeField] AudioSource deathSound;

    Vector3 playerDir;
    Vector3 startingPos;
    bool finishedStartup;
    bool isShooting;
    bool playerInRange;
    bool canshoot;
    bool canTurn;
    float angleToPlayer;
    bool destChosen;
    float stoppingDistOrig;
    bool lookPlayer;
    bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        if (belongsToGORE)
        {
            GameManager.instance.bossManager.enemiesAlive += 1;
        }
        stoppingDistOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (model.material.color == Color.blue)
            {
                agent.isStopped = true;
                canshoot = false;
                canTurn = false;
            }
            else
            {
                agent.isStopped = false;
                canshoot = true;
                canTurn = true;
            }

            if (!finishedStartup)
            {
                StartCoroutine(spawnMove());
            }
            else if (finishedStartup)
            {
                if (playerInRange && !canSeePlayer() && canTurn)
                {
                    faceTarget();
                }
                else if (!playerInRange)
                {
                    StartCoroutine(roam());
                }
                if (critPoint == null)
                {
                    agent.SetDestination(agent.transform.position);
                    gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    gameObject.GetComponent<Rigidbody>().useGravity = true;
                    if (dropOnDeath != null)
                        Instantiate(dropOnDeath, transform.position, Quaternion.identity);
                    isDead = true;
                    if (!belongsToGORE)
                    {
                        GameManager.instance.sceneSpawners[GameManager.instance.sceneBattleRoomIndex].GetComponent<SpawnTrig>().enemiesAlive--;
                    }
                    else
                    {
                        GameManager.instance.bossManager.enemiesAlive -= 1;
                    }
                    robotExplosion.Play();
                    deathSound.Play();
                }
            }
        }
        else if (isDead)
        {
            StartCoroutine(deathAnimation());
        }
    }

    IEnumerator spawnMove()
    {
        agent.Move(transform.forward * Time.deltaTime * agent.speed);
        yield return new WaitForSeconds(spawnMoveTime);
        finishedStartup = true;
    }

    IEnumerator deathAnimation()
    {
        yield return new WaitForSeconds(deathTimer);
        Destroy(agent.gameObject);
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
        if(canTurn)
        {
            faceTarget();
        }
        agent.SetDestination(GameManager.instance.player.transform.position);
        playerDir = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, playerDir.y, playerDir.z), transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer < viewAngle)
            { 
                agent.stoppingDistance = stoppingDistOrig;
                if (!isShooting && canshoot && angleToPlayer <= viewAngle)
                {
                    faceTarget();
                    StartCoroutine(shoot());
                }
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }
                return true;
            }
        }
        return false;
    }

    void faceTarget()
    {
        Vector3 direction = GameManager.instance.player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnRate * Time.deltaTime);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            destChosen = true;
            playerInRange = true;
        }
    }


    IEnumerator shoot()
    {
        isShooting = true;
        if (shootPos != null)
        {
            muzzleFlash.Play();
            gunSound.Play();
            createBullet(shootPos);
        }
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void createBullet(Transform shootPos)
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }

    public void FreezeTime(int time)
    {
        StartCoroutine(FlashBlue(time));
    }

    IEnumerator FlashBlue(int time)
    {
        model.material.color = Color.blue;
        IceCap.SetActive(true);
        yield return new WaitForSeconds(time);
        IceCap.SetActive(false);
        model.material.color = Color.white;
    }
}