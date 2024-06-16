using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindPointsLogic : MonoBehaviour
{
    [Header("Grind Logic")]
    [SerializeField] Transform gPoint;
    public bool inRangePlayer = false;
    [SerializeField] List<AudioClip> gKAudio;
    [SerializeField] AudioSource gKAudioSource;
    [SerializeField] AudioSource reachSource;
    [SerializeField] ParticleSystem particles;
    ParticleSystem currParticle, particleBase;
    //[Header("Destruction Settings")]
    //[SerializeField] int destructionTimerMax;
    //[SerializeField] [Range(0,3)] int destructionSpeed;
    //float destructionTimerCurr;

    // Start is called before the first frame update
    void Start()
    {
        //Can use start to initiate VFX
        GameManager.instance.grindScript.grindPoints.Add(gPoint);
        gKAudioSource.clip = gKAudio[0];
        gKAudioSource.Play();
        currParticle = Instantiate(particles, gPoint.position, Quaternion.identity);
        particleBase = Instantiate(particles, transform.position, Quaternion.identity);
        //destructionTimerCurr = destructionTimerMax;
    }

    // Update is called once per frame
    void Update()
    {
        //if(GameManager.instance.grindScript.grindPoints.Count >= 4)
        //{
        //    DestroyStart();
        //}
        if (GameManager.instance.playerScript.recoverOn)
        {
            DestroyStart();
        }
    }

    void DestroyStart()
    {
        //if (destructionTimerCurr > 0)
        //{
        //    destructionTimerCurr -= Time.deltaTime * destructionSpeed;
        //}
        //else if (destructionTimerCurr <= 0 && !GameManager.instance.playerScript.isGrinding)
        //{
        GameManager.instance.playerScript.gThrowCount--;
        currParticle.Stop();
        particleBase.Stop();
        particleBase.Clear();
        currParticle.Clear();
        gKAudioSource.Stop();
        Destroy(gameObject);
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int index = GameManager.instance.grindScript.grindPoints.IndexOf(gPoint);
            if (index == GameManager.instance.grindScript.grindPoints.Count - 1)
            {
                reachSource.PlayOneShot(gKAudio[1], 1f);
                inRangePlayer = true;
            }
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
