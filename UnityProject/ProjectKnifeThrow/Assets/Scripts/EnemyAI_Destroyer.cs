using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class enemyAITest : MonoBehaviour
{
    [Header("Destroyer")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] float spawnMoveTime;
    [SerializeField] float turnRate;
    [SerializeField] float focusFireTime;

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

    [Header("Dead")]
    [SerializeField] float deathTimer;
    [SerializeField] GameObject dropOnDeath;
    [SerializeField] ParticleSystem deadEffect;
    [SerializeField] AudioSource deadEffectAudio;

    bool finishedStartup;
    bool isShooting;
    bool allWeakPointsDestroyed;
    bool jointDestroyed;
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
                if (jointDestroyed)
                    StartCoroutine(focusFire());
                faceTarget();
                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }
                if (!allWeakPointsDestroyed)
                    checkWeakPointDestroy();
                if (allWeakPointsDestroyed)
                {
                    if (dropOnDeath != null)
                        Instantiate(dropOnDeath, transform.position, Quaternion.identity);
                    isDead = true;
                }
            }
        }
        else if (isDead && !animStarted)
        {
            StartCoroutine(deathAnimation());
        }
    }

    IEnumerator focusFire()
    {
        jointDestroyed = false;
        shootRate = shootRate / 2;
        turnRate = turnRate * 2;
        yield return new WaitForSeconds(focusFireTime);
        shootRate = shootRate * 2;
        turnRate = turnRate / 2;

    }

    IEnumerator spawnMove()
    {
        agent.Move(transform.forward * Time.deltaTime * agent.speed);
        yield return new WaitForSeconds(spawnMoveTime);
        finishedStartup = true;
    }

    IEnumerator deathAnimation()
    {
        animStarted = true;
        deadEffect.Play();
        deadEffectAudio.Play();
        yield return new WaitForSeconds(deathTimer);
        Destroy(gameObject);
    }

    void faceTarget()
    {
        Vector3 direction = GameManager.instance.player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnRate * Time.deltaTime);
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
                        jointDestroyed = true;
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
                        jointDestroyed = true;
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
                        jointDestroyed = true;
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
                        jointDestroyed = true;
                    }
                }
            }
        }
        if (topGone && leftGone && rightGone && bottomGone)
            allWeakPointsDestroyed = true;
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
    }
}