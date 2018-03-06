using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : MonoBehaviour
{

    public float speed = 10;
    public float gravity = 10;
    public float maxVelocityChange = 10;
    public float jumpheight = 2;
    

    private bool grounded = false;
    private bool dead;
    private Transform playerTransform;
    private GameObject enemy;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;


    // Use this for initialization
    void Start()
    {
        //playerTransform = GetComponent<Transform>();
        playerTransform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
       

        Vector3 targetvelocity = new Vector3(Input.GetAxis("Horizontal"), 0, 0f);
        targetvelocity = playerTransform.TransformDirection(targetvelocity);
        targetvelocity = targetvelocity * speed ;

        Vector3 velocity = _rigidbody.velocity*2;
        Vector3 velocityChange = targetvelocity - velocity * (speed*1/4) ;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
       
        velocityChange.y = 0;
        _rigidbody.AddForce(velocityChange, ForceMode2D.Force);
        //print(_rigidbody.velocity);

        if (Input.GetButton("Jump") && grounded == true )
        {
            _rigidbody.velocity = new Vector3(velocity.x, CalculateJump(), velocity.z);
            
        }
        grounded = false;
        _rigidbody.AddForce(new Vector3(0, -gravity * _rigidbody.mass, 0));

          Vector3 _velocity = _rigidbody.velocity;
        
        print(_velocity);
        print(grounded);
       

        bool flipSprite = (spriteRenderer.flipX ? (_velocity.x > 0.01f) : (_velocity.x < -0.1f));

        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;

        }
        
        
        animator.SetFloat("velocityX", Mathf.Abs(_velocity.x) / maxVelocityChange);
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        grounded = true;
        print(grounded);
        animator.SetBool("grounded", grounded);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        animator.SetBool("grounded", grounded);
        grounded = false;
    }


    float CalculateJump()
    {
        float jump = Mathf.Sqrt(2 * jumpheight * gravity);
        return jump;
    }

  
}
   