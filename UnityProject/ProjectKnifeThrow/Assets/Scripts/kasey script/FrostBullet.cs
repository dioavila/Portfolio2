using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] int speed;
    [SerializeField] int time;
    [SerializeField] int MaxCount;
    [SerializeField] int CurrCount;
    [SerializeField] int destroytime;

    [Header("Particle System")]
    //[SerializeField] ParticleSystem iceEffect;
    [SerializeField] ParticleSystem hitConfirm;
    [SerializeField] ParticleSystem hitBlock;
    [SerializeField] ParticleSystem hitMiss;
    //bool hitConfirmed = false;
    bool hitBlocked = false;

    [Header("Audio System")]
    [SerializeField] List<AudioClip> collisionAudio;
    [SerializeField] AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
        //iceEffect
        Destroy(gameObject, destroytime);
    }

    private void OnCollisionEnter(Collision other)
    {
        //IFreeze dmg = other.gameObject.GetComponent<IFreeze>();
        //if (dmg != null)
        //{
        //    dmg.FreezeTime(time);
        //}
        //Destroy(gameObject);

        if (!other.gameObject.CompareTag("Player"))
        {
            IFreeze dmg = other.gameObject.GetComponent<IFreeze>();
            if (dmg != null && other.gameObject.CompareTag("Enemy") && !hitBlocked)
            {
                dmg.FreezeTime(time);
                Instantiate(hitConfirm, transform.position, Quaternion.identity);
                //hitConfirmed = true;
                Destroy(gameObject);
                return;

            }
            else
            {
                hitBlocked = true;
                Instantiate(hitMiss, transform.position, Quaternion.identity);
                source.clip = collisionAudio[1];
                source.pitch = Random.Range(0.50f, 0.80f);
                source.Play();
                Destroy(gameObject, 1);
            }
        }
    }
}

