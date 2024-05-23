using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Progress;

public class KwallRun : MonoBehaviour, IDamage
{
    [Header("General Settings")]
    [SerializeField] GameObject playerBullet;
    [SerializeField] GameObject playerIceBullet;
    [SerializeField] int FreezeTime;
    [SerializeField] Transform playerShootPos;
    [SerializeField] CharacterController controller;
    [SerializeField] int playerSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;
    [SerializeField] int sprintMod;
    [SerializeField] int startingHP;
    [SerializeField] int HP;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;
    [SerializeField] GameObject playerObj;
    int gravityStorage;
    int playerSpeedStorage;

    public Vector3 moveDir;
    Vector3 playerVel;
    int jumpCount;
    bool isShooting;
    bool playerBulletIsEquipped;
    bool playerIceBulletIsEquipped;
    
    //Wallrun Variables
    [Header("Wall Detection")]
    [SerializeField] int wallDist;
    [SerializeField] float charPitch;
    [SerializeField] bool onAir = false;
    [SerializeField] bool canWallRun = true;
    Vector3 wallDirection;
    bool onWallLeft = false, onWallRight = false;

    //Bullet Time
    [Header("Bullet Time")]
    [SerializeField] float timeDilationRate;
    [SerializeField] float bTimeTotal;
    public float bTimeCurrent;
    bool bulletTimeActive = false;
    [SerializeField] float barFillRate;
    [SerializeField] float barEmptyRate;

    //Publics
    [Header("Char States")]
    bool canSprint = true;
    bool playerCanMove = true;
    bool isWallRunning = false;

    [Header("Item Pickup")]
    public Item[] item = new Item[3];
    private bool isInRange;
    public GameObject messagePanel;
    string itemTag;

    // Start is called before the first frame update
    void Start()
    {
        bTimeCurrent = bTimeTotal;
        playerSpeedStorage = playerSpeed;
        gravityStorage = gravity;
        HP = startingHP;
        updatePlayerUI();
        updateBPUI();
        playerBulletIsEquipped = true;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

        //Bullet Time Check
        BulletTimeCheck();

        //Movement and WallRun Check
        MovementCheck();

        //Pick Up Logic
        if (isInRange && Input.GetKeyDown(KeyCode.F))
        {
            for(int i = 0; i < item.Length; i++) 
            {
                if (item[i] != null && item[i].tag == itemTag)
                {
                    item[i].PickUpItem();
                    HP = startingHP;
                    updatePlayerUI();
                    CloseMessagePanel("");
                    break;
                }
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

        //weapon swap logic
        if(Input.GetButtonDown("Primary Weapon")) 
        {
            playerBulletIsEquipped = true;
            playerIceBulletIsEquipped = false;
        }
        if(Input.GetButtonDown("Second Weapon"))
        {
            playerBulletIsEquipped = false;
            playerIceBulletIsEquipped = true;
        }

        if(Input.GetButton("Fire1") && !isShooting && playerBulletIsEquipped)
        {
            StartCoroutine(shoot());
        }
        if(Input.GetButton("Fire1") && !isShooting && playerIceBulletIsEquipped)
        {
            StartCoroutine(ShootIceBullet());
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

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            canSprint = false;
            ++jumpCount;
            playerVel.y = jumpSpeed;
            onAir = true;
        }

        if (!isWallRunning)
        {
            playerVel.y -= gravity * Time.deltaTime;
            controller.Move(playerVel * Time.deltaTime);
        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint") && canSprint)
        {
            playerSpeed *= sprintMod;
        }
        else if(Input.GetButtonUp("Sprint") && canSprint)
        {
            playerSpeed = playerSpeedStorage;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(playerBullet, playerShootPos.position, Camera.main.transform.rotation);

        IDamage dmg = playerBullet.gameObject.GetComponent<IDamage>();

        if (playerBullet.transform != transform.CompareTag("Player")&& dmg != null)
        {
            dmg.TakeDamage(shootDamage);
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    //shoot ice bullet
    IEnumerator ShootIceBullet()
    {
        isShooting = true;
        Instantiate(playerIceBullet, playerShootPos.position, Camera.main.transform.rotation);
        IFreeze dmg = playerIceBullet.gameObject.GetComponent<IFreeze>();
        if (playerIceBullet.transform != transform.CompareTag("Player") && dmg != null)
        {
            dmg.FreezeTime(FreezeTime);
        }
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
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
    public void OpenMessagePanel(string text)
    {
        messagePanel.SetActive(true);
    }

    public void CloseMessagePanel(string text)
    {
        messagePanel.SetActive(false);
    }

    //triggers the ability to pickup
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Health Pickup1") || other.gameObject.CompareTag("Health Pickup2"))
        {
            itemTag = other.gameObject.tag;
            isInRange = true;
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                OpenMessagePanel("");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isInRange = false;
        CloseMessagePanel("");
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

    void updatePlayerUI()
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