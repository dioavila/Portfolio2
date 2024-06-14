using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class Bomb_AI : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int explosionRadMax;
    [SerializeField] MeshRenderer bombRenderer;
    public ParticleSystem bombExplosion;

    bool bombDetonate = false;
    bool deleteBomb = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (bombDetonate)
        {
            gameObject.GetComponent<SphereCollider>().radius += 0.5f;
            //if (gameObject.GetComponent<SphereCollider>().radius >= explosionRadMax)
            //{
            //    
            //}
            bombExplosion.Play();
            bombDetonate = false;
            deleteBomb = true;
        }
        if (deleteBomb)
        {
            StartCoroutine(destroyBomb());
        }
    }

    IEnumerator destroyBomb()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
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
        //if (collision.collider.CompareTag("Enemy"))
        //    return;
        //else
        //{
        //}
            bombRenderer.enabled = false;
            bombDetonate = true;
    }
}
