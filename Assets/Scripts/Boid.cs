using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private float desiredSeparation;
    public int maxSpeed = 60;
    public int maxForce = 40;
    private Rigidbody body;


    /* Set up all necessary members.
     */ 
    private void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();

        float radius = transform.localScale.x / 2;

        desiredSeparation = radius * radius;
    }


    /* Called each frame by the group manager. Identifies boids in this boid's neighborhood
     * and applies an appropriate force to separate it from the others and prevent collision.
     */ 
    public void Separate()
    {
        Vector3 sum = Vector3.zero;
        int numNeighbors = 0;

        // Loop through all boids in the world
        foreach(GameObject vehicle in GroupManager.boids)
        {
            Boid boid = vehicle.GetComponent<Boid>();

            Vector3 diff = transform.position - boid.transform.position;
            float distance = Vector3.Magnitude(diff);

            // If it's a neighbor within our vicinity
            if(distance > 0 && distance < desiredSeparation)
            {
                diff.Normalize();

                // The closer a neighbor (smaller the distance), the more we should flee
                diff /= distance;

                sum += diff;
                numNeighbors++;
            }
        }

        // That is, if this boid actually has neighbors to worry about
        if(numNeighbors > 0)
        {
            // Compute its average separation vector
            Vector3 avgDirection = sum / numNeighbors;
            avgDirection.Normalize();
            avgDirection *= maxSpeed;

            // Compute the steering force we need to apply
            Vector3 steeringForce = avgDirection - body.velocity;

            // Cap that steering force
            if (steeringForce.magnitude > maxForce)
            {
                steeringForce.Normalize();
                steeringForce *= maxForce;
            }

            // Apply steering force
            body.AddForce(steeringForce);
        }
    }


    /* Called each frame by the group manager. Aligns this boid with all other boids
     * in its immediate neighborhood.
     */
    public void Align()
    {
        // TODO
    }


    /* Called each frame by the group manager. Ensures this boid remains close to neighboring boids.
    */
    public void Cohere()
    {
        // TODO
    }


    /* For testing, to ensure there are no collisions.
     */
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.CompareTag("Boid"))
        {
            Debug.Log("Oopsie woopsie");
        }
    }
}
