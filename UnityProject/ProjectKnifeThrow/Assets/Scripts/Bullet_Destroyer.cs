using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] float timeToDestroy;
    [SerializeField] int damage;
    public Transform playerLocation;

    // Start is called before the first frame update
    void Start()
    {
        if(transform.parent != null)
            transform.rotation = transform.parent.rotation;
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, timeToDestroy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        if (!other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            return;
        }

        IDamage dmg = other.gameObject.GetComponent<IDamage>();

        if (dmg != null)
        {
            dmg.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
