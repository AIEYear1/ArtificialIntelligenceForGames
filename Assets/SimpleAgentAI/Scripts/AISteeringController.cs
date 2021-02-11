using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleAI
{
    [RequireComponent(typeof(Agent))]
    public class AISteeringController : MonoBehaviour
    {
        public Transform seekTarget = null;
        [Header("Steering Settings")]
        public float maxSpeed = 3f;
        public float maxForce = 5f;

        Agent agent;

        List<SteeringBehavior> steerings = new List<SteeringBehavior>();

        private void Awake()
        {
            agent = GetComponent<Agent>();

            steerings.Add(new SeekSteering { target = seekTarget });
        }

        private void Update()
        {
            Vector3 steeringForce = CalculateSteeringForce();

            agent.Velocity = Vector3.ClampMagnitude(agent.Velocity + steeringForce, maxSpeed);
            agent.UpdateMovement();
        }

        Vector3 CalculateSteeringForce()
        {
            Vector3 steeringForce = Vector3.zero;

            for (int x = 0; x < steerings.Count; ++x)
            {
                steeringForce += steerings[x].Steer(this);
            }

            steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);

            return steeringForce;
        }
    }
}
