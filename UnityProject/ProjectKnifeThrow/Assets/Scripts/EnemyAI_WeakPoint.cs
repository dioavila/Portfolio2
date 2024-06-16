using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_WeakPoint : MonoBehaviour, IDamage
{
    [SerializeField] int HP;
    [SerializeField] bool partOfList;
    //[SerializeField] int listElementNum;
    [SerializeField] ParticleSystem particle;
    //Add code to remove weakspot from the list;

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            if (partOfList)
            {
                int currIndex = GameManager.instance.bossManager.weakspotList.IndexOf(gameObject);
                GameManager.instance.bossManager.weakspotList.Remove(GameManager.instance.bossManager.weakspotList[currIndex]);
            }
            Instantiate(particle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
