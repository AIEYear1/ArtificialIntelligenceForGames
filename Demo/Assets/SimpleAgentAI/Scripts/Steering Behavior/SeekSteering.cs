using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleAI
{
    public class SeekSteering : SteeringBehavior
    {
        public Transform target;

        public override Vector3 Steer(AISteeringController controller)
        {
            return (target.position - controller.transform.position).normalized * controller.maxSpeed;
        }
    }
}
