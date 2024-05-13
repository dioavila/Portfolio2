using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] int HP;
    [SerializeField] int playerSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;
    [SerializeField] int sprintMod;

    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;
    [SerializeField] GameObject cube;

    Vector3 moveDir;
    Vector3 playerVel;
    int jumpCount;
    bool isShooting;
    bool isDamaged;

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
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
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
        else if(Input.GetButtonUp("Sprint"))
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
            Debug.Log(hit.transform.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if(hit.transform != transform && dmg != null)
            {
                dmg.TakeDamage(shootDamage);
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public Vector3 GetPosition()
    {
        return controller.transform.position;
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashScreenRed());
    }
    
    IEnumerator flashScreenRed()
    {
        GameManager.instance.playerFlashDamage.SetActive(true);
        yield return new WaitForSeconds(.1f);
        GameManager.instance.playerFlashDamage.SetActive(false);
    }
}