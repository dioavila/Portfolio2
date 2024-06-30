using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_BigGrunt : MonoBehaviour, IFreeze
{
    [Header("Boss Toggle")]
    [SerializeField] bool belongsToGORE = false;

    [Header("Level 3 Toggle")]
    [SerializeField] bool belongsToL3 = false;

    [Header("Big Grunt")]
    public NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject eyeball;
    [SerializeField] Transform headPos;
    [SerializeField] float spawnMoveTime;
    [SerializeField] float turnRate;
    [SerializeField] int viewAngle;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int roamDistance;
    [SerializeField] int roamTimer;
    [SerializeField] GameObject IceCap;

    [Header("Weak Points")]
    [SerializeField] List<GameObject> critPoints = new List<GameObject>();

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
    bool canShoot;
    bool canTurn;
    float angleToPlayer;
    bool destChosen;
    float stoppingDistOrig;
    bool isDead;
    bool lookPlayer;

    // Start is called before the first frame update
    void Start()
    {
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
                canShoot = false;
                canTurn = false;
            }
            else
            {
                agent.isStopped = false;
                canShoot = true;
                canTurn = true;
            }

            if (!finishedStartup)
            {
                StartCoroutine(spawnMove());
                if (critPoints.Count == 0)
                    deathStart();
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
                if (critPoints[0] == null && critPoints[1] == null && critPoints[2] == null)
                {
                    critPoints.Remove(critPoints[2]);
                    critPoints.Remove(critPoints[1]);
                    critPoints.Remove(critPoints[0]);
                }
                if (critPoints.Count == 0)
                {
                    Destroy(eyeball);
                    agent.SetDestination(agent.transform.position);
                    gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    gameObject.GetComponent<Rigidbody>().useGravity = true;
                    if (dropOnDeath != null)
                        Instantiate(dropOnDeath, transform.position, Quaternion.identity);
                    if (!belongsToL3)
                    {
                        if (!belongsToGORE)
                        {
                            GameManager.instance.sceneSpawners[GameManager.instance.sceneBattleRoomIndex].GetComponent<SpawnTrig>().enemiesAlive--;
                        }
                        else
                        {
                            GameManager.instance.bossManager.enemiesAlive -= 1;
                        }
                    }
                        isDead = true;
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

    private void deathStart()
    {
        Destroy(eyeball);
        agent.SetDestination(agent.transform.position);
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        if (dropOnDeath != null)
            Instantiate(dropOnDeath, transform.position, Quaternion.identity);
        if (!belongsToGORE)
        {
            GameManager.instance.sceneSpawners[GameManager.instance.sceneBattleRoomIndex].GetComponent<SpawnTrig>().enemiesAlive--;
        }
        else
        {
            GameManager.instance.bossManager.enemiesAlive -= 1;
        }
        isDead = true;
        robotExplosion.Play();
        deathSound.Play();
    }

    bool canSeePlayer()
    {
        if (canTurn)
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
                if (!isShooting && canShoot && angleToPlayer <= viewAngle)
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
            agent.SetDestination(agent.transform.position);
            destChosen = true;
            playerInRange = true;
        }
    }

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerInRange = false;
    //        agent.stoppingDistance = 0;
    //    }
    //}

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