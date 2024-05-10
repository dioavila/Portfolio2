using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class editingPlayerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] int playerBaseSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;
    [SerializeField] int sprintMod;
    [SerializeField] int HP;

    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;
    [SerializeField] GameObject cube;

    Vector3 moveDir;
    Vector3 playerVel;
    int jumpCount;
    bool isShooting;
    bool wasSprinting = false;
    [SerializeField]int currSpeed; //this is serialized for now just so I can see the players current speed change while testing

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //ground check needed to not sprint in mid air !!make sure layer mask for player is set to whatIsGround AND under "Ground Check" - player height is 2 and whatIsGround is selected!!
        //Every platform that the player can walk on has to have the layer set to whatIsGround for sprint to work
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        Debug.Log("Grounded: " + grounded);

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

        movement();
    }

    void movement()
    {
        // moveDir = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
        //transform.position += moveDir * playerSpeed * Time.deltaTime; Does not account for collision
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }
        moveDir = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);
        controller.Move(moveDir * currSpeed * Time.deltaTime);


        if (Input.GetButton("Fire1") && !isShooting)
        {
            StartCoroutine(shoot());
        }

        sprint();
        //if the sprint button is not being pressed and the player gets back on the ground then the player goes back to normal speed because the player maintains sprint speed mid air whether they let go of sprint or not bc of momentum
        if (!Input.GetButton("Sprint") && grounded)
        {
            stopSprinting();
        }

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            ++jumpCount;
            playerVel.y = jumpSpeed;

        }

        playerVel.y -= gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);
    }

    //methods brings player back to walking speed 
    void stopSprinting()
    {
        currSpeed = playerBaseSpeed;
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            if (grounded)
            {
                currSpeed = playerBaseSpeed * sprintMod; // Start sprinting if grounded
                wasSprinting = true; // Set flag to true when sprint button is pressed and grounded
            }
            else
            {
                if (wasSprinting) // Maintain sprint speed in mid-air if flag is true
                {
                    currSpeed = playerBaseSpeed * sprintMod;
                }
            }
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            if (grounded)
            {
                currSpeed = playerBaseSpeed; // Stop sprinting when grounded and sprint button released
                wasSprinting = false; // Reset flag when sprint button is released and grounded
            }
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist))
        {
            //Debug.Log(hit.transform.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && dmg != null)
            {
                dmg.TakeDamage(shootDamage);
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    //Added HP to the actual player to be able to take dmg
    public void TakeDamage(int amount)
    {
        HP -= amount;
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}