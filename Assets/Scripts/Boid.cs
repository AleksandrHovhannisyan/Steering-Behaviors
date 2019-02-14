using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private float desiredSeparation;
    private float alignmentRadius;
    private float cohesionRadius;

    public float maxSpeed = 20;
    public float maxForce;
    private Rigidbody body;


    /* Set up all necessary members.
     */ 
    private void Awake()
    {
        body = gameObject.GetComponent<Rigidbody>();

        float radius = transform.localScale.x / 2;

        maxSpeed = (int)Random.Range(10, 30);

        desiredSeparation = radius * radius;
        alignmentRadius = radius * radius + 2;
        cohesionRadius = radius * radius + 2;
    }


    /* Called each frame by the group manager. Identifies boids in this boid's neighborhood
     * and applies an appropriate force to separate it from the others and prevent collision.
     */
    public Vector3 Separate()
    {
        Vector3 totalSeparation = Vector3.zero;
        int numNeighbors = 0;

        // Loop through all boids in the world
        foreach(GameObject boid in GroupManager.boids)
        {
            Boid neighbor = boid.GetComponent<Boid>();

            Vector3 separationVector = transform.position - neighbor.transform.position;
            float distance = Vector3.Magnitude(separationVector);

            // If it's a neighbor within our vicinity
            if(distance > 0 && distance < desiredSeparation)
            {
                separationVector.Normalize();

                // The closer a neighbor (smaller the distance), the more we should flee
                separationVector /= distance; // TODO problematic or nah?

                totalSeparation += separationVector;
                numNeighbors++;
            }
        }

        // That is, if this boid actually has neighbors to worry about
        if(numNeighbors > 0)
        {
            // Compute its average separation vector
            Vector3 averageSeparation = totalSeparation / numNeighbors;
            averageSeparation.Normalize();
            averageSeparation *= maxSpeed;

            // Compute the separation force we need to apply
            Vector3 separationForce = averageSeparation - body.velocity;

            // Cap that separation force
            if (separationForce.magnitude > maxForce)
            {
                separationForce.Normalize();
                separationForce *= maxForce;
            }
            
            return separationForce;
        }

        return Vector3.zero;
    }


    /* Called each frame by the group manager. Aligns this boid with all other boids
     * in its immediate neighborhood, effectively making it travel in the same direction.
     */
    public Vector3 Align()
    {
        Vector3 totalHeading = Vector3.zero;
        int numNeighbors = 0;

        // Loop through all boids in the world
        foreach (GameObject vehicle in GroupManager.boids)
        {
            Boid boid = vehicle.GetComponent<Boid>();

            Vector3 separationVector = transform.position - boid.transform.position;
            float distance = Vector3.Magnitude(separationVector);

            // If it's a neighbor within our vicinity
            if (distance > 0 && distance < alignmentRadius)
            {
                numNeighbors++;
                totalHeading += boid.body.velocity.normalized;
            }
        }

        // That is, if this boid actually has neighbors to worry about
        if (numNeighbors > 0)
        {
            // Average direction we need to head in
            Vector3 averageHeading = (totalHeading / numNeighbors);
            averageHeading.Normalize();

            // Compute the steering force we need to apply
            Vector3 alignmentForce = averageHeading * maxSpeed;

            // Cap that steering force
            if (alignmentForce.magnitude > maxForce)
            {
                alignmentForce.Normalize();
                alignmentForce *= maxForce;
            }

            return alignmentForce;
        }

        return Vector3.zero;
    }
    

    /* Returns a vector that can be used to direct a boid towards the target position.
     */ 
    public Vector3 Seek(Vector3 target)
    {
        float distanceToTarget = Mathf.Abs(Vector3.Magnitude(target - transform.position));

        // Calculate velocities
        Vector3 currentVelocity = body.velocity;
        Vector3 desiredVelocity = GetDesiredVelocity(target) / distanceToTarget;

        // Force to be applied to the boid
        Vector3 steerForce = desiredVelocity - currentVelocity;

        // Cap the force that can be applied (lower max force = more difficult to turn)
        if (steerForce.magnitude > maxForce)
        {
            steerForce.Normalize();
            steerForce *= maxForce;
        }

        return steerForce;
    }


    /* Called each frame by the group manager. Ensures this boid remains close to neighboring boids.
    */
    public Vector3 Cohere()
    {
        Vector3 totalPositions = Vector3.zero;
        int numNeighbors = 0;

        // Loop through all boids in the world
        foreach (GameObject vehicle in GroupManager.boids)
        {
            Boid boid = vehicle.GetComponent<Boid>();

            Vector3 separationVector = transform.position - boid.transform.position;
            float distance = Vector3.Magnitude(separationVector);

            // If it's a neighbor within our vicinity, add its position to cumulative
            if (distance > 0 && distance < cohesionRadius)
            {
                numNeighbors++;
                totalPositions += boid.body.velocity.normalized;
            }
        }

        // If there are neighbors
        if(numNeighbors > 0)
        {
            Vector3 averagePosition = (totalPositions / numNeighbors);

            return Seek(averagePosition);
        }

        return Vector3.zero;
    }


    /* The desired velocity is simply the unit vector in the direction of the target
     * scaled by the speed of the object.
     */
    Vector3 GetDesiredVelocity(Vector3 target)
    {
        return Vector3.Normalize(target - transform.position) * maxSpeed;
    }
}
