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

    Vector3 moveDir;
    Vector3 playerVel;
    int jumpCount;
    bool isShooting;

    //Wallrun Variables
    float timerStorage;
    int gravityStorage;
    int playerSpeedStorage;
    bool canSprint = true;
    bool onWallLeft = false, onWallRight = false;
    Vector3 wallDirection;
    [SerializeField] int wallDist;
    [SerializeField] float charPitch;
    [SerializeField] bool onAir = false;
    [SerializeField] bool canWallRun = true;
    [SerializeField] float runTimer;

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
        movement();
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
            playerSpeed /= sprintMod;
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
            // CollisionFlags.
            //Identify where it hit
            RaycastHit wallDetect;
            Vector3 rightRayShoot = Camera.main.transform.forward + Camera.main.transform.right * 2;
            Vector3 leftRayShoot = Camera.main.transform.forward + (-Camera.main.transform.right * 2);

            //Pitch the character accordingly
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
        //Reset jumps
        if(jumpCount == 2)
        {
            jumpCount = 1;
        }
        if (wallRunSide == 1)
        {
            wallTouchChecker = playerObj.transform.right * 1;
        }
        else
        {
            wallTouchChecker = (-playerObj.transform.right * 1);
        }
        
        playerObj.transform.rotation = Quaternion.Lerp(Quaternion.identity, targetRot, 2);
        controller.Move(wallDirection * playerSpeed * Time.deltaTime);

        RaycastHit wallTouch;
        bool TouchCheck = Physics.Raycast(playerObj.transform.position, wallTouchChecker, out wallTouch, 1);

        //if (Input.GetButtonDown("Jump"))
        //{
        //    Vector3 planarDir = Camera.main.transform.forward + -wallTouchChecker;
        //    Vector3 dirResult = planarDir + playerObj.transform.up;
        //    isWallRunning = false;
        //    Debug.DrawRay(Camera.main.transform.position, dirResult * 10, Color.blue);
        //    controller.Move(dirResult * playerSpeed*5 * Time.deltaTime);
        //}

        if (!TouchCheck || Input.GetButtonDown("Jump"))
        {
            playerSpeed = playerSpeedStorage;
            controller.Move(moveDir * (playerSpeed+1) * Time.deltaTime);
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