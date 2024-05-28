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
    // Start is called before the first frame update
    void Start()
    {
       // shootPos = GameManager.instance.playerScript.playerShootPos;
      //  shootPosGrind = GameManager.instance.playerScript.playerShootPos;
        //selectKnife = GameManager.instance.playerScript.selectedKnife;
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
       // Instantiate(GameManager.instance.playerScript.knifeList[selectKnife].Knife, shootPos.position, Camera.main.transform.rotation);
    }
}
