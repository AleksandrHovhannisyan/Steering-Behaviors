using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wandering : MonoBehaviour
{
    public float maxSpeed;
    private float speed;
    public float maxForce;
    public float radius;
    private Rigidbody body;
    

    void Awake()
    {
        body = gameObject.GetComponent<Rigidbody>();
        speed = maxSpeed;
        body.velocity = new Vector3(5, 0, 5);
    }

    
    void Update()
    {
        // Get future position
        Vector3 futurePosition = GetFuturePosition();

        // Select random point on circle of radius "radius" around the future position
        Vector3 target = GeneratePointOnCircle(futurePosition);

        // Compute desired velocity as one pointing there
        Vector3 desiredVelocity = GetDesiredVelocity(target);

        // Get the steering force vector
        Vector3 steerForce = desiredVelocity - body.velocity;
        steerForce.y = 0;

        // Cap the force that can be applied (lower max force = more difficult to turn)
        if (Vector3.Magnitude(steerForce) > maxForce)
        {
            steerForce = Vector3.Normalize(steerForce) * maxForce;
        }

        // Apply the force to the body
        body.AddForce(steerForce);
    }


    /* Returns a random point on a circle positioned at the given center and radius.
     */ 
    Vector3 GeneratePointOnCircle(Vector3 center)
    {
        Vector3 point = center;

        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
        point.x += radius * Mathf.Cos(angle);
        point.z += radius * Mathf.Sin(angle);

        return point;
    }


    /* Computes and returns the future, predicted position of this object, assuming
     * it continues traveling in its current direction at its current speed.
     */
    Vector3 GetFuturePosition()
    {
        // We have a current velocity
        // We have a time elapsed
        // We have a current position
        // Future position = current position + current velocity * delta time

        return transform.position + body.velocity * Time.deltaTime;
    }


    /* The desired velocity is simply the unit vector in the direction of the target
     * scaled by the speed of the object.
     */
    Vector3 GetDesiredVelocity(Vector3 target)
    {
        return Vector3.Normalize(target - transform.position) * speed;
    }
}
