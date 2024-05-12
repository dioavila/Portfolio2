using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointDamage : MonoBehaviour, IDamage
{
    [SerializeField] int HP;

    int origHP;
    // Start is called before the first frame update
    void Start()
    {
        origHP = HP;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if ( HP <= 0 ) 
        {
            Destroy(gameObject);
        }
    }
}