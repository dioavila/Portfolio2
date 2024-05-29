using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] int speed;
    [SerializeField] int time;
    [SerializeField] int destroytime;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroytime);
    }

    private void OnCollisionEnter(Collision other)
    {
        IFreeze dmg = other.gameObject.GetComponent<IFreeze>();
        if (dmg != null)
        {
            dmg.FreezeTime(time);
        }
        Destroy(gameObject);
    }
}

