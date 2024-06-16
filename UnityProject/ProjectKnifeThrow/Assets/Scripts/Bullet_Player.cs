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

    [SerializeField] AudioSource knifeAudio;
    [SerializeField] AudioClip deflectAudio;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * Speed;
        knifeAudio.clip = deflectAudio;
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
<<<<<<< Updated upstream
                    bool hitBlocked = true;
=======
                    knifeAudio.pitch = Random.Range(0.20f, 0.50f);
                    knifeAudio.volume = 0.2f;
                    knifeAudio.Play();
                    hitBlocked = true;
>>>>>>> Stashed changes
                }
                else
                {
                    Instantiate(hitMiss, transform.position, Quaternion.identity);
                }
                Destroy(gameObject,1);
            }
        }
    }
}
