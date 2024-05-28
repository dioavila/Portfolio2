using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Progress;

public class wallRun : MonoBehaviour, IDamage
{
    [Header("General Settings")]
    [SerializeField] public CharacterController controller;
    [SerializeField] int gravity;
    [SerializeField] public int startingHP;
    [SerializeField] public int HP;
    int gravityStorage;
    
    [Header("Shooting")]
    [SerializeField] public Transform playerShootPos;
    [SerializeField] GameObject knifeModelLoc;
    [SerializeField] Transform grindKnifeModelLoc;
    [SerializeField] GameObject playerBullet;
    [SerializeField] GameObject grindBullet;
    [SerializeField] public List<GameObject> gKnifeModels = new List<GameObject>();
    public int gThrowCount;
    public int gThrowCountMax = 4; //Hardcoded because it cant be increased without changing code
    public bool resetOn = false;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;
    [SerializeField] float grindShootRate;
    [SerializeField] GameObject playerObj;
    bool isShooting;
    //Kasey Add
    [SerializeField] int shootspeed;
    [SerializeField] public List<KnifeStats> knifeList = new List<KnifeStats>();
    public int selectedKnife;
    [SerializeField] GameObject knifeModel;
    [SerializeField] int freezeTime;

    [Header("Movement")]
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int playerSpeed;
    [SerializeField] int sprintMod;
    public bool isGrinding = false;
    public Vector3 moveDir;
    int playerSpeedStorage;
    Vector3 playerVel;
    int jumpCount;
    
    //Wallrun Variables
    [Header("Wall Detection")]
    [SerializeField] int wallDist;
    [SerializeField] float charPitch;
    [SerializeField] public bool onAir = false;
    [SerializeField] bool canWallRun = true;
    Vector3 wallDirection;
    bool onWallLeft = false, onWallRight = false;

    //Bullet Time
    [Header("Bullet Time")]
    [SerializeField] public float timeDilationRate;
    [SerializeField] float bTimeTotal;
    public float bTimeCurrent;
    public bool bulletTimeActive = false;
    [SerializeField] float barFillRate;
    [SerializeField] float barEmptyRate;

    //Publics
    [Header("Char States")]
    bool canSprint = true;
    public bool playerCanMove = true;
    bool isWallRunning = false;

    [Header("Item Pickup")]
    private IPickup currentPickup;
    private bool isInRange;
    [SerializeField] public List<GameObject> keys = new List<GameObject>();

    [Header("Animation")]
    [SerializeField] Animator animR;
    [SerializeField] Animator animL;
    [SerializeField] float timerDelay;


    // Start is called before the first frame update
    void Start()
    {
        knifeModel = knifeList[0].Knife;
        Changegun();
        bTimeCurrent = bTimeTotal;
        playerSpeedStorage = playerSpeed;
        gravityStorage = gravity;
        HP = startingHP;
        updatePlayerUI();
        updateBPUI();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

        GKnifeDisplayReset();

        Selectknife();

        //Bullet Time Check
        BulletTimeCheck();

        PlayerActions();

        //Movement and WallRun Check
        MovementCheck();

        //Pick Up Logic

        if (isInRange && Input.GetKeyDown(KeyCode.F))
        {
            currentPickup.PickUpItem();
            GameManager.instance.CloseMessagePanel("");
        }


    }

    void GKnifeDisplayReset()
    {
        if (gThrowCount == 0)
        {
            for (int knifeModIter = 0; knifeModIter < gThrowCountMax; ++knifeModIter)
            {
                gKnifeModels[knifeModIter].SetActive(true);
            }
                resetOn = false;
        }
        else if (resetOn && gThrowCount > 0)
        {
            for (int knifeModIter = gThrowCount; knifeModIter < gThrowCountMax; ++knifeModIter)
            {
                gKnifeModels[knifeModIter].SetActive(true);
            }
            resetOn = false;
        }
    }

    void PlayerActions()
    {
        if (Input.GetButton("Fire1") && !isShooting && knifeList[selectedKnife])
        {
            StartCoroutine(shoot(knifeList[selectedKnife].Knife, shootRate));
        }

        if (Input.GetButtonDown("Grind Throw") && !isShooting && gThrowCount < gThrowCountMax)
        {
            if (gThrowCount >= 0 && gThrowCount <= 4)
            {

                ++gThrowCount;
                gKnifeModels[gThrowCount - 1].SetActive(false);
            }
            StartCoroutine(shoot(grindBullet, grindShootRate));
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (Time.timeScale == 1f)
            {
                Time.timeScale = timeDilationRate;
                bulletTimeActive = true;
            }
            else
            {
                Time.timeScale = 1f;
                bulletTimeActive = false;
            }
        }
    }

