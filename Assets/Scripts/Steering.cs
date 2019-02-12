using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour
{
    public Camera camera;
    public float maxSpeed;
    private float speed;
    public float maxForce;
    public float targetRadius;
    private Rigidbody body;

    void Awake()
    {
        body = gameObject.GetComponent<Rigidbody>();
        speed = maxSpeed;
    }
    
    /* Called every physics frame. Works well with RigidBody.
     */ 
    void FixedUpdate()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // If the camera is pointing somewhere on the floor
        if (Physics.Raycast(ray, out hit))
        {
            // Adjust the speed depending on the remaining speed
            Vector3 target = hit.point;
            float distanceToTarget = Mathf.Abs(Vector3.Magnitude(target - transform.position));
            speed = SpeedAppropriateFor(distanceToTarget);

            // Get the desired velocity and apply a force
            Vector3 currentVelocity = body.velocity;
            Vector3 desiredVelocity = GetDesiredVelocity(target);
            Vector3 steerForce = desiredVelocity - currentVelocity;
            steerForce.y = 0;

            // Cap the force that can be applied (lower max force = more difficult to turn)
            if (Vector3.Magnitude(steerForce) > maxForce)
            {
                steerForce = Vector3.Normalize(steerForce) * maxForce;
            }

            body.AddForce(steerForce);
        }        
    }

    /* Maps the remainingDistance argument to an appropriate speed (interpolation).
     */ 
    float SpeedAppropriateFor(float remainingDistance)
    {
        return (remainingDistance / targetRadius) * (maxSpeed);
    }

    /* The desired velocity is simply the unit vector in the direction of the target
     * scaled by the speed of the object.
     */ 
    Vector3 GetDesiredVelocity(Vector3 target)
    {
        return Vector3.Normalize(target - transform.position) * speed;
    }
}
