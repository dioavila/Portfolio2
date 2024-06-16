using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBullet : MonoBehaviour
{
    [SerializeField] GameObject grindBullet;
    [SerializeField] GameObject regKnifeModel;
    Transform shootPosGrind;
    Transform shootPos;
    int selectKnife;
    int upForce;
    // Start is called before the first frame update
    void Start()
    {
        shootPos = GameManager.instance.playerScript.playerShootPos;
        selectKnife = GameManager.instance.playerScript.selectedKnife;
        upForce = GameManager.instance.playerScript.UpWardForce;
        //shootPosGrind = GameManager.instance.playerScript.playerShootPos;
    }

    // Update is called once per frame
    void Update()
    {
       if(selectKnife != GameManager.instance.playerScript.selectedKnife)
       {
           selectKnife = GameManager.instance.playerScript.selectedKnife;
       }
    }

    //public void CreateBG()
    //{
    //    Instantiate(grindBullet, shootPosGrind.position, Camera.main.transform.rotation);
    //}

    public void CreateB()
    {
        regKnifeModel.SetActive(false);
        GameObject Projectile = Instantiate(GameManager.instance.playerScript.knifeList[selectKnife].Knife, shootPos.position, Camera.main.transform.rotation);
        GameManager.instance.playerScript.attackDecreaseCurr = GameManager.instance.playerScript.attackDecreaseAmmt;
        Rigidbody ProjectileRB = Projectile.GetComponent<Rigidbody>();
        Vector3 ForceDir = Camera.main.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 500f))
        {
            ForceDir = (hit.point - shootPos.position).normalized;
        }

        Vector3 forcetoadd = ForceDir * GameManager.instance.playerScript.knifeList[selectKnife].speed + GameManager.instance.playerScript.knifeList[selectKnife].Knife.transform.up * upForce;

        ProjectileRB.AddForce(forcetoadd, ForceMode.Impulse);
        //Instantiate(GameManager.instance.playerScript.knifeList[selectKnife].Knife, shootPos.position, Camera.main.transform.rotation);
    }
}
