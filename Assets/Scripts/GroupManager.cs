using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupManager : MonoBehaviour
{
    public bool alignmentEnabled = false;
    public bool separationEnabled = false;
    public bool cohesionEnabled = false;

    public static int numVehicles = 150;
    public static List<GameObject> boids;
    public GameObject boidPrefab;

    private float minX = 200;
    private float minZ = 200;
    

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


    void FixedUpdate()
    {
        foreach(GameObject boid in boids)
        {
            if(alignmentEnabled)
            {
                boid.gameObject.GetComponent<Boid>().Align();
            }
            if(separationEnabled)
            {
                boid.gameObject.GetComponent<Boid>().Separate();
            }
            if(cohesionEnabled)
            {
                boid.gameObject.GetComponent<Boid>().Cohere();
            }
        }
    }
}
