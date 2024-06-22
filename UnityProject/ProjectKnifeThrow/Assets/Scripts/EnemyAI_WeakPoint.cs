using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class EnemyAI_WeakPoint : MonoBehaviour, IDamage, IFire
{
    [SerializeField] int HP;
    [SerializeField] bool partOfList;
    [SerializeField] ParticleSystem particle;
    [SerializeField] AudioSource takeDamageSound;
    [SerializeField] AudioSource weakpointSound;
    Renderer model;
    //[SerializeField] int listElementNum;

    private void Start()
    {
        model = GetComponent<Renderer>();
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        takeDamageSound.Play();
        StartCoroutine(flashred());
        if (HP <= 0)
        {
            if (partOfList)
            {
                int currIndex = GameManager.instance.bossManager.weakspotList.IndexOf(gameObject);
                GameManager.instance.bossManager.weakspotList.Remove(GameManager.instance.bossManager.weakspotList[currIndex]);
            }
            Instantiate(particle, transform.position, Quaternion.identity);
            weakpointSound.Play();
            Destroy(gameObject);
        }
    }

    IEnumerator flashred()
    {
        Color temp = model.material.color;
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = temp;
    }

    public void FireDamage(int amount, int time)
    {
        StartCoroutine(FireTime(amount, time));
    }

    IEnumerator FireTime(int amount, int time)
    {
        Color temp = model.material.color;
        for (int i = 0; i <= time; i++)
        {
            HP -= amount;
            takeDamageSound.Play();
            model.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            model.material.color = temp;
            yield return new WaitForSeconds(0.5f);
            if (HP <= 0)
            {
                Instantiate(particle, transform.position, Quaternion.identity);
                weakpointSound.Play();
                Destroy(gameObject);
            }
        }
    }
}
