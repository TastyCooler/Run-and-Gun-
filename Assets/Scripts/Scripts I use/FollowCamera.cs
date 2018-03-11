using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowCamera : MonoBehaviour
{
    #region Public Fields
    public GameObject target; // Needed to attaching the camera to an GameObject
    #endregion

    #region Private Fields
    private float interpVelocity; // Camera movement velocity
    private Vector3 targetPos; // The position the camera has to move
    #endregion

    #region Serializedfields
    [SerializeField]private Vector3 offset; // adds an offset to the camera, add 0.3f to the x-axis for a only "right" facing cam
    #endregion

    #region Unity Functions
    // Use this for initialization
    void Start()
    {
        targetPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CamMovement();
    }
    #endregion

    #region Private functions
    private void CamMovement()
    {
        if (target) // only works if a GameObject is attached to it
        {
            Vector3 posNoZ = transform.position; // setting a local variable to the cams transform
            posNoZ.z = target.transform.position.z; // setting the variable to the Targets transforms z-axis

            Vector3 targetDirection = (target.transform.position - posNoZ); // moving the cam to the Target

            interpVelocity = targetDirection.magnitude * 10f;

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.25f); // maths?!

        }
    }
    #endregion

}
