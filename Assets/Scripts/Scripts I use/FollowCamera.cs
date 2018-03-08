using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowCamera : MonoBehaviour
{

    private float interpVelocity;
    public GameObject target;
    private Vector3 offset;
    Vector3 targetPos;
    // Use this for initialization
    void Start()
    {
        targetPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;

            Vector3 targetDirection = (target.transform.position - posNoZ);

            interpVelocity = targetDirection.magnitude * 10f;

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position , targetPos + offset, 0.25f);

        }
    }
}