    void movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
            if (onAir)
            {
                onAir = false;
            }
            if (!canWallRun)
            {
                canWallRun = true;
            }
            if (!canSprint)
            {
                canSprint = true;
                playerSpeed = playerSpeedStorage;
            }
        }
        moveDir = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);
        if (playerCanMove) {
            controller.Move(moveDir * playerSpeed * Time.deltaTime);
        }
       sprint();

        //if(Input.GetButton("Fire1") && !isShooting && knifeList[selectedKnife])
        //{
        //    StartCoroutine(shoot(playerBullet, shootRate));
        //}

        //if (Input.GetButtonDown("Grind Throw") && !isShooting)
        //{
        //    StartCoroutine(shoot(grindBullet, grindShootRate));
        //}

        //if (Input.GetButtonDown("Fire2"))
        //{
        //    if (Time.timeScale == 1f)
        //    {
        //        Time.timeScale = timeDilationRate;
        //        bulletTimeActive = true;
        //    }
        //    else
        //    {
        //        Time.timeScale = 1f;
        //        bulletTimeActive = false;
        //    }
        //}

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            canSprint = false;
            ++jumpCount;
            playerVel.y = jumpSpeed;
            onAir = true;
        }

        if (!isWallRunning && !isGrinding)
        {
            playerVel.y -= gravity * Time.deltaTime;
            controller.Move(playerVel * Time.deltaTime);
        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint") && canSprint)
        {
            animR.SetFloat("Speed", Mathf.Lerp(0, 1, 1));
            animL.SetFloat("Speed", Mathf.Lerp(0, 1, 1));
            playerSpeed *= sprintMod;
        }
        else if(Input.GetButtonUp("Sprint") && canSprint)
        {
            playerSpeed = playerSpeedStorage;
            animR.SetFloat("Speed", 0);
            animL.SetFloat("Speed", 0);
        }
    }

    IEnumerator shoot(GameObject bulletType, float shootRateType)
    {
        if (bulletType.name == "Ammo - playerBulletG")
        {
            isShooting = true;
            animL.SetTrigger("ShootG");
            Instantiate(grindBullet, playerShootPos.position, Camera.main.transform.rotation);

            yield return new WaitForSeconds(shootRateType);
            isShooting = false;
        }
        else
        {
            isShooting = true;
            animR.SetTrigger("Shoot");

            IDamage dmg = knifeList[selectedKnife].Knife.gameObject.GetComponent<IDamage>();

            if (knifeList[selectedKnife].Knife != transform.CompareTag("Player") && dmg != null)
            {
                dmg.TakeDamage(shootDamage);
            }

            yield return new WaitForSeconds(shootRate);
            isShooting = false;
            knifeModelLoc.SetActive(true);
        }
    }

    public void CreateBulletG()
    {
        Instantiate(grindBullet, playerShootPos.position, Camera.main.transform.rotation);
    }

    public void CreateBullet()
    {
        Instantiate(knifeList[selectedKnife].Knife, playerShootPos.position, Camera.main.transform.rotation);
    }

    void Selectknife()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedKnife < knifeList.Count - 1)
        {
            selectedKnife++;
            Changegun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedKnife > 0)
        {
            selectedKnife--;
            Changegun();
        }
    }

    void Changegun()
    {
        shootDamage = knifeList[selectedKnife].Damage;
        shootspeed = knifeList[selectedKnife].speed;
        freezeTime = knifeList[selectedKnife].freeze;

        knifeModelLoc.GetComponent<MeshFilter>().sharedMesh = knifeList[selectedKnife].Knife.GetComponentInChildren<MeshFilter>().sharedMesh;
        knifeModelLoc.GetComponent<MeshRenderer>().sharedMaterial = knifeList[selectedKnife].Knife.GetComponentInChildren<MeshRenderer>().sharedMaterial;
    }

    /// <summary>
    /// Wall Run Logic
    /// </summary>
    void MovementCheck()
    {
        if (onWallRight || onWallLeft)
        {
            if (onWallLeft) { WallRun(0); }
            else if (onWallRight) { WallRun(1); }
        }
        if (!isWallRunning)
        {
            movement();
        }
    }

    void OnControllerColliderHit(ControllerColliderHit colHit)
    {
        if (colHit.collider.gameObject.tag == "enableWallRun" && onAir && canWallRun)
        {
            RaycastHit wallDetect;
            Vector3 rightRayShoot = Camera.main.transform.forward + Camera.main.transform.right * 2;
            Vector3 leftRayShoot = Camera.main.transform.forward + (-Camera.main.transform.right * 2);

            if (Physics.Raycast(Camera.main.transform.position, rightRayShoot, out wallDetect, wallDist))
            {
                onWallRight = true;
                wallDirection = Vector3.Cross(wallDetect.transform.up, wallDetect.transform.forward).normalized;
            }
            else if (Physics.Raycast(Camera.main.transform.position, leftRayShoot, out wallDetect, wallDist))
            {
                onWallLeft = true;
                wallDirection = Vector3.Cross(wallDetect.transform.up, wallDetect.transform.forward).normalized;
            }
            if ((playerObj.transform.forward - wallDirection).magnitude > (playerObj.transform.forward - -wallDirection).magnitude)
            {
                wallDirection = -wallDirection;
            }
        }
    }

    void WallRun(int wallRunSide)
    {
        Vector3 wallTouchChecker;

        if(jumpCount == 2)
        {
            jumpCount = 1;
        }
        if (wallRunSide == 1)
        {
            wallTouchChecker = playerObj.transform.right * 10;
        }
        else
        {
            wallTouchChecker = (-playerObj.transform.right * 10);
        }

        RaycastHit wallTouch;
        bool TouchCheck = Physics.Raycast(playerObj.transform.position, wallTouchChecker, out wallTouch, 1);
        
        controller.Move(wallDirection * (playerSpeed *sprintMod) * Time.deltaTime);
        isWallRunning = true;

        if(playerCanMove)
        {
            canSprint = false;
            playerCanMove = false;
        }

        if (controller.collisionFlags != CollisionFlags.Sides && !TouchCheck || Input.GetButtonDown("Jump") || !transform.hasChanged)
        {
            ValuesReset();
        }

        void ValuesReset()
        {
            gravity = gravityStorage;
            onWallRight = false;
            onWallLeft = false;
            canWallRun = true;
            playerCanMove = true;
            isWallRunning = false;
        }       
    }

    /// <summary>
    /// Pick Up Logic
    /// </summary>
    /// 
    private void OnTriggerEnter(Collider other)
    {
        IPickup pickup = other.GetComponent<IPickup>();
        if (pickup != null)
        {
            isInRange = true;
            GameManager.instance.OpenMessagePanel("");
            currentPickup = pickup;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        isInRange = false;
        GameManager.instance.CloseMessagePanel("");
        currentPickup = null;
    }


    /// <summary>
    /// Damage Logic
    /// </summary>
    public void TakeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();
        StartCoroutine(flashScreenRed());
        if (HP <= 0)
        {
            GameManager.instance.youLose();
        }
    }
    IEnumerator flashScreenRed()
    {
        GameManager.instance.playerFlashDamage.SetActive(true);
        yield return new WaitForSeconds(.1f);
        GameManager.instance.playerFlashDamage.SetActive(false);
    }

    public void updatePlayerUI()
    {
        GameManager.instance.playerHPBar.fillAmount = (float)HP / startingHP;
    }

    /// <summary>
    /// Bullet Time Logic
    /// </summary>
    void BulletTimeCheck()
    {
        if (bulletTimeActive)
        {
            if (bTimeCurrent > 0)
            {
                GameManager.instance.playerBPUI.SetActive(true);
                BulletTimeActive();
            }
            else
            {
                Time.timeScale = 1f;
                bulletTimeActive = false;
            }
        }
        else
        {
            GameManager.instance.playerBPUI.SetActive(false);
            if (bTimeCurrent < bTimeTotal)
            {
                BulletTimeRefill();
            }
        }
    }

    void BulletTimeActive()
    {
        bTimeCurrent -= Time.deltaTime* barEmptyRate;
        updateBPUI();
    }
    void BulletTimeRefill()
    {
        bTimeCurrent += Time.deltaTime * barFillRate;
        updateBPUI();
    }

    void updateBPUI()
    {
        GameManager.instance.playerBTBar.fillAmount = bTimeCurrent / bTimeTotal;
    }

}