using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] ParticleSystem spark;
    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        if (!other.gameObject.CompareTag("Player")){
            Destroy(gameObject);
            return;
        }

        IDamage dmg = other.gameObject.GetComponent<IDamage>();

        if (dmg != null && !GameManager.instance.playerScript.isDead)
        {
            dmg.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

}