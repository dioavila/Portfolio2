using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Animator anim;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos1;
    [SerializeField] Transform shootPos2;
    [SerializeField] Transform shootPos3;
    [SerializeField] Transform shootPos4;
    [SerializeField] Transform headPos;

    [SerializeField] int HP;
    [SerializeField] int viewAngle;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int roamDistance;
    [SerializeField] int roamTimer;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    public NavMeshAgent agent;

    bool isShooting;
    bool playerInRange;
    bool destChosen;
    bool isReadyToOrbit = false;

    Vector3 playerDir;
    Vector3 startingPos;

    float angleToPlayer;
    float stoppingDistOrig;

    Renderer materialOrig;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.updateGameGoal(1);
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        materialOrig = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //float animSpeed = agent.velocity.normalized.magnitude;
        //anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans));

        if (playerInRange && !canSeePlayer())
        {
            StartCoroutine(roam());
        }
        else if (!playerInRange)
        {
            //if (isReadyToOrbit)
            //    isReadyToOrbit= false;
            StartCoroutine(roam());
        }

        //if (isReadyToOrbit)
        //{
        //    orbiting();
        //}
    }

    IEnumerator roam()
    {
        if (!destChosen && agent.remainingDistance < 0.1f)
        {
            destChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamTimer);
            Vector3 ranPos = Random.insideUnitSphere * roamDistance;
            ranPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(ranPos, out hit, roamDistance, 1);
            agent.SetDestination(hit.position);

            destChosen = false;
        }
    }

    bool canSeePlayer()
    {
        playerDir = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, playerDir.y, playerDir.z), transform.forward);
        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDir);

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
                    faceTarget();
                    //isReadyToOrbit = true;
                }

                return true;
            }
        }

        agent.stoppingDistance = 0;
        return false;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, playerDir.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
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
            playerInRange = false;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        //anim.SetTrigger("Shoot");
        if (shootPos1 != null)
        {
            createBullet(shootPos1);
        }
        if (shootPos2 != null)
        {
            createBullet(shootPos2);
        }
        if (shootPos3 != null)
        {
            createBullet(shootPos3);
        }
        if (shootPos4 != null)
        {
            createBullet(shootPos4);
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void createBullet(Transform shootPos)
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        agent.SetDestination(GameManager.instance.player.transform.position);
        StartCoroutine(flashred());

        if (HP <= 0)
        {
            GameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
            GameManager.instance.doorIsDestroyable = true;
        }
    }

    IEnumerator flashred()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    IEnumerator flashmagenta()
    {
        Color temp = materialOrig.material.color;
        materialOrig.material.color = Color.magenta;
        yield return new WaitForSeconds(0.5f);
        materialOrig.material.color = temp;
    }
}