using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Flying : MonoBehaviour, IFreeze
{
    [SerializeField] Renderer modelTop;
    [SerializeField] Renderer modelBottom;
    [SerializeField] Transform bombPos;
    [SerializeField] GameObject critPoint;
    [SerializeField] GameObject eyePos;

    [SerializeField] GameObject bomb;
    [SerializeField] float bombReload;

    public NavMeshAgent agent;

    bool hasBomb;
    bool canShoot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        eyeTrack();

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

        if (hasBomb)
        {
            agent.SetDestination(GameManager.instance.player.transform.position);

            if (agent.remainingDistance <= 5.0f)
            {
                dropBomb();
                //attacking = false;
            }
        }
        else if (!hasBomb)
        {
            agent.SetDestination(new Vector3(agent.transform.localPosition.x + agent.transform.forward.x, agent.transform.localPosition.y, agent.transform.localPosition.z + agent.transform.forward.z));
            StartCoroutine(bombReloadTimer());
        }

        if(critPoint == null)
        {
            Destroy(agent.gameObject);
        }
    }

    private void dropBomb()
    {
        //createBomb(bombPos);
        hasBomb = false;
    }

    IEnumerator bombReloadTimer()
    {
        yield return new WaitForSeconds(bombReload);
        hasBomb = true;
    }

    private void eyeTrack()
    {
        eyePos.transform.LookAt(GameManager.instance.player.transform.position);
    }

    public void createBomb(Transform bombPos)
    {
        Instantiate(bomb, bombPos.position, transform.rotation);
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
