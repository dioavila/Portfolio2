using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] int Damage;
    [SerializeField] int Speed;
    [SerializeField] int DestroyTime;

    [Header("Particle System")]
    [SerializeField] ParticleSystem hitConfirm;
    [SerializeField] ParticleSystem hitBlock;
    [SerializeField] ParticleSystem hitMiss;
    bool hitConfirmed = false;
    bool hitBlocked = false;

    [SerializeField] List<AudioClip> collisionAudio;
    [SerializeField] AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * Speed;
        Destroy(gameObject, DestroyTime);
    }
   
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            IDamage dmg = other.gameObject.GetComponent<IDamage>();
            if (dmg != null && other.gameObject.CompareTag("critPoint") && !hitBlocked)
            {
                dmg.TakeDamage(Damage);
                Instantiate(hitConfirm, transform.position, Quaternion.identity);
                hitConfirmed = true;
                Destroy(gameObject);
                return;
                
            }
            else
            {
                if (dmg == null && !hitConfirmed && other.gameObject.CompareTag("Enemy"))
                {
                    Instantiate(hitBlock, transform.position, Quaternion.identity);
                    source.clip = collisionAudio[0];
                    source.pitch = Random.Range(0.50f, 0.80f);
                    source.Play();
                    hitBlocked = true;
                }
                else
                {
                    Instantiate(hitMiss, transform.position, Quaternion.identity);
                    source.clip = collisionAudio[1];
                    source.pitch = Random.Range(0.50f, 0.80f);
                    source.Play();
                }
                Destroy(gameObject, 1);
            }
        }
    }
}
