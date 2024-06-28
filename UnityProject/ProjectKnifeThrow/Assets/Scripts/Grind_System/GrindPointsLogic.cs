using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindPointsLogic : MonoBehaviour
{
    [Header("Grind Logic")]
    [SerializeField] Transform gPoint;
    public bool inRangePlayer = false;

    [SerializeField] ParticleSystem particles;
    [SerializeField] ParticleSystem particlesB;
    ParticleSystem currParticle, baseParticle;

    [SerializeField] AudioSource baseSource;
    [SerializeField] AudioClip baseClip;
    [SerializeField] AudioSource pointSource;
    [SerializeField] AudioClip pointClip;

    [SerializeField] Material rPMat;
    [SerializeField] Material oMat;
    //[Header("Destruction Settings")]
    //[SerializeField] int destructionTimerMax;
    //[SerializeField] [Range(0,3)] int destructionSpeed;
    //float destructionTimerCurr;

    // Start is called before the first frame update
    void Start()
    {
        //Can use start to initiate VFX
        baseSource.clip = baseClip;
        baseSource.Play();
        pointSource.clip = pointClip;
        GameManager.instance.grindScript.grindPoints.Add(gPoint);
        currParticle = Instantiate(particles, gPoint.position, Quaternion.identity);
        baseParticle = Instantiate(particlesB, transform.position, Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.grindScript.grindPoints.IndexOf(gPoint) == GameManager.instance.grindScript.grindPoints.Count - 1)
        {
            currParticle.GetComponent<ParticleSystemRenderer>().material = rPMat;
        }
        else
        {
            currParticle.GetComponent<ParticleSystemRenderer>().material = oMat;
        }
        if (GameManager.instance.playerScript.recoverOn)
        {
            DestroyStart();
        }
    }

    void DestroyStart()
    {
        GameManager.instance.playerScript.gThrowCount--;
        currParticle.Stop();
        baseParticle.Stop();
        currParticle.Clear();
        baseParticle.Clear();
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.instance.grindScript.grindPoints.IndexOf(gPoint) == GameManager.instance.grindScript.grindPoints.Count - 1)
        {
            pointSource.Play();
            inRangePlayer = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRangePlayer = false;
        }
    }
     void OnDestroy()
    {
        //GameManager.instance.grindScript.grindPoints.Remove(gPoint);
        GameManager.instance.grindScript.destroyedCount++;
    }
}
