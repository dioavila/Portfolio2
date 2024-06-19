using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class enemyAITest : MonoBehaviour
{
    [Header("Destroy")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] float spawnMoveTime;

    [Header("Weak Points")]
    [SerializeField] List<List<GameObject>> allWeakPoints = new List<List<GameObject>>();
    [SerializeField] List<GameObject> weakPointsTop = new List<GameObject>();
    [SerializeField] List<GameObject> weakPointsLeft = new List<GameObject>();
    [SerializeField] List<GameObject> weakPointsRight = new List<GameObject>();
    [SerializeField] List<GameObject> weakPointsBottom = new List<GameObject>();

    [Header("Guns")]
    [SerializeField] List<GameObject> turretJoint = new List<GameObject>();
    [SerializeField] Transform[] shootPos = new Transform[4];
    [SerializeField] List<ParticleSystem> muzzleFlash = new List<ParticleSystem>();
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] AudioSource muzzleSound;

    [Header("Knock Back")]
    [SerializeField] float knockbackForce;
    [SerializeField] float activationRadius;
    [SerializeField] AudioSource knockbackAudio;

    [Header("Dead")]
    [SerializeField] float deathTimer;
    [SerializeField] GameObject dropOnDeath;
    [SerializeField] ParticleSystem deadEffect;
    [SerializeField] AudioSource deadEffectAudio;

    float turnspeed;

    Vector3 startMoveLoc;
    bool finishedStartup;
    bool isShooting;
    bool knockbackReady;
    bool playerInRange;
    bool allWeakPointsDestroyed;
    bool topGone;
    bool leftGone;
    bool rightGone;
    bool bottomGone;
    bool animStarted;
    bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        allWeakPoints.Add(weakPointsTop);
        allWeakPoints.Add(weakPointsLeft);
        allWeakPoints.Add(weakPointsRight);
        allWeakPoints.Add(weakPointsBottom);
        //turnspeed = GameManager.instance.jointCS.limbTurnRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (!finishedStartup)
            {
                StartCoroutine(spawnMove());
            }
            else if (finishedStartup)
            {
                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }
                
                if(!allWeakPointsDestroyed)
                    checkWeakPointDestroy();

                if (allWeakPointsDestroyed)
                {
                    gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    gameObject.GetComponent<Rigidbody>().useGravity = true;
                    if (dropOnDeath != null)
                        Instantiate(dropOnDeath, transform.position, Quaternion.identity);
                    isDead = true;
                }
            }
        }
        else if (isDead && animStarted)
        {
            StartCoroutine(deathAnimation());
        }
    }

    IEnumerator spawnMove()
    {
        agent.SetDestination(transform.forward);
        yield return new WaitForSeconds(spawnMoveTime);
        finishedStartup = true;
    }

    private void checkWeakPointDestroy()
    {
        for (int theWeakPoint = 0; theWeakPoint < allWeakPoints.Count; theWeakPoint++)
        {
            if (theWeakPoint == 0 && !topGone)
            {
                for (int j = 0; j < weakPointsTop.Count; j++)
                {
                    if (weakPointsTop[j] == null)
                    {
                        weakPointsTop.RemoveAt(j);
                    }
                    if (weakPointsTop.Count == 0)
                    {
                        Destroy(turretJoint[0]);
                        topGone = true;
                    }
                }
            }

            if (theWeakPoint == 1 && !leftGone)
            {
                for (int j = 0; j < weakPointsLeft.Count; j++)
                {
                    if (weakPointsLeft[j] == null)
                    {
                        weakPointsLeft.RemoveAt(j);
                    }
                    if (weakPointsLeft.Count == 0)
                    {
                        Destroy(turretJoint[1]);
                        leftGone = true;
                    }
                }
            }

            if (theWeakPoint == 2 && !rightGone)
            {
                for (int j = 0; j < weakPointsRight.Count; j++)
                {
                    if (weakPointsRight[j] == null)
                    {
                        weakPointsRight.RemoveAt(j);
                    }
                    if (weakPointsRight.Count == 0)
                    {
                        Destroy(turretJoint[2]);
                        rightGone = true;
                    }
                }
            }

            if (theWeakPoint == 3 && !bottomGone)
            {
                for (int j = 0; j < weakPointsBottom.Count; j++)
                {
                    if (weakPointsBottom[j] == null)
                    {
                        weakPointsBottom.RemoveAt(j);
                    }
                    if (weakPointsBottom.Count == 0)
                    {
                        Destroy(turretJoint[3]);
                        bottomGone = true;
                    }
                }
            }
        }
        if (topGone && leftGone && rightGone && bottomGone)
            allWeakPointsDestroyed = true;
    }

    IEnumerator deathAnimation()
    {
        deadEffect.Play();
        deadEffectAudio.Play();
        yield return new WaitForSeconds(deathTimer);
        Destroy(gameObject);
        animStarted = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }
    IEnumerator shoot()
    {
        isShooting = true;
        for (int i = 0; i < shootPos.Length; i++)
        {
            if (shootPos[i] != null)
            {
                muzzleFlash[i].Play();
                muzzleSound.Play();
                Instantiate(bullet, shootPos[i].position, transform.rotation, shootPos[i]);
                yield return new WaitForSeconds(shootRate);
            }
        }
        isShooting = false;

        //if (shootPos[0] == null)
        //{
        //    shootRate -= 0.2f;
        //    turnspeed = turnspeed / 2;
        //    FocusFire.Play();
        //    //FocusFire[1].Play();
        //    //FocusFire[2].Play();
        //    //FocusFire[3].Play();
        //}
        //if (shootPos[1] == null)
        //{
        //    shootRate -= 0.2f;
        //    turnspeed = turnspeed / 2;
        //    FocusFire.Play();
        //    //FocusFire[0].Play();
        //    //FocusFire[2].Play();
        //    //FocusFire[3].Play();
        //}
        //if (shootPos[2] == null)
        //{
        //    shootRate -= 0.2f;
        //    turnspeed = turnspeed / 2;
        //    FocusFire.Play();
        //    //FocusFire[1].Play();
        //    //FocusFire[0].Play();
        //    //FocusFire[3].Play();
        //}
        //if (shootPos[3] == null)
        //{
        //    shootRate -= 0.2f;
        //    turnspeed = turnspeed / 2;
        //    FocusFire.Play();
        //    //FocusFire[1].Play();
        //    //FocusFire[2].Play();
        //    //FocusFire[0].Play();
        //}
        //if (shootPos[0] == null && shootPos[1] == null)
        //{
        //    FocusFire[2].Play();
        //    FocusFire[3].Play();
        //}
        //if (shootPos[0] == null && shootPos[2] == null)
        //{
        //    FocusFire[1].Play();
        //    FocusFire[3].Play();
        //}
        //if (shootPos[0] == null && shootPos[3] == null)
        //{
        //    FocusFire[2].Play();
        //    FocusFire[1].Play();
        //}
        //if (shootPos[1] == null && shootPos[2] == null)
        //{
        //    FocusFire[0].Play();
        //    FocusFire[3].Play();
        //}
        //if (shootPos[1] == null && shootPos[3] == null)
        //{
        //    FocusFire[0].Play();
        //    FocusFire[2].Play();
        //}
        //if (shootPos[2] == null && shootPos[3] == null)
        //{
        //    FocusFire[0].Play();
        //    FocusFire[1].Play();
        //}
        //isShooting = false;
    }

    IEnumerator pushflash()
    {
        GameManager.instance.playerPushBack.SetActive(true);
        yield return new WaitForSeconds(.1f);
        GameManager.instance.playerPushBack.SetActive(false);
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    IPushback player = collision.collider.GetComponent<IPushback>();
    //    if (player != null)
    //    {
    //        player.Pushback((transform.position - collision.transform.position) * PushbackAmount);
    //    }
    //}
}