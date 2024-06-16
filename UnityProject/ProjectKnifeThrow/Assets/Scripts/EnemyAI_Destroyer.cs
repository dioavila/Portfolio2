using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAITest : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform[] shootPos = new Transform [4];
    [SerializeField] int HP;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    bool isShooting;
    public bool playerInRange;
    public Transform playerLocation;

    [SerializeField] List<GameObject> muzzleFlash = new List<GameObject> ();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(GameManager.instance.player.transform.position);

        if (!isShooting)
        {
            StartCoroutine(shoot());
        }

        if (shootPos[0] == null && shootPos[1] == null && shootPos[2] == null && shootPos[3] == null)
        {
            muzzleFlash.Clear ();
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        if (shootPos[0] != null)
        {
            StartCoroutine(flashMuzzle(muzzleFlash[0]));
            Instantiate(bullet, shootPos[0].position, transform.rotation, shootPos[0]);
            yield return new WaitForSeconds(0.1f);
        }
        if (shootPos[1] != null) {
            StartCoroutine(flashMuzzle(muzzleFlash[1]));
            Instantiate(bullet, shootPos[1].position, transform.rotation, shootPos[1]);
            yield return new WaitForSeconds(0.1f);

        }
        if (shootPos[2] != null)
        {
            StartCoroutine(flashMuzzle(muzzleFlash[2]));
            Instantiate(bullet, shootPos[2].position, transform.rotation, shootPos[2]);
            yield return new WaitForSeconds(0.1f);
        }
        if (shootPos[3] != null)
        {
            StartCoroutine(flashMuzzle(muzzleFlash[3]));
            Instantiate(bullet, shootPos[3].position, transform.rotation, shootPos[3]);
            yield return new WaitForSeconds(0.1f);
        }
        isShooting = false;
    }

    IEnumerator flashMuzzle(GameObject muzzleFlash)
    {
        if(muzzleFlash != null)
        {
            muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(.1f);
            if (muzzleFlash != null)
            {
                muzzleFlash.SetActive(false);
            }
        }
    }
}