using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaseysAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] int HP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int amount)
   {
       HP -= amount;
      // agent.SetDestination(GameManager.instance.player.transform.position);
       StartCoroutine(flashred());

       if (HP <= 0)
       {
           //GameManager.instance.updateGameGoal(-1);
           Destroy(gameObject);
       }
   }

   IEnumerator flashred()
   {
       model.material.color = Color.red;
       yield return new WaitForSeconds(0.1f);
       model.material.color = Color.white;
   }
}
