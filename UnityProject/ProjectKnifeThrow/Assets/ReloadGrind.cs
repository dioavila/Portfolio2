using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadGrind : MonoBehaviour
{
    [SerializeField] ParticleSystem rechargeEffect;
    [SerializeField] GameObject particlePosition;
    public void RecoverGKnives()
    {
        GameManager.instance.playerScript.recoverOn = true;
    }

    public void ResetGKnives()
    {
        GameManager.instance.playerScript.recoverOn = false;
        GameManager.instance.playerScript.GKnifeDisplayReset();
    }

    public void PlayEffect()
    {
        ParticleSystem part = Instantiate(rechargeEffect, particlePosition.transform.position, Quaternion.identity, particlePosition.transform);
      
        
    }

}
