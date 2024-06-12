using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] int sensitivity = 100; 
    [SerializeField] int lockVertMin = -45, lockVertMax = 45;
    [SerializeField] bool invertY = false;
    [SerializeField] int sensitivityComp = 2;
    int sensitivityOrig;

    float rotX;

    void Start()
    {
        sensitivityOrig = sensitivity;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!GameManager.instance.playerScript.isDead)
        {
            HandleCameraRotation();
        }
    }

    public void HandleCameraRotation()
    {
        // Get Input
        float mouseY, mouseX;
        mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
        //if (GameManager.instance.playerScript.bulletTimeActive)
        //{
        //    mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity * sensitivityComp;
        //    mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity * sensitivityComp;
        //}
        //else
        //{
        //    mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        //    mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
        //}
        
        if (invertY)
        {
            rotX += mouseY;
        }
        else
        {
            rotX -= mouseY;
        }

        // Clamp the rotation on the X-axis
        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        // Rotate the camera on the X-axis
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        // Rotate the player on the Y-axis
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}

