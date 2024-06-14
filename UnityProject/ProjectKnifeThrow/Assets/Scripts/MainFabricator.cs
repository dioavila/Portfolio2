using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainFabricator : MonoBehaviour
{
    [Header("First Floor")]
    [SerializeField] GameObject eyeF1One;
    [SerializeField] GameObject eyeF1Two;
    [SerializeField] GameObject eyeF1Three;
    [SerializeField] GameObject eyeF1Four;
    [SerializeField] GameObject eyeF1Five;
    [SerializeField] GameObject laserF1Wall;
    [SerializeField] GameObject laserF1Door;

    Vector3 eyeF1OneLoc;
    Vector3 eyeF1TwoLoc;
    Vector3 eyeF1ThreeLoc;
    Vector3 eyeF1FourLoc;
    Vector3 eyeF1FiveLoc;

    [Header("Eye Explosion Effect")]
    public ParticleSystem eyeExplosionEffect;

    bool eyeExplode1 = false;
    bool eyeExplode2 = false;
    bool eyeExplode3 = false;
    bool eyeExplode4 = false;
    bool eyeExplode5 = false;

    // Start is called before the first frame update
    void Start()
    {
        eyeF1OneLoc = eyeF1One.transform.position;
        eyeF1TwoLoc = eyeF1Two.transform.position;
        eyeF1ThreeLoc = eyeF1Three.transform.position;
        eyeF1FourLoc = eyeF1Four.transform.position;
        eyeF1FiveLoc = eyeF1Five.transform.position;
    }

    // Update is called once per framea
    void Update() 
    {
        destroyF1WallLaser();
        destroyF1DoorLaser();
    }

    private void destroyF1WallLaser()
    {

        if (eyeF1One == null && !eyeExplode1)
        {
            Instantiate(eyeExplosionEffect, eyeF1OneLoc, Quaternion.identity);
            eyeExplode1 = true;
        }
        if (eyeF1Two == null && !eyeExplode2)
        {
            Instantiate(eyeExplosionEffect, eyeF1TwoLoc, Quaternion.identity);
            eyeExplode2 = true;
        }
        if (eyeF1One == null & eyeF1Two == null)
            Destroy(laserF1Wall);
    }

    private void destroyF1DoorLaser()
    {
        if (eyeF1Three == null && !eyeExplode3)
        {
            Instantiate(eyeExplosionEffect, eyeF1ThreeLoc, Quaternion.identity);
            eyeExplode3 = true;
        }
        if (eyeF1Four == null && !eyeExplode4)
        {
            Instantiate(eyeExplosionEffect, eyeF1FourLoc, Quaternion.identity);
            eyeExplode4 = true;
        }
        if (eyeF1Five == null && !eyeExplode5)
        {
            Instantiate(eyeExplosionEffect, eyeF1FiveLoc, Quaternion.identity);
            eyeExplode5 = true;
        }
        if (eyeF1Three == null & eyeF1Four == null && eyeF1Five == null)
            Destroy(laserF1Door);
    }
    
}
