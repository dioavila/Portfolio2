using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireKnife : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] int Time;
    [SerializeField] int speed;
    [SerializeField] int Damage;
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

    private void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroytime);
    }


    public void OnCollisionEnter(Collision other)
    {
        //if (!other.gameObject.CompareTag("Player"))
        //{
        //    IFire fmg = other.gameObject.GetComponent<IFire>();
        //    if (fmg != null)
        //    {
        //        fmg.FireDamage(Damage, Time);
        //    }
        //    Destroy(gameObject);
        //}

        if (!other.gameObject.CompareTag("Player"))
        {
            IFire fmg = other.gameObject.GetComponent<IFire>();
            if (fmg != null && other.gameObject.CompareTag("Enemy") && !hitBlocked)
            {
                fmg.FireDamage(Damage, Time);
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
