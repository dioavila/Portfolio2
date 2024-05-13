using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class wallRun : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] int playerSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;
    [SerializeField] int sprintMod;

    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;
    [SerializeField] GameObject playerObj;
    [SerializeField] GameObject playerRef;

    public Vector3 moveDir;
    Vector3 playerVel;
    int jumpCount;
    bool isShooting;

    //Wallrun Variables
    [SerializeField] int wallDist;
    [SerializeField] float charPitch;
    [SerializeField] bool onAir = false;
    [SerializeField] bool canWallRun = true;
    [SerializeField] float runTimer;
    [SerializeField] bool flagCheck;
    //Privates
    float timerStorage;
    int gravityStorage;
    int playerSpeedStorage;
    bool canSprint = true;
    bool onWallLeft = false, onWallRight = false;
    Vector3 wallDirection;
    //Publics
    public bool playerCanMove = true;
    public bool isWallRunning = false;
    Quaternion targetRot;
    // Start is called before the first frame update
    void Start()
    {
        playerSpeedStorage = playerSpeed;
        timerStorage = runTimer;
        gravityStorage = gravity;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 test = playerObj.transform.right * 25;
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

        if (onWallRight || onWallLeft)
        {
            if (onWallLeft) { WallRun(0); }
            else if(onWallRight) { WallRun(1); }
        }
        if (!isWallRunning) { 
        
        movement();
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
            }
        }
        moveDir = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);
        if (playerCanMove) {
            controller.Move(moveDir * playerSpeed * Time.deltaTime);
        }
       sprint();

        if(Input.GetButton("Fire1") && !isShooting)
        {
            StartCoroutine(shoot());
        }

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
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

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if(hit.transform != transform && dmg != null)
            {
                dmg.TakeDamage(shootDamage);
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
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
                wallDirection = Vector3.Cross(wallDetect.transform.up, wallDetect.transform.forward).normalized;
                if ((playerObj.transform.forward - wallDirection).magnitude > (playerObj.transform.forward - -wallDirection).magnitude)
                {
                    wallDirection = -wallDirection;
                }
                targetRot = Quaternion.LookRotation(wallDirection, wallDetect.transform.up);
                onWallRight = true;
            }
            else if (Physics.Raycast(Camera.main.transform.position, leftRayShoot, out wallDetect, wallDist))
            {
                wallDirection = Vector3.Cross(wallDetect.transform.up, wallDetect.transform.forward).normalized;
                if ((playerObj.transform.forward - wallDirection).magnitude > (playerObj.transform.forward - -wallDirection).magnitude)
                {
                    wallDirection = -wallDirection;
                }
                targetRot = Quaternion.LookRotation(wallDirection, wallDetect.transform.up);
                onWallLeft = true;
            }

            isWallRunning = true;
            canSprint = false;
            playerCanMove = false;
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

        flagCheck = controller.collisionFlags == CollisionFlags.Sides;

        
         controller.Move(wallDirection * (playerSpeed *sprintMod) * Time.deltaTime);
        

        if (!flagCheck && !TouchCheck || Input.GetButtonDown("Jump") || !transform.hasChanged)
        {
            ValuesReset();
        }

        void ValuesReset()
        {
            playerSpeed = playerSpeedStorage;
            gravity = gravityStorage;
            runTimer = timerStorage;
            onWallRight = false;
            onWallLeft = false;
            canWallRun = true;
            playerCanMove = true;
            isWallRunning = false;
        }
    }

}