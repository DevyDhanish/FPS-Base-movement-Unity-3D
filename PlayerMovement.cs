using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Required Objects")]
    public Transform Orientation;
    public Rigidbody PlayerRB;
    public Transform Head;


    [Header("Speed")]
    public float MovementSpeed;
    [Range(0,1000f)]
    public float MovementSpeedMultiplier;

    [Header("Ground Check")]
    public float PlayerHeight;
    public LayerMask WhatisGround;
    public float GroundDrag;
    public bool isGrounded;
    
    [Header("Head Bobbing")]
    public bool EnableHeadBobbing;
    public float walkingBobbingSpeed = 14f;
    public float bobbingAmount = 0.05f;
    private float defaultPosY = 0;
    private float timer = 0;

    [Header("Jump")]
    public bool EnableJump;
    public KeyCode JumpKey = KeyCode.Space;
    public float JumpForce;
    [Range(0,1000f)]
    public float JumpForceMultiplier;
    public float JumpCoolDown;
    public bool isReadToJump = true;

    [Header("Crouch")]
    public bool EnableCrouch;
    public KeyCode CrouchKey = KeyCode.LeftControl;
    public bool isCrouching = false;

    void Start()
    {
        defaultPosY = Head.localPosition.y;
    }
    
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.2f, WhatisGround);

        SpeedControl();

        if(EnableHeadBobbing && isGrounded && isCrouching == false){
            HeadBobbing();
        }

        if(Input.GetKey(JumpKey) && isGrounded && isReadToJump && EnableJump){
            isReadToJump = false;
            Jump();
            Invoke(nameof(ResetJump), JumpCoolDown);
        }

        if(Input.GetKeyDown(CrouchKey) && isGrounded && isCrouching == false && EnableCrouch){
            isCrouching = true;
            Crouch();
        }

        if(Input.GetKeyUp(CrouchKey) && isGrounded && isCrouching && EnableCrouch){
            isCrouching = false;
            StopCrouch();
        }

        if(isGrounded){
            PlayerRB.drag = GroundDrag;
        }else{
            PlayerRB.drag = 0;
        }
    }

    void FixedUpdate(){
        MovePlayer();
    }

    private void MovePlayer(){

        float HorizontalInput = Input.GetAxisRaw("Horizontal");
        float VerticalInput = Input.GetAxisRaw("Vertical");

        Vector3 MoveDir = Orientation.forward * VerticalInput + Orientation.right * HorizontalInput;
        PlayerRB.AddForce(MoveDir.normalized * MovementSpeed * MovementSpeedMultiplier * Time.fixedDeltaTime, ForceMode.Force);

    }

    private void SpeedControl(){
        Vector3 FlatVel = new Vector3(PlayerRB.velocity.x, 0f, PlayerRB.velocity.z);
        if(FlatVel.magnitude > MovementSpeed){
            Vector3 LimVel = FlatVel.normalized * MovementSpeed;
            PlayerRB.velocity = new Vector3(LimVel.x, PlayerRB.velocity.y, LimVel.z);
        }
    }

    private void Jump(){
        PlayerRB.AddForce(Vector3.up * JumpForce * JumpForceMultiplier * Time.fixedDeltaTime, ForceMode.Impulse);
    }
    private void ResetJump(){
        isReadToJump = true;
    }

    private void Crouch(){
        transform.localScale = new Vector3(1f, 0.5f, 1f);
    }

    private void StopCrouch(){
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void HeadBobbing(){
        if(PlayerRB.velocity.magnitude > 0.1f)
        {
            timer += Time.deltaTime * walkingBobbingSpeed;
            Head.localPosition = new Vector3(Head.localPosition.x, defaultPosY + Mathf.Sin(timer) * bobbingAmount, Head.localPosition.z);
        }
        else
        {
            timer = 0;
            Head.localPosition = new Vector3(Head.localPosition.x, Mathf.Lerp(Head.localPosition.y, defaultPosY, Time.deltaTime * walkingBobbingSpeed), Head.localPosition.z);
        }
    }
}
