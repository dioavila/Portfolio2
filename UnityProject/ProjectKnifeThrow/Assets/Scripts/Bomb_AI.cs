using System.Collections;
using System.Collections.Generic;
//using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class Bomb_AI : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] MeshRenderer bombRenderer;
    public ParticleSystem bombExplosion;

    [Header("Sound")]
    [SerializeField] AudioSource bombSource;

    bool bombDetonate = false;
    bool deleteBomb = false;
    bool tookDmg = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (bombDetonate)
        {
            bombExplosion.Play();
            bombSource.Play();
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
        yield return new WaitForSeconds(1.0f);
        tookDmg = true;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!tookDmg)
            {
                IDamage dmg = other.gameObject.GetComponent<IDamage>();

                if (dmg != null)
                {
                    dmg.TakeDamage(damage);
                }
                StartCoroutine(destroyBomb());
            }
        }
        else
        {
            StartCoroutine(destroyBomb());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponent<SphereCollider>().enabled = true;
        bombRenderer.enabled = false;
        bombDetonate = true;
    }

}
