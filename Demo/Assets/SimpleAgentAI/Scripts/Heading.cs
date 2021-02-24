using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleAI
{
    public class Heading
    {
        public Transform transform;
        private Vector3 prevPos;

        Heading(Transform transform)
        {
            this.transform = transform;
            prevPos = transform.position;
        }

        public Vector3 GetHeading()
        {
            Vector3 toReturn = transform.position - prevPos;
            prevPos = transform.position;

            return toReturn;
        }

        public static implicit operator Heading(Transform transform)
        {
            return new Heading(transform);
        }
    }
}
