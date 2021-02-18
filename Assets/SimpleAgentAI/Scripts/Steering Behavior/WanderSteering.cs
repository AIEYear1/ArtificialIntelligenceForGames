using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleAI
{
    public class WanderSteering : SteeringBehavior
    {
        public Heading target;
        public float radius;
        public float jitter;
        public float headingDistance;

        public override Vector3 Steer(AISteeringController controller)
        {
            Vector2 wander = Random.insideUnitCircle.normalized * radius;
            Vector3 displacement = new Vector3(wander.x, 0, wander.y);
            //Vector3 displacement = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * radius;

            displacement += (target.transform.position - displacement).normalized * jitter;
            displacement.Normalize();
            displacement *= radius;

            displacement += target.GetHeading().normalized * headingDistance;

            return (displacement - controller.transform.position).normalized * controller.maxSpeed;
        }
    }
}
