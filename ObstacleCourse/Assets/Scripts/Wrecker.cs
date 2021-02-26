using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrecker : MonoBehaviour
{
    [SerializeField]
    float maxSpeed = 30f;
    [SerializeField]
    float minSpeed = 2f;
    [Space(10)]
    public float maxAngle = 0;
    [Space(10)]
    [SerializeField]
    bool goingTowardsEdge1 = true;

    public float curSpeed = 0;

    bool hasHitZero = false;


    private void FixedUpdate()
    {
        float angle = Vector3.Angle(transform.up, Vector3.up) / maxAngle;
        print(angle);
        // TODO: Needs Fixing
        if (goingTowardsEdge1)
        {
            curSpeed = Mathf.Lerp(maxSpeed, minSpeed, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(transform.forward, transform.right), curSpeed);

            if (!hasHitZero)
                hasHitZero = angle < .1f;
            else if (angle >= 1)
            {
                goingTowardsEdge1 = false;
                hasHitZero = false;
            }

            return;
        }
        curSpeed = -Mathf.Lerp(maxSpeed, minSpeed, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(transform.forward, transform.right), curSpeed);

        if (!hasHitZero)
            hasHitZero = angle < .1f;
        else if (angle >= 1)
        {
            goingTowardsEdge1 = true;
            hasHitZero = false;
        }
    }
}
