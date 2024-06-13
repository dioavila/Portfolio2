using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Flying : MonoBehaviour, IFreeze
{
    [SerializeField] Renderer modelTop;
    [SerializeField] Renderer modelBottom;
    [SerializeField] Transform bombPos;
    [SerializeField] GameObject weakPoint;
    [SerializeField] GameObject eyePos;

    [SerializeField] GameObject eyeball;
    [SerializeField] ParticleSystem robotExplosion;

    [SerializeField] GameObject bomb;
    [SerializeField] float bombReload;
    [SerializeField] int deathTimer;

    public NavMeshAgent agent;

    bool hasBomb;
    bool canShoot;
    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        hasBomb = true;
    }

    // Update is called once per frame
    void Update()
    {
        eyeTrack();
        if (!isDead)
        {
            if (modelTop.material.color == Color.blue && modelBottom.material.color == Color.blue)
            {
                agent.isStopped = true;
                canShoot = false;
            }
            else
            {
                agent.isStopped = false;
                canShoot = true;
            }
            agent.SetDestination(GameManager.instance.player.transform.position);
            if (hasBomb && (agent.transform.position.x - GameManager.instance.player.transform.position.x) <= 2.0f && (agent.transform.position.z - GameManager.instance.player.transform.position.z) <= 2.0f)
            {
                StartCoroutine(bombDrop());
            }
            if (weakPoint == null)
            {
                isDead = true;
                robotExplosion.Play();
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

    IEnumerator bombDrop()
    {
        if (hasBomb)
        {
            createBomb(bombPos);
            hasBomb = false;
        }
        yield return new WaitForSeconds(bombReload);
        hasBomb = true;
    }

    public void createBomb(Transform bombPos)
    {
        Instantiate(bomb, bombPos.position, transform.rotation);
    }

    private void eyeTrack()
    {
        eyePos.transform.LookAt(GameManager.instance.player.transform.position);
    }

    public void FreezeTime(int time)
    {
        StartCoroutine(FlashBlue(time));
    }

    IEnumerator FlashBlue(int time)
    {
        modelTop.material.color = Color.blue;
        modelBottom.material.color = Color.blue;
        yield return new WaitForSeconds(time);
        modelTop.material.color = Color.white;
        modelBottom.material.color = Color.white;
    }
}
