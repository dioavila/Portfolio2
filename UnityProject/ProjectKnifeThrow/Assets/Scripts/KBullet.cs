using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] int Damage;
    [SerializeField] int Speed;
    [SerializeField] int DestroyTime;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * Speed;
        Destroy(gameObject, DestroyTime);
    }
   
    private void OnCollisionEnter(Collision other)
    {   
        IDamage dmg = other.gameObject.GetComponent<IDamage>();
        if(dmg != null )
        {
            dmg.TakeDamage(Damage);
        }
        Destroy(gameObject);
    }
}
