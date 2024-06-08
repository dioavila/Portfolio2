using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class Bomb_AI : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int explosionRadMax;
    [SerializeField] MeshRenderer bombRenderer;

    bool expandRadius;

    // Start is called before the first frame update
    void Start()
    {
        expandRadius = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (expandRadius)
        {
            gameObject.GetComponent<SphereCollider>().radius += 0.5f;
            if (gameObject.GetComponent<SphereCollider>().radius >= explosionRadMax)
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IDamage dmg = other.gameObject.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //gameObject.GetComponent<SphereCollider>().isTrigger = true;
        expandRadius = true;
        bombRenderer.enabled = false;
    }
}
