using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Public fields
    public GameObject shot;

    #endregion

    #region Serializedfields
    [SerializeField] private float speed = 15; // Players speed; will be set to 15
    [SerializeField] private float gravity = 3; // Gravity which pushes against the Player; will be set to 3
    [SerializeField] private float maxVelocityChange = 50; // How fast the Player is allowed to be; will be set to 50
    [SerializeField] private float jumpheight = 2; // Players jump height; will be set to 2
    [SerializeField] private float shotSpeed = 3f;
    [SerializeField] private float shootRate = 0.2f;

    #endregion

    #region private fields
    private bool grounded = false; // Used to check if the player hits the ground
    private bool dead; // Unused yet; will be used to respawn the player|| end game
    private Transform playerTransform; // Players Transform component
    private Rigidbody2D _rigidbody; // Players Rigidbody; Mass: 0.25f, LD: 0, AD: 0.05f, Gravity S.: 1
    private SpriteRenderer spriteRenderer; // Players SpriteRenderer component; used to flip the Sprite after turning the character
    private Animator animator; // Players Animator component; used to set conditions for the AnimatorController
    private int lookState;
    private BoxCollider2D collider;

    #endregion

    #region Unity functions
    // Use this for initialization
    void Start()
    {
        playerTransform = transform; // Equals the Players Transform component to the attached gameObject
        GetComponents(); // Gets component references
        

    }

    // used for physics 
    //! TODO: Seperate Player Input; because it has to be in Update (needs more frequent updates else inputs might be ignored) || set FixedUpdate updates amount more frequent
    private void FixedUpdate()
    {
        Moving();
        Flipping();
        AddRay();
        
    }

    private void Update()
    {
        Shooting();
        //Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), new Vector3(-0.15f, -0.1f) * 0.3f);
        //Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), new Vector3( 0.15f, -0.1f) * 0.3f);
    }
    #endregion
    
   
    #region Functions
    void Moving()
    {
        #region moving()
        Vector3 targetvelocity = new Vector3(Input.GetAxis("Horizontal"), 0, 0f); // new Vector3 variable (targetvelocity) set to a new Vector3, which includes Players x-axis.velocity && used for moving
        targetvelocity = playerTransform.TransformDirection(targetvelocity); //! Actually does nothing? Returns the targetvelocity; redundant code?
        targetvelocity = targetvelocity * speed; // Adding Players speed to the targetvelocity
        
        Vector3 velocity = _rigidbody.velocity * 2; // new Vector3 variable (velocity) set to the Players-Rigidbody velocity times 2; 2 because it made the jump feel better
        Vector3 velocityChange = targetvelocity - velocity * (speed /4); // new Vector3 variable (velocityChange) set to the calculation of: (targetvelocity - velocity * (speed / 4)) Rigidbodies mass and gravity firstly gets used here
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange); // clamping the new x-axis velocity between -maxVelocityChange and maxVelocityChange
        velocityChange.y = 0; // Y-Axis has to be set to 0; else the Y velocity gets calculated all the time 
        _rigidbody.AddForce(velocityChange, ForceMode2D.Force); // adds the value of velocityChange as a force to the rigidbody
        #endregion
        #region jump()
        if (Input.GetButton("Jump") && grounded == true) // Adding the Jump Input. Players only able to jump if he is standing on the ground (if grounded == true)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, CalculateJump()); // Setting the rigidbodies velocity to a new Vector3 (can also be Vector2), with the CalculateJump function

        }
        
        _rigidbody.AddForce(new Vector3(0, -gravity * _rigidbody.mass, 0)); // adds the -gravity * the rigidbodys mass, to simulate a more realistic jump, since the Character has a mass
                                                                            // its a ghost...
        
        #endregion
    }

    void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {

            InvokeRepeating("Shooter", 0.000001f, shootRate);
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            CancelInvoke("Shooter");
        }
    }

    void Shooter()
    {
        Vector3 offset = transform.position + new Vector3(0.1f, 0f, 0f);
        Vector2 slowStart = new Vector2(1f, 0f);
        if (lookState == 0)
        {
           offset = transform.position + new Vector3(0.1f, 0f, 0f);
            GameObject shoot = Instantiate(shot, offset, Quaternion.identity) as GameObject;
            shoot.GetComponent<Rigidbody2D>().AddForce(slowStart * shotSpeed, ForceMode2D.Impulse);
                Destroy(shoot, 5);

        } else if ( lookState == 1)
        {
            offset = transform.position + new Vector3(-0.1f, 0f, 0f);
            GameObject shoot = Instantiate(shot, offset, Quaternion.identity) as GameObject;
            Rigidbody2D rShot = shoot.GetComponent<Rigidbody2D>();
            rShot.AddForce(-slowStart  * shotSpeed, ForceMode2D.Impulse);
            if(rShot.velocity.x < 0f)
            {
                shoot.transform.localScale = new Vector3(-shoot.transform.localScale.x, shoot.transform.localScale.y , shoot.transform.localScale.z);
            }
            Destroy(shoot, 5);
        }
        
    }

    void Flipping()
    {
        Vector3 _velocity = _rigidbody.velocity; // setting the rigidbodys velocity to a new variable (_velocity)
        bool flipSprite = (spriteRenderer.flipX ? (_velocity.x > 0.01f) : (_velocity.x < -0.01f)); // ? returns a bool value; it checks if the velocity.x is bigger than 0.01f
        // or is less than -0.01f; if its bigger it returns a true and flips the sprite to the right; if its false it flips the sprite to the left
        if (flipSprite) // if flipSprite is false
        {
            spriteRenderer.flipX = !spriteRenderer.flipX; //flip the Sprite to the left

        }


        animator.SetFloat("velocityX", Mathf.Abs(_velocity.x) / maxVelocityChange); // velocityX used as a condition for the AnimationController, to detect if the player is moving -> to start the moving anim.
    }

    
    void AddRay()
    {
        Vector3 _velocity = _rigidbody.velocity;
        if (_velocity.x > 0.01f)
        {
            lookState = 0;
        }
        else if (_velocity.x < -0.01f)
        {
            lookState = 1;
        }
        if (lookState == 0 && Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector3(0.15f, -0.1f), 0.3f))
        {
           // RaycastHit2D ray = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector3(0.15f, -0.1f), 0.5f);
           // print("rayhits" + ray.collider.gameObject.layer.ToString());
            collider.size = new Vector2(1f, collider.size.y);
        } else if (lookState == 1 && Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector3(-0.15f, -0.1f), 0.3f))
        {
           // RaycastHit2D ray = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector3(-0.15f, -0.1f), 0.5f);
           // print("rayhits" + ray.collider.gameObject.layer.ToString());
            collider.size = new Vector2(1f, collider.size.y);
        } else
        {
           // print("ray no hit");
            collider.size = new Vector2(1.5f, collider.size.y);
        }
        
        
    }

    private void OnCollisionStay2D(Collision2D collision) // if the player is colliding with something, grounded is set to true 
    {
        grounded = true;
        animator.SetBool("grounded", grounded); // sets the animation condition "grounded" to the grounded variables value
    }

    private void OnCollisionExit2D(Collision2D collision) // if the player doesn't collide with anything
    {
        grounded = false;
        animator.SetBool("grounded", grounded);
        
    }


    float CalculateJump() // Calculation used for jumping
    {
        float jump = Mathf.Sqrt(2 * jumpheight * gravity); // The Jump is the square root of (2* jumpheight * gravity)
        return jump;
    }

  
    private void GetComponents()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
    }
    #endregion
}
