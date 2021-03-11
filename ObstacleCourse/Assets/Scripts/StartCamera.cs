using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCamera : MonoBehaviour
{
    public Transform center = null;
    public float semiMajor = 84f;
    public float semiMinor = 40f;
    public float speed = 10f;

    float alpha = 0f;

    private void FixedUpdate()
    {
        transform.position = new Vector3(center.position.x + (semiMajor * Mathf.Sin(Mathf.Deg2Rad * alpha)), 
                                         transform.position.y, 
                                         center.position.z + (semiMinor * Mathf.Cos(Mathf.Deg2Rad * alpha)));

        transform.forward = center.position - transform.position;

        alpha += speed * Time.deltaTime;
    }
}
