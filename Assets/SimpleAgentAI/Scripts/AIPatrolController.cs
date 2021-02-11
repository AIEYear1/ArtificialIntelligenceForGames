using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleAI
{
    [RequireComponent(typeof(Agent))]
    public class AIPatrolController : MonoBehaviour
    {
        public Transform target = null;
        public float speed = 3f;
        public float playerDetectionDist = 5f;
        public float patrolPointStoppingDist = 2f;
        public Transform[] patrolPoints = new Transform[4];

        int patrolIndex = 0;

        Agent agent;

        void Start()
        {
            agent = GetComponent<Agent>();
        }

        void Update()
        {
            Vector3 offset = new Vector3();

            if (Vector3.Distance(target.position, transform.position) < playerDetectionDist)
            {
                offset = target.position - transform.position;

                agent.Velocity = offset.normalized * speed;
                agent.UpdateMovement();
                return;
            }
            offset = patrolPoints[patrolIndex].position - transform.position;

            agent.Velocity = offset.normalized * speed;
            agent.UpdateMovement();

            if (Vector3.Distance(patrolPoints[patrolIndex].position, transform.position) < patrolPointStoppingDist)
            {
                ++patrolIndex;

                if (patrolIndex == patrolPoints.Length)
                {
                    patrolIndex = 0;
                }
            }
        }
    }
}
