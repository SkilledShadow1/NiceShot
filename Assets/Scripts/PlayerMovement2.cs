using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class PlayerMovement2 : MonoBehaviour
{
    
    //Other
    private Rigidbody2D rb;
    private GroundCheck gc;

    [SerializeField] float constantGravity;

    //Movement
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public float airSpeedMultiplier;
    public LayerMask whatIsGround;
    private float speedMultplier = 1f;
    
    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    [SerializeField] float jumpForce;

    private CooldownManager cm;
    private CooldownData frictionStopper;

    private GrappleGun grapple;

    public  LayerMask[] ignoreLayers;

    private WallCheck leftWallCheck;
    private WallCheck rightWallCheck;

    public GameObject leftWall;
    public GameObject rightWall;

    public float inputFreezeTime = 0.2f;
    bool freezeInput;

    [SerializeField] float maxPlayerHealth = 100;
    [SerializeField]float currentPlayerHealth;

    //Input
    float x, y;


    void Start() {
        PlayerInput.Instance.StartingValues();
        rb = GetComponent<Rigidbody2D>();
        gc = GetComponentInChildren<GroundCheck>();
        grapple = GetComponent<GrappleGun>();
        cm = CooldownManager.Instance;
        frictionStopper = cm.FindCooldown(CooldownData.CooldownType.FRICTION);
        speedMultplier = 1f;

        leftWallCheck = leftWall.GetComponent<WallCheck>();
        rightWallCheck = rightWall.GetComponent<WallCheck>();

        freezeInput = false;
        inputWait = 0f;

        currentPlayerHealth = maxPlayerHealth;
    }

    public float checkRadius;

    private void FixedUpdate() 
    {
        Movement();
        PlayerJump();
        WallCling(leftWallCheck, -1);
        WallCling(rightWallCheck, 1);
    }


    private void Update() {
        MyInput();
    }

    /// <summary>
    /// Find user input. Should put this in its own class but im lazy
    /// </summary>
    /// 
    private float inputWait;

    void MyInput()
    {
        if (freezeInput) 
        {

            x = 0;
            inputWait += Time.deltaTime;

            if (inputWait >= inputFreezeTime)
            {
                inputWait = 0;
                freezeInput = false;
            }

        }

        else 
        {
            
            x = PlayerInput.Instance.horizontalMovement;
        }
    }


    private void Movement() {
        //Extra gravity
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - constantGravity);


        //Counteract sliding and sloppy movement
        CounterMovement(x, y, rb.velocity.x, rb.velocity.y);
        float maxSpeed = this.maxSpeed;

        //Set max speed
        if (!gc.playerIsGrounded)
        {
            maxSpeed = this.maxSpeed;
        }

       


        
        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (x > 0 && rb.velocity.x > maxSpeed) x = 0;
        if (x < 0 && rb.velocity.x < -maxSpeed) x = 0;

        speedMultplier = 1f;
        // Movement in air
        if (!gc.playerIsGrounded) {
            speedMultplier = airSpeedMultiplier;
        }
        
        //Apply forces to move player
        if(!grapple.isGrappling)
            rb.AddForce(transform.right * x * moveSpeed * Time.deltaTime * speedMultplier);
    }

    private void CounterMovement(float x, float y, float magX, float magY) {
        //if(!grapple.isGrappling)
            if (!gc.playerIsGrounded || !frictionStopper.IsReady()) 
                return;

        //Counter movement
        if (Math.Abs(magX) > threshold && Math.Abs(x) < 0.05f || (magX < -threshold && x > 0) || (magX > threshold && x < 0)) {
            rb.AddForce(moveSpeed * Vector2.right * Time.deltaTime * -magX * counterMovement);
        }
        /*
        if (grapple.isGrappling) 
        {
            if (Math.Abs(magY) > threshold && Math.Abs(y) < 0.05f || (magY < -threshold && y > 0) || (magY > threshold && y < 0))
            {
                rb.AddForce(moveSpeed * Vector2.up * Time.deltaTime * -magY * counterMovement);
            }
        }
        */
    }

    private void PlayerJump()
    {

        if (gc.playerIsGrounded && PlayerInput.Instance.pressingJump)
        {
            PlayerInput.Instance.jumped = true;
            Vector2 yForce = new Vector2(0, jumpForce);
            rb.AddForce(yForce, ForceMode2D.Impulse);
        }
    }

    public Transform wallCheck;
    bool wallSliding;
    public float wallSlidingSpeed;

    private void WallCling(WallCheck wall, int neededInput) 
    {
        if (wall.isTouchingWall && !gc.playerIsGrounded && x == neededInput) 
        {
            wallSliding = true;

        }
        else 
        {
            wallSliding = false;
        }

        if (wallSliding) 
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            
        }

        WallJump();
    }

    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;

    private void WallJump() 
    {
        if(PlayerInput.Instance.pressingJump && wallSliding) 
        {
            freezeInput = true;
            PlayerInput.Instance.jumped = true;
            wallSliding = false;
            rb.AddForce(new Vector2(xWallForce * -PlayerInput.Instance.horizontalMovement, yWallForce));
        }

    }


    public void Damage(int damageTaken) 
    {
        currentPlayerHealth -= damageTaken;
        if(currentPlayerHealth < 0) 
        {
            SceneManager.LoadScene(0);
        }
    }


}