using System.Collections;
using System.Collections.Generic;
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

    Vector3 moveDir;
    Vector3 playerVel;
    int jumpCount;
    bool isShooting;
    //Wallrun Variables
    [SerializeField] int wallDist;
    [SerializeField] float charPitch;
    [SerializeField] bool onAir = false;
    [SerializeField] bool canWallRun = true;
    bool onWallLeft = false, onWallRight = false;
    [SerializeField] float runTimer;
    Vector3 wallDirection;
    float timerStorage;
    int gravityStorage;
    int playerSpeedStorage;
    bool canSprint = true;

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
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
        if(onWallRight || onWallLeft)
        {
            if(onWallLeft) { WallRun(0); }
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
        controller.Move(moveDir * playerSpeed * Time.deltaTime);

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

        playerVel.y -= gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);
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
        if (colHit.collider.gameObject.name == "wallrunWall" && onAir && canWallRun)
        {
            //Identify where it hit
            RaycastHit wallDetect;
            Vector3 rightRayShoot = Camera.main.transform.forward + Camera.main.transform.right;
            Vector3 leftRayShoot = Camera.main.transform.forward + (-Camera.main.transform.right);

            Debug.DrawRay(Camera.main.transform.position, rightRayShoot * wallDist, Color.blue);
            Debug.DrawRay(Camera.main.transform.position, leftRayShoot * wallDist, Color.green);


            //Pitch the character accordingly
            if (Physics.Raycast(Camera.main.transform.position, rightRayShoot, out wallDetect, wallDist))
            {
                //Vector3 wallNormal = wallDetect.normal;
                wallDirection = Vector3.Cross(wallDetect.transform.up, wallDetect.transform.forward).normalized;
                onWallRight = true;
                canSprint = false;

            }
            else if (Physics.Raycast(Camera.main.transform.position, leftRayShoot, out wallDetect, wallDist))
            {
                wallDirection = Vector3.Cross(wallDetect.transform.up, wallDetect.transform.forward).normalized;
                onWallLeft = true;
                canSprint = false;
            }

        }
    }

    void WallRun(int wallRunSide)
    {
        Vector3 temp = playerObj.transform.localEulerAngles;
        

        if (wallRunSide == 0)
        {
            playerObj.transform.localRotation = Quaternion.Euler(temp.x, temp.y, -charPitch);
            controller.Move(wallDirection * playerSpeed * Time.deltaTime);
        }
        else if(wallRunSide == 1)
        {
            playerObj.transform.localRotation = Quaternion.Euler(temp.x, temp.y, charPitch);
            controller.Move((-wallDirection) * playerSpeed * Time.deltaTime);
        }

        if (runTimer > 0)
        {
            gravity = 10;
            runTimer -= Time.deltaTime;
        }
        else
        {
            playerSpeed = playerSpeedStorage;
            controller.Move(moveDir * playerSpeed * Time.deltaTime);
            gravity = gravityStorage;
            runTimer = timerStorage;
            onWallRight = false;
            onWallLeft = false;
            canWallRun = false;
            playerObj.transform.localRotation = Quaternion.Euler(0, temp.y, 0);
        }

    }

}