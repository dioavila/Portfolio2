using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] int Damage;
    [SerializeField] int Speed;
    [SerializeField] int DestroyTime;
    [SerializeField] GameObject knifeModel;
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
            Destroy(gameObject);
        }
        else
        {
            RaycastHit hit;
            
            rb.velocity *= 0;
            rb.isKinematic = true;
            ContactPoint collisionPoint = other.GetContact(0);
            
            Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hit, 100f);
            Debug.DrawRay(collisionPoint.point, hit.normal, Color.blue, 100);
            Destroy(gameObject);

            Vector3 opposite = -hit.normal;
            Quaternion rotation = Quaternion.FromToRotation(knifeModel.transform.forward, opposite);
            Instantiate(knifeModel, collisionPoint.point, rotation);
        }
    }
}
