 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int playerNum = 1;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [Space, Header("Ground Movement")]
    public float maxSpeed = 400;
    public float timeToMax = 0.15f;
    public float timeToMin = 0.08f;

    public float turnSlow = 230;
    public float speedThreshold = 300;

    private float accel;
    private float decel;
    private float speed;

    private int face = 1;

    [Space, Header("Jump Variables")]
    public float jumpForce;
    public float jumpDecay;

    public float maxFall;
    public float fallAccel;
    private bool jumping = false;
    private float ySpeed = 0;

    private bool grounded = false;
    public LayerMask ground;
    public GameObject checkObj;
    public Vector2 checkSize;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        MovementCalcs();
        GetJump(playerNum);
        JumpCalcs();
        GroundCheck();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(speed * face * Time.fixedDeltaTime, ySpeed * Time.fixedDeltaTime);
    }



    private void MovementCalcs()
    {
        float hor = GetInput(playerNum);

        if (hor != 0)
        {
            speed += accel * Time.deltaTime;
        }
        else
        {
            speed += decel * Time.deltaTime;
        }


        accel = (maxSpeed - 0) / timeToMax;
        decel = (0 - maxSpeed) / timeToMin;

        speed = Mathf.Clamp(speed, 0, maxSpeed);


        if (face == 1)
        {
            if (hor < 0)
            {
                face = -1;
                if (speed > speedThreshold)
                {
                    speed -= turnSlow;
                }
                
            }

            sr.flipX = false;
        }
        if (face == -1)
        {
            if (hor > 0)
            {
                face = 1;
                if (speed > speedThreshold)
                {
                    speed -= turnSlow;
                }
            }

            sr.flipX = true;
        }
    }

    private void JumpCalcs()
    {
        if (grounded && GetJump(playerNum))
        {
            jumping = true;
            ySpeed = jumpForce;
        }

        if (jumping)
        {
            ySpeed -= jumpDecay * Time.deltaTime;

            if (ySpeed <= 0)
            {
                jumping = false;
            }

            if (GetJump(playerNum) != true)
            {
                jumping = false;
                ySpeed = ySpeed / 2;
            }
        }
        else
        {
            if (!grounded)
            {
                ySpeed -= fallAccel * Time.deltaTime;
            }
            else
            {
                ySpeed = 0;
            }
        }

        ySpeed = Mathf.Clamp(ySpeed, -maxFall, jumpForce);
    }

    private bool GetJump(int port)
    {
        switch(port)
        {
            case 1:
                if (Input.GetButton("Jump"))
                {
                   
                    return true;
                }
                break;
            case 2:


                if (Input.GetButton("Jump2"))
                {

                    return true;
                }
                break;
        }

        return false;

    }

    private float GetInput(int port)
    {
        if (port == 1)
        {
            return Input.GetAxisRaw("Horizontal");
        }
        else if (port == 2)
        {
            return Input.GetAxisRaw("Horizontal2");
        }
        else
        {
            return 0;
        }
        
    }

    private void GroundCheck()
    {
        grounded = Physics2D.OverlapBox(checkObj.transform.position, checkSize, 0, ground);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(checkObj.transform.position, checkSize);
    }
}
