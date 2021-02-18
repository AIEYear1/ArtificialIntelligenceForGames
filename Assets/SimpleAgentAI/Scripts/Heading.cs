using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Vector3 toReturn = prevPos - transform.position;
        prevPos = transform.position;

        return toReturn;
    }

    public static implicit operator Heading(Transform transform)
    {
        return new Heading(transform);
    }
}
