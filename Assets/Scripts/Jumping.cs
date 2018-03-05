using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : MonoBehaviour
{

    public float speed = 10;
    public float gravity = 10;
    public float maxVelocityChange = 10;
    public float jumpheight = 2;

    private bool grounded;
    private bool dead;
    private Transform playerTransform;
    private GameObject enemy;
    private Rigidbody2D _rigidbody;


    // Use this for initialization
    void Start()
    {
        //playerTransform = GetComponent<Transform>();
        playerTransform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.freezeRotation = true;
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

        if (Input.GetButtonDown("Jump") )
        {
            _rigidbody.velocity = new Vector3(velocity.x, CalculateJump(), velocity.z);
        }

        _rigidbody.AddForce(new Vector3(0, -gravity * _rigidbody.mass, 0));

        grounded = false;

          Vector3 _velocity = _rigidbody.velocity;
          print(_velocity);
        print(grounded);

    }


    private void OnCollisionStay(Collision collision)
    {
        grounded = true;
    }

    float CalculateJump()
    {
        float jump = Mathf.Sqrt(2 * jumpheight * gravity);
        return jump;
    }
}
   