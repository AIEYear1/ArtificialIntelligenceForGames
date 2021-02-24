using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleAI
{
    public class EvadeSteering : SteeringBehavior
    {
        public Heading target;

        public override Vector3 Steer(AISteeringController controller)
        {
            return Vector3.Cross(target.GetHeading(), Vector3.up).normalized * controller.maxSpeed;
        }
    }
}
