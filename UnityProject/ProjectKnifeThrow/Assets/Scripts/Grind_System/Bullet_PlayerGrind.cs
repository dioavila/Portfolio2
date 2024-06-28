using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] public int Speed;
    [SerializeField] int DestroyTime;
    [SerializeField] GameObject knifeModel;
    bool knifeOn = false;
    //bool destroyedOnCollision = false;
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
            if (dmg != null)
            {
                Destroy(gameObject);
            }
            else
            {
                //RaycastHit hit;

                //rb.velocity *= 0;
                //rb.isKinematic = true;
                ////ContactPoint collisionPoint = other.GetContact(0);

                //Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hit);
                //Debug.DrawRay(transform.position, hit.normal, Color.blue, 100);
                ////destroyedOnCollision = true;

                //Vector3 opposite = -hit.normal;
                if (!knifeOn) { 
                Vector3 opposite = -other.contacts[0].normal;
                Quaternion rotation = Quaternion.FromToRotation(knifeModel.transform.forward, opposite);
                //Instantiate(knifeModel, transform.position, rotation );
                Instantiate(knifeModel, other.contacts[0].point, rotation);
                    knifeOn = true;
                }
                Destroy(gameObject);
            }
        }
    }

    //private void OnDestroy()
    //{
    //    if (!destroyedOnCollision)
    //    {
    //        GameManager.instance.playerScript.gThrowCount++;
    //        //GameManager.instance.playerScript.resetOn = true;
    //    }
    //}


}
