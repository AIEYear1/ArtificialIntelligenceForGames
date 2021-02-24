using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleAI
{
    public class SteeringBehavior
    {
        public virtual Vector3 Steer(AISteeringController controller)
        {
            return Vector3.zero;
        }
    }
}
