using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraController : MonoBehaviour
{
    [SerializeField] int lockVertMin = -45, lockVertMax = 45;
    [SerializeField] bool invertY = false;
    [SerializeField] int sensitivityComp;
    [SerializeField] float sliderValue;

    float rotX;

    void Start()
    {
        GameManager.instance.LoadSettings();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleCameraRotation();
        invertY = GameManager.instance.isInverted;
    }

    public void HandleCameraRotation()
    {
        // Get Input
        float sensitivity = GameManager.instance.sensitivity * 1000f;
        invertY = GameManager.instance.isInverted;
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

