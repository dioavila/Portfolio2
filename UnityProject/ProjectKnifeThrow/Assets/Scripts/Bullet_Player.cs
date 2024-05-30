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
            if (dmg != null && other.gameObject.CompareTag("critPoint"))
            {
                dmg.TakeDamage(Damage);
                Instantiate(hitConfirm, transform.position, Quaternion.identity);
                Destroy(gameObject);
                return;
                
            }
            else
            {
                if (dmg == null)// && other.gameObject.CompareTag("Enemy"))
                {
                    Instantiate(hitBlock, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            }
        }
    }
}
