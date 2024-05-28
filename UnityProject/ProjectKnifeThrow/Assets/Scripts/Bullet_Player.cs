using System.Collections;
using System.Collections.Generic;
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
        IDamage dmg = other.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            Instantiate(hitConfirm, transform.position, Quaternion.identity);
            dmg.TakeDamage(Damage);
        }
        else if (dmg == null && other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(hitBlock, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
