using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Horizontal movement
    [SerializeField] private float speed;
    private float horizontal;
    private Vector2 movement;

    //Jumping
    [SerializeField] private float jumpingForce;
    private bool isJumping;
    private bool onGround;
    private Vector2 jumpingVector;
    private bool canDoubleJump;
    private bool isDoubleJump;

    private Rigidbody2D rb2d;
    private Animator anim;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isJumping = false;
        canDoubleJump = false;
        isDoubleJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        SelectAnimaton();

        Debug.Log(rb2d.velocity.y);

        if(rb2d.velocity.y < -0.05f)
        {
            anim.SetInteger("isJumping", -1);
        }

    }

    void FixedUpdate()
    {
        if (!isJumping && onGround)
        {
            Movement();
        }
        else if(isJumping && onGround)
        {
            Jump(1f);
        }
        else if(isJumping && isDoubleJump && !onGround)
        {
            Jump(1.5f);
            isDoubleJump = false;
        }
        
    }

    private void GetInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(!isJumping && onGround)
            {
                isJumping = true;
                anim.SetInteger("isJumping", 1);
                canDoubleJump = true;
            }
            else if(canDoubleJump)
            {
                canDoubleJump = false;
                isDoubleJump = true;
                anim.SetInteger("isJumping", 2);
            }
        }
    }

    private void Movement()
    {       
        movement = new Vector2(horizontal * speed * Time.fixedDeltaTime, 0);
        rb2d.velocity = movement;
    }

    private void SelectAnimaton()
    {
        if(horizontal != 0)
        {
            if(horizontal < 0 )
            {
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }

            anim.SetBool("isRunning", true);
        }
        else
        {   
            anim.SetBool("isRunning", false);
        }
    }

    private void Jump(float forceMultiplier)
    {
        jumpingVector = new Vector2(0, jumpingForce * Time.fixedDeltaTime * forceMultiplier);
        rb2d.AddForce(jumpingVector, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            isJumping = false;
            onGround = true;
            canDoubleJump = false;
            anim.SetInteger("isJumping", 0);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            onGround = false;
           
        }
    }

}
