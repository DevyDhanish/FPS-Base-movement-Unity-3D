using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Senstivity")]
    public float MouseSenstivity;
    [Range(0,100f)]
    public float MouseSenstivityMultiplier;
    public bool MouseInverted = false; 

    [Header("Required Objects")]
    public Transform PlayerCamera, Orientation;
    public Rigidbody Player;
    private float xRotation;
    private float yRotation;
    
    

    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update(){
        MoveMouse();
    }

    private void MoveMouse(){
        float MouseX = Input.GetAxisRaw("Mouse X") * (MouseSenstivity * MouseSenstivityMultiplier) * Time.deltaTime;
        float MouseY = Input.GetAxisRaw("Mouse Y") * (MouseSenstivity * MouseSenstivityMultiplier) * Time.deltaTime;
        
        if(MouseInverted){
            xRotation += MouseY;
        }else{
            xRotation -= MouseY;
        }
        yRotation += MouseX;

        xRotation = Mathf.Clamp(xRotation, -90f, 90);
        PlayerCamera.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        Orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
        
    }

}
