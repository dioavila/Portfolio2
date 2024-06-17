using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Progress;

public class wallRun : MonoBehaviour, IDamage, IPushback
{
    [Header("General Settings")]
    [SerializeField] public CharacterController controller;
    [SerializeField] int gravity;
    [SerializeField] public int startingHP;
    [SerializeField] public int HP;
    [SerializeField] int Force;
    int gravityStorage;
    public bool isDead = false;
    Rigidbody rb;
    
    [Header("Shooting")]
    [SerializeField] public Transform playerShootPos;
    [SerializeField] Transform knifeModelLoc;
    [SerializeField] GameObject playerBullet;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;
    [SerializeField] GameObject playerObj;
    
    [SerializeField] Transform playerShootPosG;
    [SerializeField] GameObject grindBullet;
    [SerializeField] Transform grindKnifeModelLoc;
    [SerializeField] List<GameObject> gKnifeModels = new List<GameObject>();
    public int AmmoCount;
    public int gThrowCount;
    public bool resetOn = false;
    public bool recoverOn = false;
    public int gThrowCountMax = 4; //Hardcoded because it cant be increased without changing code
    [SerializeField] float grindShootRate;
    
    bool isShooting;
    //Kasey Add
    [SerializeField] int shootspeed;
    [SerializeField] public List<KnifeStats> knifeList = new List<KnifeStats>();
    public int selectedKnife;
    [SerializeField] GameObject knifeModel;
    [SerializeField] int freezeTime;
    [SerializeField] public int UpWardForce;

