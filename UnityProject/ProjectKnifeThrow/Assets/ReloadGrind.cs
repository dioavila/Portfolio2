using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadGrind : MonoBehaviour
{
    public void RecoverGKnives()
    {
        GameManager.instance.playerScript.recoverOn = true;
    }

    public void ResetGKnives()
    {
        GameManager.instance.playerScript.recoverOn = false;
        GameManager.instance.playerScript.GKnifeDisplayReset();
    }
}
