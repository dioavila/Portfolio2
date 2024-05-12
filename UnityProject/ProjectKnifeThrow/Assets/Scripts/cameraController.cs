using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;

    float rotX;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Get Input
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;

        if (invertY)
        {
            rotX += mouseY;
        }
        else
        {
            rotX -= mouseY;
        }

        //Clamp the rotX on the X-axis
        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        //Rotate the cam on the x-axis
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        //Rotate the player on the Y-axis
     //   if (!GameManager.instance.playerScript.isWallRunning)
        {
            transform.parent.Rotate(Vector3.up * mouseX);
        }
     //   else
     //   {
    //        transform.localRotation = Quaternion.Euler(mouseX, 0, 0);
    //    }


    }
}
