using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] int playerSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;
    [SerializeField] int sprintMod;
    [SerializeField] Transform ShootPOS;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;
    [SerializeField] GameObject Bullet;

    Vector3 moveDir;
    Vector3 playerVel;
    int jumpCount;
    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
        controller.Move(moveDir * playerSpeed * Time.deltaTime);

        sprint();

        if (Input.GetButton("Fire1") && !isShooting)
        {
            StartCoroutine(shoot());
        }

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            ++jumpCount;
            playerVel.y = jumpSpeed;

        }

        playerVel.y -= gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            playerSpeed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            playerSpeed /= sprintMod;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(Bullet, moveDir += ShootPOS.position, Camera.main.transform.rotation);

        IDamage dmg = Bullet.gameObject.GetComponent<IDamage>();

        if (Bullet.transform != transform && dmg != null)
        {
            dmg.TakeDamage(shootDamage);
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
