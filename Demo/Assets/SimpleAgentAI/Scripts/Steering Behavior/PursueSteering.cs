using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleAI
{
    public class PursueSteering : SteeringBehavior
    {
        public Heading target;

        public override Vector3 Steer(AISteeringController controller)
        {
            float eta = Vector3.Distance(controller.transform.position, target.transform.position) / controller.maxSpeed;

            Vector3 toHeadOff = target.transform.position + ((target.GetHeading() / Time.deltaTime) * eta);
            Debug.Log(toHeadOff);
            return (toHeadOff - controller.transform.position).normalized * controller.maxSpeed;
        }
    }
}