    [Header("Movement")]
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] public int playerSpeed;
    [SerializeField] int sprintMod;
    public bool isGrinding = false;
    public Vector3 moveDir;
    int playerSpeedStorage;
    Vector3 playerVel;
    int jumpCount;
    Vector3 PushBack;

    //sliding 
    [Header("Sliding")]
    [SerializeField] float slideSpeed = 10f;
    [SerializeField] float slideDuration = 1f;
    PlayerFovController fovController;

    [SerializeField] int slideCharges = 2;
    [SerializeField] float slideCooldown = 3f;
    private int currentSlideCharges;
    private float slideCoolDownTimer;
    public bool isSliding = false;
    Vector3 slideDirection;

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
    [SerializeField] public float attackDecreaseAmmt = 0.5f;
    public float attackDecreaseCurr = 0;
    public float bTimeCurrent;
    public bool bulletTimeActive = false;
    [SerializeField] float barFillRate;
    [SerializeField] float barEmptyRate;

    //Publics
    [Header("Char States")]
    bool canSprint = true;
    public bool playerCanMove = true;
    public bool isWallRunning = false;
    public bool isClimbing = false;

    [Header("Item Pickup")]
    private IPickup currentPickup;
    private bool isInRange;
    [SerializeField] public List<GameObject> keys = new List<GameObject>();

    [Header("Animation")]
    [SerializeField] public Animator anim;
    [SerializeField] int smoothAnimMod;

    // Start is called before the first frame update
    void Start()
    {
       // knifeModel = knifeList[0].Knife;
        Changegun();
        bTimeCurrent = bTimeTotal;
        playerSpeedStorage = playerSpeed;
        gravityStorage = gravity;
        HP = startingHP;
        spawnPlayer();
        updateBPUI();

        fovController = GetComponent<PlayerFovController>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (!GameManager.instance.isPaused)
            {
            
               // Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
                if(gThrowCount <0)
                {
                    gThrowCount = 0;
                }

                Selectknife();

                //Bullet Time Check
                BulletTimeCheck();

                PlayerActions();

                if (!isClimbing)
                {
                    MovementCheck();
                }

                //Pick Up Logic
                if (isInRange && Input.GetKeyDown(KeyCode.F))
                {
                    currentPickup.PickUpItem();
                    GameManager.instance.CloseMessagePanel("");
                }
            }

            // Slide cooldown logic
            if (currentSlideCharges < slideCharges)
            {
                slideCoolDownTimer += Time.deltaTime;
                if (slideCoolDownTimer >= slideCooldown)
                {
                    currentSlideCharges++;
                    slideCoolDownTimer = 0f;
                }
            }

            if (Input.GetKeyDown(KeyCode.C) && !isSliding && currentSlideCharges > 0)
            {
                StartCoroutine(Slide());
                currentSlideCharges--;
            }

            if (Input.GetKeyDown(KeyCode.R) && !isGrinding && gThrowCount > 0)
            {
                anim.SetTrigger("ResetG");//Fix Bug where player stop in place if reload takes place right before pressing grind button
            }
        }
        GameManager.instance.updateKnifeCount(knifeList[2].CurrentKinfeCount);
    }

    public void GKnifeDisplayReset()
    {
        for (int knifeModIter =  0; knifeModIter < gThrowCountMax; knifeModIter++)
        {
            gKnifeModels[knifeModIter].SetActive(true);
        }
        gThrowCount = 0;
    }

    void PlayerActions()
    {
        if (Input.GetButton("Fire1") && !isShooting && knifeList[selectedKnife])
        {
            StartCoroutine(shoot(knifeList[selectedKnife].Knife, shootRate));
        }

        if (Input.GetButtonDown("Grind Throw") && !isShooting && gThrowCount < gThrowCountMax)
        {
            if(gThrowCount >= 0 && gThrowCount < 4)
            {
                ++gThrowCount;
                gKnifeModels[gThrowCount-1].SetActive(false);
                StartCoroutine(shoot(grindBullet, grindShootRate)); // shoot needs to be withing the 0 to 4 constraint
            }
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
            //anim.SetBool("Jump1Bool", false);
            //anim.SetBool("Jump2Bool", false);
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
                //playerSpeed = playerSpeedStorage;
            }
        }

        if (isSliding)
        {
            //skip other movement if sliding
            return;
        }

        moveDir = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);
        if (playerCanMove) {
            controller.Move(moveDir * playerSpeed * Time.deltaTime);
        }
       sprint();

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            //anim.SetTrigger("JumpAnim");
            canSprint = false;
            if(jumpCount < 1)
            {
                anim.SetTrigger("JumpAnim");
            }
            else 
            {
                //anim.SetBool("JumpBool1", false);
                anim.SetTrigger("JumpAnim2");
            }
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
        if (Input.GetButtonDown("Sprint") && canSprint && moveDir != Vector3.zero)
        {
            anim.SetFloat("Speed", Mathf.Lerp(0, 1, 1));
            playerSpeed *= sprintMod;
        }
        else if(Input.GetButtonUp("Sprint") || moveDir == Vector3.zero)
        {
            anim.SetFloat("Speed", Mathf.Lerp(1, 0, 1));
            playerSpeed = playerSpeedStorage;
        }
    }

    IEnumerator shoot(GameObject bulletType, float shootRateType)
    {
        if (bulletType.name == "Ammo - playerBulletG")
        {
            isShooting = true;
            if (anim.GetCurrentAnimatorStateInfo(2).IsName("KnifeOpenG 1") && anim.GetCurrentAnimatorStateInfo(2).normalizedTime < 1.0f ||
                anim.GetCurrentAnimatorStateInfo(2).IsName("KnifeCloseG") && anim.GetCurrentAnimatorStateInfo(2).normalizedTime < 1.0f)
            {
                //anim.SetBool("Shoot2G",true);
                //anim.SetBool("Shoot2G", false);
                anim.SetTrigger("ShootG2");
            }
            else
            {
                anim.SetTrigger("ShootG");
            }
            GameObject Projectile = Instantiate(bulletType, playerShootPosG.position, Camera.main.transform.rotation);
            Rigidbody ProjectileRB = Projectile.GetComponent<Rigidbody>();
            Vector3 ForceDir = Camera.main.transform.forward;
            GameManager.instance.audioScript.PlayThrowG();

            RaycastHit hit;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 500f))
            {
                ForceDir = (hit.point - playerShootPosG.position).normalized;
            }

            Vector3 forcetoadd = ForceDir * Projectile.GetComponent<GrindBullet>().Speed + Projectile.transform.transform.up * UpWardForce;

            ProjectileRB.AddForce(forcetoadd, ForceMode.Impulse);
            yield return new WaitForSeconds(shootRateType);
            isShooting = false;
        }
        else if (bulletType.name == "Standard Knife")
        {
            isShooting = true;
            anim.SetTrigger("Shoot");
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
            knifeModelLoc.gameObject.SetActive(true);
        }
        else if (knifeList[selectedKnife].CurrentKinfeCount > 0)
        {
            isShooting = true;
            anim.SetTrigger("Shoot");
            knifeList[selectedKnife].CurrentKinfeCount--;
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
            knifeModelLoc.gameObject.SetActive(true);
        }
    }

    public void GetKnifeStats(KnifeStats _Knife)
    {
        knifeList.Add(_Knife);
        selectedKnife = knifeList.Count - 1;

        shootDamage = _Knife.Damage;
        shootspeed = _Knife.speed;
        freezeTime = _Knife.freeze;
        AmmoCount = _Knife.CurrentKinfeCount;

        knifeModelLoc.GetComponent<MeshFilter>().sharedMesh = knifeList[selectedKnife].Knife.GetComponentInChildren<MeshFilter>().sharedMesh;
        knifeModelLoc.GetComponent<MeshRenderer>().sharedMaterial = knifeList[selectedKnife].Knife.GetComponentInChildren<MeshRenderer>().sharedMaterial;

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
        AmmoCount = knifeList[selectedKnife].CurrentKinfeCount;

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
        bool TouchCheck = Physics.Raycast(playerObj.transform.position, (wallTouchChecker + -playerObj.transform.forward), out wallTouch, 3);
        
        controller.Move(wallDirection * (playerSpeed *sprintMod) * Time.deltaTime);
        isWallRunning = true;

        if(playerCanMove)
        {
            //anim.SetBool("Jump1Bool", false);
            //anim.SetBool("Jump2Bool", false);
            canSprint = false;
            playerCanMove = false;
        }

        if (!TouchCheck || Input.GetButtonDown("Jump") || !transform.hasChanged || controller.collisionFlags == CollisionFlags.Sides)
        {
            //anim.SetBool("Jump1Bool", true);
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
        if(other.name == "Enemy - Destroyer (DB) v2.0.0 1")
        {
           //  Pushback((transform.position - other.transform.position) * Force);
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
        GameManager.instance.audioScript.PlayDamage();
        if (HP <= 0)
        {
            isDead = true;
            GameManager.instance.TriggerRedToBlackScreen();
            anim.SetTrigger("isDead");
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
        bTimeCurrent -= (Time.deltaTime* barEmptyRate) + attackDecreaseCurr;
        if (attackDecreaseCurr != 0)
            attackDecreaseCurr = 0;
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

    public void spawnPlayer()
    {
        HP = startingHP;
        updatePlayerUI();

        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    IEnumerator Slide()
    {
        isSliding = true;
        // Determine the slide direction
        if (moveDir == Vector3.zero)
        {
            slideDirection = transform.forward;
        }
        else
        {
            slideDirection = moveDir.normalized;
        }

        // Apply appropriate camera effect based on the direction
        if (Vector3.Dot(slideDirection, transform.forward) > 0.5f)
        {
            // Forward dash
            fovController.IncreaseFovForSlide();
        }
        else if (Vector3.Dot(slideDirection, -transform.forward) > 0.5f)
        {
            // Backward dash
            fovController.DecreaseFovForBackwardSlide();
        }
        else if (Vector3.Dot(slideDirection, -transform.right) > 0.5f)
        {
            // Left dash
            fovController.TiltCameraLeft();
        }
        else if (Vector3.Dot(slideDirection, transform.right) > 0.5f)
        {
            // Right dash
            fovController.TiltCameraRight();
        }
        GameManager.instance.audioScript.PlayDash();
        float elapsedTime = 0f;
        while (elapsedTime < slideDuration)
        {
            Vector3 slideVelocity = slideDirection * slideSpeed;
            slideVelocity.y -= gravity * Time.deltaTime;
            controller.Move(slideVelocity * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isSliding = false;
        fovController.ResetFov();
    }

    public void Pushback(Vector3 dir)
    {
        PushBack = new Vector3(dir.x, 0, dir.z);
        rb.velocity = PushBack * Force;
        transform.position = Vector3.Slerp(transform.position, PushBack, Time.deltaTime);
    }
}