using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    public float moveSpeed;
    Vector3 moveInput;
    public float jumpHeight;
    Vector3 jumpInput;
    bool grounded;
    Camera mainCamera;
    Vector3 cameraForward, cameraRight;
    public float dashSpeed;
    public float dashTime;
    
    public int dashesLeft;
    float dashTimer;
    Vector3 dashDirection;

    public Material[] materials;
    Vector3 originScale;
    public float scaleSmoothTime;
    Vector3 scaleVelocity;
    MeshRenderer meshRenderer;
    public LayerMask bounceLayers;
    Collider lastCollectedDashRefresh;
    public Vector3 gravity;
    Vector3 moveVelocityFinal;
    Vector3 jumpVelocityFinal;
    Vector3 dashVelocityFinal;
    Vector3 gravityVelocityFinal;
    float notGroundedTime;
    public float dashVelocityTime;
    Vector3 dashVelocityRef;
    public float moveVelocityTime;
    Vector3 moveVelocityRef;
    float zeroGravityTime;
    void Awake()
    {
        dashesLeft = 1;
        originScale = transform.localScale;
        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        //Register move input callback with InputManager
        InputManager.RegisterMoveInputCallback((Vector2 moveInput) => { this.moveInput = new Vector3(moveInput.x, 0f, moveInput.y); }) ;
        //Register jump input callback with InputManager
        InputManager.RegisterJumpInputCallback((float heldTime) => { 
            if(heldTime > 0)
            {
                jumpInput = new Vector3(0f, 1f, 0f);
            }
            else
            {
                jumpInput = Vector3.zero;
            }
        });
        //Register mouse left click callback with InputManager
        InputManager.RegisterMouseLeftClickCallback((float heldTime) =>
        {
            if(heldTime > 0 && dashesLeft > 0)
            {
                PerformDash();
            }
        });
    }
    private void Update()
    {
        cameraForward = mainCamera.transform.forward;
        cameraRight = mainCamera.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        if(dashTimer > 0)
        {
            meshRenderer.material = materials[1];
            dashTimer -= Time.deltaTime;
        }
        else
        {
            meshRenderer.material = materials[0];
        }

        if(zeroGravityTime > 0)
        {
            zeroGravityTime -= Time.deltaTime;
        }

        transform.localScale = Vector3.SmoothDamp(transform.localScale, originScale, ref scaleVelocity, scaleSmoothTime);
    }
    void FixedUpdate()
    {
        //Add velocity in directions relative to camera
        moveVelocityFinal += (cameraForward * moveInput.z + cameraRight * moveInput.x);
        //If move input small or zero
        if(moveInput.magnitude <= 0.1f)
        {
            //Smooth movement speed to zero in set smooth time
            moveVelocityFinal = Vector3.SmoothDamp(moveVelocityFinal, Vector3.zero, ref moveVelocityRef, moveVelocityTime);
        }
        //Clamp move velocity to be no larger than moveSpeed
        moveVelocityFinal = Vector3.ClampMagnitude(moveVelocityFinal, moveSpeed);
        //If not grounded start applying gravity
        if(!grounded && zeroGravityTime <= 0)
        {
            gravityVelocityFinal += gravity;
        }
        else
        {
            //If grounded set gravity to zero
            gravityVelocityFinal = Vector3.zero;
        }

        //If jump input and we are grounded then set jump velocity final to jump height
        if(jumpInput.y > 0 && grounded)
        {
            jumpVelocityFinal = new Vector3(0f, jumpHeight, 0f);
        }
        else
        {
            jumpVelocityFinal += gravity;
            if (jumpVelocityFinal.y <= 0)
            {
                jumpVelocityFinal = Vector3.zero;
            }
        }
        if (dashVelocityFinal.magnitude > 0)
        {
            RaycastHit dashHit;
            if (Physics.SphereCast(transform.position, 0.5f, dashVelocityFinal.normalized, out dashHit, dashVelocityFinal.magnitude * Time.fixedDeltaTime, bounceLayers))
            {
                dashDirection = Vector3.Reflect(dashDirection, dashHit.normal);
                transform.localScale = Vector3.one*2f;
                dashTimer = dashTime;
                gravityVelocityFinal = Vector3.zero;
            }
            dashVelocityFinal = Vector3.SmoothDamp(dashVelocityFinal, Vector3.zero, ref dashVelocityRef, dashVelocityTime);
        }
        if (dashTimer > 0)
        {
            dashVelocityFinal = dashDirection * dashSpeed;
        }

        Vector3 dv = (moveVelocityFinal + gravityVelocityFinal + jumpVelocityFinal + dashVelocityFinal);

        Vector3 newPos = rb.position + dv;
        rb.velocity = dv;
    }
    private void OnCollisionEnter(Collision col)
    {
        if(col.collider.CompareTag("Ground"))
        {
            grounded = true;
            if(dashesLeft < 1)
            {
                dashesLeft++;
            }
        }
    }
    private void OnCollisionExit(Collision col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
    public void Reset()
    {
        //Reset all velocities
        rb.velocity = Vector3.zero;
        moveVelocityFinal = Vector3.zero;
        jumpVelocityFinal = Vector3.zero;
        gravityVelocityFinal = Vector3.zero;
        dashVelocityFinal = Vector3.zero;
        dashesLeft = 1;
    }
    void PerformDash()
    {
        dashTimer = dashTime;
        dashDirection = mainCamera.transform.root.forward;
        dashesLeft--;
    }
    public void DashRefresh()
    {
        Debug.Log("Dash Refresh");
        dashesLeft = 1;
    }
    public void ZeroGravity(float zeroGravTime)
    {
        zeroGravityTime = zeroGravTime;
    }
}
