using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleAI
{
    [RequireComponent(typeof(Agent))]
    public class PlayerController : MonoBehaviour
    {
        public float speed = 3f;

        Agent agent;

        void Start()
        {
            agent = GetComponent<Agent>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            input = Vector3.ClampMagnitude(input, 1);
            agent.Velocity = input * speed;
            agent.UpdateMovement();
        }
    }
}
