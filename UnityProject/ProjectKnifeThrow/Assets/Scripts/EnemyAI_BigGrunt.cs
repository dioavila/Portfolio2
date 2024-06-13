using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_BigGrunt : MonoBehaviour, IFreeze
{
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos1;
    [SerializeField] Transform shootPos2;
    [SerializeField] Transform shootPos3;
    [SerializeField] Transform shootPos4;
    [SerializeField] Transform headPos;

    [SerializeField] List<GameObject> critPoints = new List<GameObject>();
    [SerializeField] GameObject eyeball;
    [SerializeField] ParticleSystem eyeBlood;
    [SerializeField] ParticleSystem robotExplosion;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject dropOnDeath;

    [SerializeField] int viewAngle;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int roamDistance;
    [SerializeField] int roamTimer;
    [SerializeField] int deathTimer;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    [SerializeField] public Transform spawnPath;
    private Transform startingSpawn;

    public NavMeshAgent agent;

    bool isShooting;
    bool playerInRange;
    bool destChosen;
    bool finishedStartup = false;
    bool canshoot = true;

    Vector3 playerDir;
    Vector3 startingPos;

    float angleToPlayer;
    float stoppingDistOrig;

    bool isDead = false;
    bool lookPlayer = false;
    public int turnRate;

    // Start is called before the first frame update
    void Start()
    {
        startingSpawn = spawnPath;
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
            }
            else
            {
                agent.isStopped = false;
                canshoot = true;
            }
            if (finishedStartup)
            {
                startingPos = transform.position;
                if (lookPlayer)
                {
                    faceTarget();
                }
                if (playerInRange && !canSeePlayer())
                {
                    if (!lookPlayer)
                        lookPlayer = false;
                    Debug.Log("Roam Forgot");
                    StartCoroutine(roam());
                }
                else if (!playerInRange)
                {
                    Debug.Log("Roam General");
                    StartCoroutine(roam());
                }
                if (critPoints.Count != 0)
                {
                    for (int i = 0; i < critPoints.Count; i++)
                    {
                        if (critPoints[i] == null)
                            critPoints.Remove(critPoints[i]);
                    }
                }
                if (critPoints.Count == 0)
                {
                    Destroy(eyeball);
                    eyeBlood.Play();
                    agent.SetDestination(agent.transform.position);
                    gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    gameObject.GetComponent<Rigidbody>().useGravity = true;
                    if (dropOnDeath != null)
                        Instantiate(dropOnDeath, transform.position, Quaternion.identity);
                    isDead = true;
                    robotExplosion.Play();
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
        else if (isDead)
        {
            StartCoroutine(deathAnimation());
        }
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
            muzzleFlash.Play();
            createBullet(shootPos1);
        }
        if (shootPos2 != null)
        {
            muzzleFlash.Play();
            createBullet(shootPos2);
        }
        if (shootPos3 != null)
        {
            muzzleFlash.Play();
            createBullet(shootPos3);
        }
        if (shootPos4 != null)
        {
            muzzleFlash.Play();
            createBullet(shootPos4);
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
        yield return new WaitForSeconds(time);
        model.material.color = Color.white;
    }
}