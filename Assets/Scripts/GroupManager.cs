using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupManager : MonoBehaviour
{
    public bool alignmentEnabled = false;
    public bool separationEnabled = false;
    public bool cohesionEnabled = false;

    public float alignmentWeight = 1.0f;
    public float separationWeight = 2.0f;
    public float cohesionWeight = 1.5f;

    public static int numVehicles = 100;
    public static List<GameObject> boids;
    public GameObject boidPrefab;

    private float minX = 200;
    private float minZ = 200;
    
    
    /* Sets up all necessary info for the script. Main role is to spawn boids.
     */ 
    void Awake()
    {
        boids = new List<GameObject>(numVehicles);

        for(int i = 0; i < numVehicles; i++)
        {
            Vector3 position = new Vector3(Random.Range(-minX, minX), 5, Random.Range(-minZ, minZ));
            GameObject newBoid = Instantiate(boidPrefab, position, Quaternion.identity);
            boids.Add(newBoid);
        }        
    }
    

    /* Called by the physics engine. Update loop.
     */ 
    void FixedUpdate()
    {
        foreach(GameObject vehicle in boids)
        {
            Boid boid = vehicle.GetComponent<Boid>();
            Rigidbody body = boid.GetComponent<Rigidbody>();

            if(alignmentEnabled)
            {
                body.AddForce(boid.Align() * alignmentWeight);
            }

            if(separationEnabled)
            {
                body.AddForce(boid.Separate() * separationWeight);
            }

            if(cohesionEnabled)
            {
                body.AddForce(boid.Cohere() * cohesionWeight);
            }
        }
    }
}
