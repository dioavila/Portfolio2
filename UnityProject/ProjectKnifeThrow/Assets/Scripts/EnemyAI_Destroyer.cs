using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class enemyAITest : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform[] shootPos = new Transform[4];
    [SerializeField] int HP;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] int PushbackAmount;
    [SerializeField] ParticleSystem FocusFire;
    [SerializeField] AudioSource pushAudio;
    [SerializeField] AudioClip pushAudioClip;
    float range;
    float turnspeed;
    bool isShooting;
    public bool playerInRange;
    public Transform playerLocation;
    CapsuleCollider colid;
    SphereCollider sphereCollider;
    Vector3 dir;
    [SerializeField] List<GameObject> muzzleFlash = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        colid = GetComponent<CapsuleCollider>();
        sphereCollider = GetComponent<SphereCollider>();
        range = colid.radius;
        turnspeed = GameManager.instance.jointCS.limbTurnRate;
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
            muzzleFlash.Clear();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           //colid.radius = sphereCollider.radius;
           // pushAudio.PlayOneShot(pushAudioClip);
           // StartCoroutine(pushflash());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //colid.radius = range;
        }
    }
    IEnumerator shoot()
    {
        isShooting = true;
        if (shootPos[0] != null)
        {
            StartCoroutine(flashMuzzle(muzzleFlash[0]));
            Instantiate(bullet, shootPos[0].position, transform.rotation, shootPos[0]);
            yield return new WaitForSeconds(shootRate);
        }
        if (shootPos[1] != null)
        {
            StartCoroutine(flashMuzzle(muzzleFlash[1]));
            Instantiate(bullet, shootPos[1].position, transform.rotation, shootPos[1]);
            yield return new WaitForSeconds(shootRate);

        }
        if (shootPos[2] != null)
        {
            StartCoroutine(flashMuzzle(muzzleFlash[2]));
            Instantiate(bullet, shootPos[2].position, transform.rotation, shootPos[2]);
            yield return new WaitForSeconds(shootRate);
        }
        if (shootPos[3] != null)
        {
            StartCoroutine(flashMuzzle(muzzleFlash[3]));
            Instantiate(bullet, shootPos[3].position, transform.rotation, shootPos[3]);
            yield return new WaitForSeconds(shootRate);
        }
        if (shootPos[0] == null)
        {
            shootRate -= 0.2f;
            turnspeed = turnspeed / 2;
            FocusFire.Play();
            //FocusFire[1].Play();
            //FocusFire[2].Play();
            //FocusFire[3].Play();
        }
        if (shootPos[1] == null)
        {
            shootRate -= 0.2f;
            turnspeed = turnspeed / 2;
            FocusFire.Play();
            //FocusFire[0].Play();
            //FocusFire[2].Play();
            //FocusFire[3].Play();
        }
        if (shootPos[2] == null)
        {
            shootRate -= 0.2f;
            turnspeed = turnspeed / 2;
            FocusFire.Play();
            //FocusFire[1].Play();
            //FocusFire[0].Play();
            //FocusFire[3].Play();
        }
        if (shootPos[3] == null)
        {
            shootRate -= 0.2f;
            turnspeed = turnspeed / 2;
            FocusFire.Play();
            //FocusFire[1].Play();
            //FocusFire[2].Play();
            //FocusFire[0].Play();
        }
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
        isShooting = false;
    }

    IEnumerator flashMuzzle(GameObject muzzleFlash)
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(.1f);
            if (muzzleFlash != null)
            {
                muzzleFlash.SetActive(false);
            }
        }
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