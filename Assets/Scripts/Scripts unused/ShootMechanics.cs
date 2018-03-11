using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootMechanics : MonoBehaviour {

    private Rigidbody2D _rigidbody;
    public float speed = 5;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
        
        Vector3 shot = transform.position;
        Vector2 rigi = _rigidbody.velocity;
       rigi  = new Vector2(rigi.x * speed * Time.deltaTime, transform.position.y);
        shot = new Vector3(rigi.x, transform.position.y);
	}
}
