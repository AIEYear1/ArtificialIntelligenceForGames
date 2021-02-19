using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockSystem : MonoBehaviour
{
    public int boidCount = 100;
    public float boidSpawnRadius = 20f;
    public float neighborhoodRadius = 5f;
    public GameObject boidPrefab = null;
    public float mass = 3f;
    public float maxForce = 50f;
    public float maxSpeed = 20f;
    [Space(10)]
    public float seekForce = 1f;
    public float separationForce = 50f;
    public float alignmentForce = 50f;
    public float cohesionForce = 50f;

    private Transform[] boidTransform;

    private Vector3[] boidPosition;

    private Vector3[] boidVelocity;

    private void Start()
    {
        boidTransform = new Transform[boidCount];
        boidPosition = new Vector3[boidCount];
        boidVelocity = new Vector3[boidCount];

        for(int x = 0; x < boidCount; ++x)
        {
            boidTransform[x] = Instantiate(boidPrefab, transform.position + Random.insideUnitSphere * boidSpawnRadius, Quaternion.Euler(Random.insideUnitSphere * 360)).transform;
            boidPosition[x] = boidTransform[x].position;
            boidVelocity[x] = boidTransform[x].forward;
        }
    }

    private void Update()
    {
        for(int x = 0; x < boidCount; ++x)
        {
            Vector3 sumForces = CalculateForces(boidTransform[x]);

            sumForces = Vector3.ClampMagnitude(sumForces, maxForce);

            sumForces /= mass;

            boidVelocity[x] = boidVelocity[x] + sumForces * Time.deltaTime;
            boidVelocity[x] = Vector3.ClampMagnitude(boidVelocity[x], maxSpeed);


            boidPosition[x] += boidVelocity[x] * Time.deltaTime;

            boidTransform[x].position = boidPosition[x];
            boidTransform[x].forward = boidVelocity[x];
        }
    }

    private Vector3 CalculateForces(Transform boid)
    {

        Vector3 sepForce = Vector3.zero;        // Separation
        Vector3 alignForce = Vector3.zero;      // Alignment
        Vector3 cohesForce = Vector3.zero;      // Cohesion

        int nearbyBoidCount = 0;                // Counter for cohesion

        for (int x = 0; x < boidCount; ++x)
        {
            if (boidTransform[x] != boid && Vector3.Distance(boid.position, boidTransform[x].position) < neighborhoodRadius)
            {
                // Flee from nearby boids
                sepForce += boid.position - boidTransform[x].position;
                // Align forward with nearby boids
                alignForce += boidTransform[x].forward;
                // Fit into the center of nearby boids
                cohesForce += boidTransform[x].position;

                // Count up number of nearby boids for cohesion math
                ++nearbyBoidCount;
            }
        }
        // divide by number of nearby boids to calculate average position of nearby vectors or center
        cohesForce /= (nearbyBoidCount != 0) ? nearbyBoidCount : 1;

        Vector3 toReturn = (transform.position - boid.position) * seekForce;    // Send in the direction of flock center
        toReturn += sepForce * separationForce;                                 // Send in direction where nearby boids aren't
        toReturn += alignForce * alignmentForce;                                // Send in the same direction of travel of nearby boids
        toReturn += (cohesForce - boid.position) * cohesionForce;               // Send in the direction of the center of the nearby boids

        return toReturn;
    }
}
