using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainFabricator : MonoBehaviour
{
    [Header("---First Floor---")]
    [SerializeField] GameObject eyeF1One;
    [SerializeField] GameObject eyeF1Two;
    [SerializeField] GameObject eyeF1Three;
    [SerializeField] GameObject eyeF1Four;
    [SerializeField] GameObject eyeF1Five;
    [SerializeField] GameObject laserF1Wall;
    [SerializeField] GameObject laserF1Door;

    //Locations for corresponding eye
    Vector3 eyeF1OneLoc;
    Vector3 eyeF1TwoLoc;
    Vector3 eyeF1ThreeLoc;
    Vector3 eyeF1FourLoc;
    Vector3 eyeF1FiveLoc;

    [Header("---Second Floor---")]
    [SerializeField] List<GameObject> eyeParents;
    [SerializeField] GameObject laserF2;
    bool F2Laser = true;

    [Header("Glass Set")]
    //List for all the gameobject eyes to go in
    [SerializeField] GameObject glassEyeParent;
    [SerializeField] List<GameObject> glassSet;
    [SerializeField] GameObject glassEyeOne;
    [SerializeField] GameObject glassEyeTwo;
    [SerializeField] GameObject glassEyeThree;
    [SerializeField] GameObject glassEyeFour;
    [SerializeField] GameObject glassEyeFive;
    [SerializeField] GameObject glassEyeSix;

    //Locations for corresponding eye
    Vector3 glassEyeOneLoc;
    Vector3 glassEyeTwoLoc;
    Vector3 glassEyeThreeLoc;
    Vector3 glassEyeFourLoc;
    Vector3 glassEyeFiveLoc;
    Vector3 glassEyeSixLoc;

    [Header("Top Set")]
    //List for all the gameobject eyes to go in
    [SerializeField] GameObject topEyeParent;
    [SerializeField] List<GameObject> topSet;
    [SerializeField] GameObject topEyeOne;
    [SerializeField] GameObject topEyeTwo;
    [SerializeField] GameObject topEyeThree;
    [SerializeField] GameObject topEyeFour;
    [SerializeField] GameObject topEyeFive;
    [SerializeField] GameObject topEyeSix;
    [SerializeField] GameObject topEyeSeven;
    [SerializeField] GameObject topEyeEight;
    [SerializeField] GameObject topEyeNine;
    [SerializeField] GameObject topEyeTen;
    [SerializeField] GameObject topEyeEleven;
    [SerializeField] GameObject topEyeTwelve;

    //Locations for corresponding eye
    Vector3 topEyeOneLoc;
    Vector3 topEyeTwoLoc;
    Vector3 topEyeThreeLoc;
    Vector3 topEyeFourLoc;
    Vector3 topEyeFiveLoc;
    Vector3 topEyeSixLoc;
    Vector3 topEyeSevenLoc;
    Vector3 topEyeEightLoc;
    Vector3 topEyeNineLoc;
    Vector3 topEyeTenLoc;
    Vector3 topEyeElevenLoc;
    Vector3 topEyeTwelveLoc;

    [Header("Bottom Set")]
    //List for all the gameobject eyes to go in
    [SerializeField] GameObject bottomEyeParent;
    [SerializeField] List<GameObject> bottomSet;
    [SerializeField] GameObject bottomEyeOne;
    [SerializeField] GameObject bottomEyeTwo;
    [SerializeField] GameObject bottomEyeThree;
    [SerializeField] GameObject bottomEyeFour;
    [SerializeField] GameObject bottomEyeFive;
    [SerializeField] GameObject bottomEyeSix;
    [SerializeField] GameObject bottomEyeSeven;
    [SerializeField] GameObject bottomEyeEight;
    [SerializeField] GameObject bottomEyeNine;
    [SerializeField] GameObject bottomEyeTen;
    [SerializeField] GameObject bottomEyeEleven;
    [SerializeField] GameObject bottomEyeTwelve;

    //Locations for corresponding eye
    Vector3 bottomEyeOneLoc;
    Vector3 bottomEyeTwoLoc;
    Vector3 bottomEyeThreeLoc;
    Vector3 bottomEyeFourLoc;
    Vector3 bottomEyeFiveLoc;
    Vector3 bottomEyeSixLoc;
    Vector3 bottomEyeSevenLoc;
    Vector3 bottomEyeEightLoc;
    Vector3 bottomEyeNineLoc;
    Vector3 bottomEyeTenLoc;
    Vector3 bottomEyeElevenLoc;
    Vector3 bottomEyeTwelveLoc;

    [Header("Third Floor")]


    [Header("Eye Explosion Effect")]
    public ParticleSystem eyeExplosionEffect;
    public AudioSource eyeExplosionSound;

    //Bool for if the eye has exploded
    bool eyeF1Explode1 = false;
    bool eyeF1Explode2 = false;
    bool eyeF1Explode3 = false;
    bool eyeF1Explode4 = false;
    bool eyeF1Explode5 = false;

    //Bool for if the eye has exploded
    bool glassEyeOneExplode = false;
    bool glassEyeTwoExplode = false;
    bool glassEyeThreeExplode = false;
    bool glassEyeFourExplode = false;
    bool glassEyeFiveExplode = false;
    bool glassEyeSixExplode = false;

    //Bool for if the eye has exploded
    bool topEyeOneExplode = false;
    bool topEyeTwoExplode = false;
    bool topEyeThreeExplode = false;
    bool topEyeFourExplode = false;
    bool topEyeFiveExplode = false;
    bool topEyeSixExplode = false;
    bool topEyeSevenExplode = false;
    bool topEyeEightExplode = false;
    bool topEyeNineExplode = false;
    bool topEyeTenExplode = false;
    bool topEyeElevenExplode = false;
    bool topEyeTwelveExplode = false;

    //Bool for if the eye has exploded
    bool bottomEyeOneExplode = false;
    bool bottomEyeTwoExplode = false;
    bool bottomEyeThreeExplode = false;
    bool bottomEyeFourExplode = false;
    bool bottomEyeFiveExplode = false;
    bool bottomEyeSixExplode = false;
    bool bottomEyeSevenExplode = false;
    bool bottomEyeEightExplode = false;
    bool bottomEyeNineExplode = false;
    bool bottomEyeTenExplode = false;
    bool bottomEyeElevenExplode = false;
    bool bottomEyeTwelveExplode = false;

    //Spawn enemy settings
    [Header("Spawner")]
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] GameObject floor2RoomEnemy;
    [SerializeField] int spawnTimer;
    [SerializeField] int numberToSpawn;
    [SerializeField] List<Transform> spawnLocationList = new List<Transform>();
    [SerializeField] List<Transform> spawn2ndFloorLocs = new List<Transform>();
    [SerializeField] bool goal = false;
    int numberSpawned = 0;

    bool floor2Start = true;
    bool glassSpawn = false;
    bool topSpawn = false;
    bool bottomSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        //First floor eye position holders for explosion effect
        eyeF1OneLoc = eyeF1One.transform.position;
        eyeF1TwoLoc = eyeF1Two.transform.position;
        eyeF1ThreeLoc = eyeF1Three.transform.position;
        eyeF1FourLoc = eyeF1Four.transform.position;
        eyeF1FiveLoc = eyeF1Five.transform.position;

        //Second floor glass eye set position holders for explosion effect
        glassEyeOneLoc = glassEyeOne.transform.position;
        glassEyeTwoLoc = glassEyeTwo.transform.position;
        glassEyeThreeLoc = glassEyeThree.transform.position;
        glassEyeFourLoc = glassEyeFour.transform.position;
        glassEyeFiveLoc = glassEyeFive.transform.position;
        glassEyeSixLoc = glassEyeSix.transform.position;

        //Second floor top eye set position holders for explosion effect
        topEyeOneLoc = topEyeOne.transform.position;
        topEyeTwoLoc = topEyeTwo.transform.position;
        topEyeThreeLoc = topEyeThree.transform.position;
        topEyeFourLoc = topEyeFour.transform.position;
        topEyeFiveLoc = topEyeFive.transform.position;
        topEyeSixLoc = topEyeSix.transform.position;
        topEyeSevenLoc = topEyeSeven.transform.position;
        topEyeEightLoc = topEyeEight.transform.position;
        topEyeNineLoc = topEyeNine.transform.position;
        topEyeTenLoc = topEyeTen.transform.position;
        topEyeElevenLoc = topEyeEleven.transform.position;
        topEyeTwelveLoc = topEyeTwelve.transform.position;

        //Second floor bottom eye set position holders for explosion effect
        bottomEyeOneLoc = bottomEyeOne.transform.position;
        bottomEyeTwoLoc = bottomEyeTwo.transform.position;
        bottomEyeThreeLoc = bottomEyeThree.transform.position;
        bottomEyeFourLoc = bottomEyeFour.transform.position;
        bottomEyeFiveLoc = bottomEyeFive.transform.position;
        bottomEyeSixLoc = bottomEyeSix.transform.position;
        bottomEyeSevenLoc = bottomEyeSeven.transform.position;
        bottomEyeEightLoc = bottomEyeEight.transform.position;
        bottomEyeNineLoc = bottomEyeNine.transform.position;
        bottomEyeTenLoc = bottomEyeTen.transform.position;
        bottomEyeElevenLoc = bottomEyeEleven.transform.position;
        bottomEyeTwelveLoc = bottomEyeTwelve.transform.position;

        //Spawner settings
        if (goal)
        {
            GameManager.instance.updateGameGoal(numberToSpawn);
        }
    }

    // Update is called once per framea
    void Update()
    {
        destroyF1WallLaser();
        destroyF1DoorLaser();
        initial2ndFloorSpawn();
        activateWave1();
        activateWave2();
        activateWave3();
        destroyF2Laser();
    }

    IEnumerator waitDestroyTime(ParticleSystem PS, AudioSource AS)
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(PS.gameObject);
        Destroy(AS.gameObject);
    }

    private void destroyF1WallLaser()
    {

        if (eyeF1One == null && !eyeF1Explode1)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, eyeF1OneLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, eyeF1OneLoc, Quaternion.identity);
            eyeF1Explode1 = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (eyeF1Two == null && !eyeF1Explode2)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, eyeF1TwoLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, eyeF1TwoLoc, Quaternion.identity);
            eyeF1Explode2 = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (eyeF1One == null && eyeF1Two == null)
            Destroy(laserF1Wall);
    }

    private void destroyF1DoorLaser()
    {
        if (eyeF1Three == null && !eyeF1Explode3)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, eyeF1ThreeLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, eyeF1ThreeLoc, Quaternion.identity);
            eyeF1Explode3 = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (eyeF1Four == null && !eyeF1Explode4)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, eyeF1FourLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, eyeF1FourLoc, Quaternion.identity);
            eyeF1Explode4 = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (eyeF1Five == null && !eyeF1Explode5)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, eyeF1FiveLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, eyeF1FiveLoc, Quaternion.identity);
            eyeF1Explode5 = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (eyeF1Three == null && eyeF1Four == null && eyeF1Five == null)
            Destroy(laserF1Door);
    }

    private void initial2ndFloorSpawn()
    {
        if(laserF1Door == null && floor2Start)
        {
            Instantiate(floor2RoomEnemy, spawn2ndFloorLocs[0].position, Quaternion.identity);
            Instantiate(floor2RoomEnemy, spawn2ndFloorLocs[1].position, Quaternion.identity);
            floor2Start = false;
        }
    }

    private void activateWave1()
    {
        if (glassEyeOne == null && !glassEyeOneExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, glassEyeOneLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, glassEyeOneLoc, Quaternion.identity);
            glassSet.Remove(glassEyeOne);
            glassEyeOneExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (glassEyeTwo == null && !glassEyeTwoExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, glassEyeTwoLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, glassEyeTwoLoc, Quaternion.identity);
            glassSet.Remove(glassEyeTwo);
            glassEyeTwoExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (glassEyeThree == null && !glassEyeThreeExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, glassEyeThreeLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, glassEyeThreeLoc, Quaternion.identity);
            glassSet.Remove(glassEyeThree);
            glassEyeThreeExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (glassEyeFour == null && !glassEyeFourExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, glassEyeFourLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, glassEyeFourLoc, Quaternion.identity);
            glassSet.Remove(glassEyeFour);
            glassEyeFourExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (glassEyeFive == null && !glassEyeFiveExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, glassEyeFiveLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, glassEyeFiveLoc, Quaternion.identity);
            glassSet.Remove(glassEyeFive);
            glassEyeFiveExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (glassEyeSix == null && !glassEyeSixExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, glassEyeSixLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, glassEyeSixLoc, Quaternion.identity);
            glassSet.Remove(glassEyeSix);
            glassEyeSixExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (glassSet.Count == 0 && !glassSpawn)
        {
            eyeParents.Remove(glassEyeParent);
            spawnWave();
            glassSpawn = true;
        }
    }

    private void activateWave2()
    {
        if (topEyeOne == null && !topEyeOneExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, topEyeOneLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, topEyeOneLoc, Quaternion.identity);
            topSet.Remove(topEyeOne);
            topEyeOneExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (topEyeTwo == null && !topEyeTwoExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, topEyeTwoLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, topEyeTwoLoc, Quaternion.identity);
            topSet.Remove(topEyeTwo);
            topEyeTwoExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (topEyeThree == null && !topEyeThreeExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, topEyeThreeLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, topEyeThreeLoc, Quaternion.identity);
            topSet.Remove(topEyeThree);
            topEyeThreeExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (topEyeFour == null && !topEyeFourExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, topEyeFourLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, topEyeFourLoc, Quaternion.identity);
            topSet.Remove(topEyeFour);
            topEyeFourExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (topEyeFive == null && !topEyeFiveExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, topEyeFiveLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, topEyeFiveLoc, Quaternion.identity);
            topSet.Remove(topEyeFive);
            topEyeFiveExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (topEyeSix == null && !topEyeSixExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, topEyeSixLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, topEyeSixLoc, Quaternion.identity);
            topSet.Remove(topEyeSix);
            topEyeSixExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (topEyeSeven == null && !topEyeSevenExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, topEyeSevenLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, topEyeSevenLoc, Quaternion.identity);
            topSet.Remove(topEyeSeven);
            topEyeSevenExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (topEyeEight == null && !topEyeEightExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, topEyeEightLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, topEyeEightLoc, Quaternion.identity);
            topSet.Remove(topEyeEight);
            topEyeEightExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (topEyeNine == null && !topEyeNineExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, topEyeNineLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, topEyeNineLoc, Quaternion.identity);
            topSet.Remove(topEyeNine);
            topEyeNineExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (topEyeTen == null && !topEyeTenExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, topEyeTenLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, topEyeTenLoc, Quaternion.identity);
            topSet.Remove(topEyeTen);
            topEyeTenExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (topEyeEleven == null && !topEyeElevenExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, topEyeElevenLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, topEyeElevenLoc, Quaternion.identity);
            topSet.Remove(topEyeEleven);
            topEyeElevenExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (topEyeTwelve == null && !topEyeTwelveExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, topEyeTwelveLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, topEyeTwelveLoc, Quaternion.identity);
            topSet.Remove(topEyeTwelve);
            topEyeTwelveExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (topSet.Count == 0 && !topSpawn)
        {
            eyeParents.Remove(topEyeParent);
            spawnWave();
            topSpawn = true;
        }
    }

    private void activateWave3()
    {
        if (bottomEyeOne == null && !bottomEyeOneExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, bottomEyeOneLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, bottomEyeOneLoc, Quaternion.identity);
            bottomSet.Remove(bottomEyeOne);
            bottomEyeOneExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (bottomEyeTwo == null && !bottomEyeTwoExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, bottomEyeTwoLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, bottomEyeTwoLoc, Quaternion.identity);
            bottomSet.Remove(bottomEyeTwo);
            bottomEyeTwoExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (bottomEyeThree == null && !bottomEyeThreeExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, bottomEyeThreeLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, bottomEyeThreeLoc, Quaternion.identity);
            bottomSet.Remove(bottomEyeThree);
            bottomEyeThreeExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (bottomEyeFour == null && !bottomEyeFourExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, bottomEyeFourLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, bottomEyeFourLoc, Quaternion.identity);
            bottomSet.Remove(bottomEyeFour);
            bottomEyeFourExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (bottomEyeFive == null && !bottomEyeFiveExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, bottomEyeFiveLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, bottomEyeFiveLoc, Quaternion.identity);
            bottomSet.Remove(bottomEyeFive);
            bottomEyeFiveExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (bottomEyeSix == null && !bottomEyeSixExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, bottomEyeSixLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, bottomEyeSixLoc, Quaternion.identity);
            bottomSet.Remove(bottomEyeSix);
            bottomEyeSixExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (bottomEyeSeven == null && !bottomEyeSevenExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, bottomEyeSevenLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, bottomEyeSevenLoc, Quaternion.identity);
            bottomSet.Remove(bottomEyeSeven);
            bottomEyeSevenExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (bottomEyeEight == null && !bottomEyeEightExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, bottomEyeEightLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, bottomEyeEightLoc, Quaternion.identity);
            bottomSet.Remove(bottomEyeEight);
            bottomEyeEightExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (bottomEyeNine == null && !bottomEyeNineExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, bottomEyeNineLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, bottomEyeNineLoc, Quaternion.identity);
            bottomSet.Remove(bottomEyeNine);
            bottomEyeNineExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (bottomEyeTen == null && !bottomEyeTenExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, bottomEyeTenLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, bottomEyeTenLoc, Quaternion.identity);
            bottomSet.Remove(bottomEyeTen);
            bottomEyeTenExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (bottomEyeEleven == null && !bottomEyeElevenExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, bottomEyeElevenLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, bottomEyeElevenLoc, Quaternion.identity);
            bottomSet.Remove(bottomEyeEleven);
            bottomEyeElevenExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (bottomEyeTwelve == null && !bottomEyeTwelveExplode)
        {
            ParticleSystem tempPS = Instantiate(eyeExplosionEffect, bottomEyeTwelveLoc, Quaternion.identity);
            AudioSource tempAS = Instantiate(eyeExplosionSound, bottomEyeTwelveLoc, Quaternion.identity);
            bottomSet.Remove(bottomEyeTwelve);
            bottomEyeTwelveExplode = true;
            StartCoroutine(waitDestroyTime(tempPS, tempAS));
        }
        if (bottomSet.Count == 0 && !bottomSpawn)
        {
            eyeParents.Remove(bottomEyeParent);
            spawnWave();
            bottomSpawn = true;
        }
    }

    private void destroyF2Laser()
    {
        if(eyeParents.Count == 0 && F2Laser)
        {
            Destroy(laserF2);
            F2Laser = false;
        }
    }

    private void spawnWave()
    {
        StartCoroutine(Spawn());
    }
    IEnumerator Spawn()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            for (int spawnLocIter = 0; spawnLocIter < spawnLocationList.Count; ++spawnLocIter)
            {
                Instantiate(enemyToSpawn, spawnLocationList[spawnLocIter].position, spawnLocationList[spawnLocIter].rotation);
            }
            numberSpawned++;
            yield return new WaitForSeconds(spawnTimer);
        }
    }
}