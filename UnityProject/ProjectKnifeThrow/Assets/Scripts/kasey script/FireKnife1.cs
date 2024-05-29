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

    private void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroytime);
    }


    public void OnCollisionEnter(Collision other)
    {
        IFire dmg = other.gameObject.GetComponent<IFire>();
        if (dmg != null)
        {
            dmg.FireDamage(Damage, Time);
        }
        Destroy(gameObject);
    }

}