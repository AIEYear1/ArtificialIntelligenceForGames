using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleAI
{
    [RequireComponent(typeof(Rigidbody))]
    public class Agent : MonoBehaviour
    {
        protected Rigidbody rb;

        protected Vector3 velocity;
        public virtual Vector3 Velocity 
        { 
            get => velocity; 
            set => velocity = value; 
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public virtual void UpdateMovement()
        {
            rb.MovePosition(rb.position + velocity * Time.deltaTime);
        }
    }

}
