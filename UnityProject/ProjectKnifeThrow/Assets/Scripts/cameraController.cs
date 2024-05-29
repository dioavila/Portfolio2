using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;
    [SerializeField] int sensitivityComp;
    int sensitivityOrig;

    float rotX;

    // Start is called before the first frame update
    void Start()
    {
        sensitivityOrig = sensitivity;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Get Input
        //Adjusting dpi for bullettime
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

        //Clamp the rotX on the X-axis
        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        //Rotate the cam on the x-axis
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        //Rotate the player on the Y-axis

        transform.parent.Rotate(Vector3.up * mouseX);
   }
}
