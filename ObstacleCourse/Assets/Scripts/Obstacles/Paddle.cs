using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField]
    float speed = 30;
    [SerializeField]
    bool rotationFlip = false;
    [SerializeField]
    Timer flipTimer = new Timer();

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(transform.right, transform.up), speed);

        if (rotationFlip && flipTimer.Check())
            speed *= -1;
    }
}
