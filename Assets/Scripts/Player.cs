using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 playerVel;
    public bool facingRight = true;

    //Basic movement
    [SerializeField] float playerMoveSpeed;
    [SerializeField] int jumpHeight;
    [SerializeField] float fallSpeed;
    [SerializeField] int wallSlideSpeed;
    bool wasOnGround;

    //Advanced movement
    [SerializeField] float acceleration;
    [SerializeField] float decceleration;
    [SerializeField] float velPower;

    [SerializeField] float coyoteTime;
    [SerializeField] float coyoteTimeCounter;

    [SerializeField] float jumpBuffer;
    [SerializeField] float jumpBufferCounter;

    bool lockX = false;
    bool lockY = false;

    float playerGravity;

    //Horizontal movment
    float horMovement;
    float horInput;

    //Ground checker
    bool isGrounded = false;
    [SerializeField] BoxCollider2D groundedChecker;
    [SerializeField] LayerMask groundLayer;
    
    //Wall checker
    bool isTouchingWall = false;
    string whichWall = "none";
    [SerializeField] BoxCollider2D wallCheckerLeft;
    [SerializeField] BoxCollider2D wallCheckerRight;

    //Wall sliding and jumping
    [SerializeField] int wallJumpHorLaunch;
    bool isWallSilding = false;    

    //Components
    public Rigidbody2D PlayerRigidbody2D;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D r2d;
    [SerializeField] SpriteRenderer spriteRenderer;


    //Particles
    [SerializeField] float runParticleStart;
    [SerializeField] int runParticleKickUp;

    //Run particles
    [SerializeField] ParticleSystem runParticles;
    ParticleSystem.EmissionModule runParticlesEmission;
    ParticleSystem.MinMaxCurve runParticlesRateOverTime;

    //Run into wall particles
    [SerializeField] ParticleSystem runIntoWallRParticles;
    ParticleSystem.EmissionModule runIntoWallRParticlesEmission;
    ParticleSystem.MinMaxCurve runIntoWallRParticlesRateOverTime;

    [SerializeField] ParticleSystem runIntoWallLParticles;
    ParticleSystem.EmissionModule runIntoWallLParticlesEmission;
    ParticleSystem.MinMaxCurve runIntoWallLParticlesRateOverTime;

    //Landing particles
    [SerializeField] ParticleSystem landingParticles;
    ParticleSystem.EmissionModule landingParticlesEmission;
    bool willMakeImpact = false;


    void Start()
    {
        playerGravity = r2d.gravityScale;

        //Get components
        r2d = GetComponent<Rigidbody2D>();
        PlayerRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Find objects
        

        runParticlesEmission = runParticles.emission;
        runParticlesRateOverTime = runParticlesEmission.rateOverTime;

        runIntoWallRParticlesEmission = runIntoWallRParticles.emission;
        runIntoWallRParticlesRateOverTime = runIntoWallRParticlesEmission.rateOverTime;
        runIntoWallLParticlesEmission = runIntoWallLParticles.emission;
        runIntoWallLParticlesRateOverTime = runIntoWallLParticlesEmission.rateOverTime;

        landingParticlesEmission = landingParticles.emission;
    }

    void Update()
    {
        playerVel = r2d.velocity;
        
        Particles();

        r2d.gravityScale = playerGravity;

        CheckIfGrounded();
        CheckIfTouchingWall();

        //Take input
        horInput = Input.GetAxis("Horizontal");

        //Calculate horizontal movement
        horMovement = horInput * playerMoveSpeed;

        //Flip player
        if(horMovement > 0){
            spriteRenderer.flipX = false;
            facingRight = true;
        }
        else if(horMovement < 0){
            spriteRenderer.flipX = true;
            facingRight = false;
        }

        //Set animator varibles
        animator.SetFloat("Speed", Mathf.Abs(horMovement));
        animator.SetBool("Grounded", isGrounded);
        animator.SetFloat("VerticalVel", r2d.velocity.y);

        //Start wall sliding if touching a wall and not grounded
        if(isTouchingWall && !isGrounded){
            WallSlide();
        }

        if(isGrounded || !isTouchingWall){
            isWallSilding = false;
        }

        //Coyote time stuff
        if(isGrounded){
            coyoteTimeCounter = coyoteTime;
        }
        else{
            coyoteTimeCounter -= Time.deltaTime;
        }

        //Jump buffer stuff
        if(Input.GetButtonDown("Jump")){
            jumpBufferCounter = jumpBuffer;
        }
        else{
            jumpBufferCounter -= Time.deltaTime;
        }

        PlayerJump();
    }

    void CheckIfGrounded()
    { 
        //Check for collision
        isGrounded = groundedChecker.IsTouchingLayers(groundLayer);
    }

    void CheckIfTouchingWall()
    { 
        int collisions = 0;

        if(wallCheckerLeft.IsTouchingLayers(groundLayer)){
            isTouchingWall = true;
            whichWall = "left";
            collisions++;
        }
        else if(wallCheckerRight.IsTouchingLayers(groundLayer)){
            isTouchingWall = true;
            whichWall = "right";
            collisions++;
        }
        
        if(collisions == 0)
        {
            isTouchingWall = false;
            whichWall = "none";
        }

    }

    void PlayerJump()
    {
        //Jump when button pressed
        if(coyoteTimeCounter > 0 && jumpBufferCounter > 0){
            r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
            isGrounded = false;
            isWallSilding = false;
            coyoteTimeCounter = 0f;
            willMakeImpact = true;
        }
        if(r2d.velocity.y < 0 && !isGrounded){
            r2d.gravityScale = fallSpeed;
            if(Input.GetButtonUp("Jump")){
                r2d.velocity = new Vector2(r2d.velocity.x, r2d.velocity.y * 0.5f);
            }
        }
    }

    void WallSlide()
    {
        isWallSilding = true;
        if(r2d.velocity.y <= wallSlideSpeed){
            r2d.velocity = new Vector2(r2d.velocity.x, wallSlideSpeed);
        }
    }

    void Particles(string type = "none")
    {
        if(isTouchingWall && isGrounded && Mathf.Abs(horInput)>0.1){
            if(whichWall == "left"){
                runIntoWallLParticlesEmission.rateOverTime = runIntoWallLParticlesRateOverTime;
            }
            else if(whichWall == "right"){
                runIntoWallRParticlesEmission.rateOverTime = runIntoWallRParticlesRateOverTime;
            }
            else{
                Debug.Log("this shouldnt happen");
            }
        }
        else{
            runIntoWallLParticlesEmission.rateOverTime = 0;
            runIntoWallRParticlesEmission.rateOverTime = 0;
        }

        if( Mathf.Abs(horInput) > runParticleStart && isGrounded && !isTouchingWall){
            runParticlesEmission.rateOverTime = runParticlesRateOverTime;
        }
        else{
            runParticlesEmission.rateOverTime = 0;
        }
        if(!wasOnGround && isGrounded && willMakeImpact){
            landingParticles.gameObject.SetActive(true);
            landingParticles.Stop();
            landingParticles.transform.position = runParticles.transform.position;
            landingParticles.Play();
        }
        wasOnGround = isGrounded;

        if(isGrounded){
            willMakeImpact = false;
        }
    }

    void FixedUpdate()
    {
        float targetSpeed = horMovement * playerMoveSpeed;
        
        float speedDif = targetSpeed - r2d.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;

        float movement = Mathf.Pow(Mathf.Abs(speedDif)* accelRate, velPower) * Mathf.Sign(speedDif); 

        r2d.AddForce(movement * Vector2.right);
    }

    public void PlayerRespawn()
    {
        transform.position = Vector3.zero;
        horMovement = 0;
        r2d.velocity = Vector3.zero;
    }
}