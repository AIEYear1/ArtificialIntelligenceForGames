using UnityEngine;

public class Wrecker : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 10.0f)]
    float maxSpeed = 5.0f;
    [SerializeField]
    [Range(0.1f, 10.0f)]
    float minSpeed = 0.1f;
    [Space(10)]
    [SerializeField]
    float maxAngle = 40;
    [Space(10)]
    [SerializeField]
    bool goingTowardsEdge1 = true;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float soundStart = .3f;

    AudioSource source = null;

    float curSpeed = 0;

    bool hasHitZero = false;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        float angle = Vector3.Angle(transform.up, Vector3.up) / maxAngle;

        if (goingTowardsEdge1)
        {
            curSpeed = Mathf.Lerp(maxSpeed, minSpeed, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(transform.forward, transform.right), curSpeed);

            if (!hasHitZero)
                hasHitZero = angle < 1.0f;
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
        {
            if (!source.isPlaying && angle < soundStart)
            {
                source.Play();
            }
            hasHitZero = angle < .1f;
        }
        else if (angle >= 1)
        {
            goingTowardsEdge1 = true;
            hasHitZero = false;
        }
    }
}
