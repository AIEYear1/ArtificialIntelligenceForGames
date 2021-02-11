using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleAI
{
    public class FleeSteering : SteeringBehavior
    {
        public Transform target;

        public override Vector3 Steer(AISteeringController controller)
        {
            return (controller.transform.position - target.position).normalized * controller.maxSpeed;
        }
    }
}
