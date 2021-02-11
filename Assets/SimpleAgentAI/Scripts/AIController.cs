using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleAI
{
    [RequireComponent(typeof(Agent))]
    public class AIController : MonoBehaviour
    {
        public Transform target = null;
        public float speed = 3f;

        Agent agent;

        void Start()
        {
            agent = GetComponent<Agent>();
        }

        void Update()
        {
            Vector3 offset = target.position - transform.position;

            agent.Velocity = offset.normalized * speed;
            agent.UpdateMovement();
        }
    }
}
