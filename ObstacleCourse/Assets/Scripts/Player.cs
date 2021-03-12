using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform head = null;
    [Space(10)]
    public float speed = 5;
    public float jForce = 7;
    public float gravity = 20;
    public float height = 2;
    [Space(10)]
    public float mouseSensitivity = 3;
    public float LookSpeed = .01f;
    public float maximumY = 70, minimumY = -70;
    public float smoothing = 2.0f;
    [Space(10)]

    public bool locked = false;

    Rigidbody rb;
    Vector3 vel = new Vector3();

    Vector3 smoothV, mouseLook;
    Vector2 md = new Vector2();

    bool grounded = false;
    bool jump = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        grounded = IsGrounded();
        jump = grounded && (Input.GetKeyDown(KeyCode.Space) | jump);
        //print(Vector3.Angle(Vector3.forward, transform.forward));
    }

    void FixedUpdate()
    {
        if (locked)
            return;

        // get mouse input
        md.x = Input.GetAxisRaw("Mouse X");
        md.y = Input.GetAxisRaw("Mouse Y");
        md = Vector2.Scale(md, Vector2.one * mouseSensitivity * smoothing);

        // smooth input
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        // incrementally add to the camera look
        mouseLook += smoothV * LookSpeed;

        // lock mouse look on the y access
        if (mouseLook.y > maximumY) mouseLook.y = maximumY;
        if (mouseLook.y < minimumY) mouseLook.y = minimumY;

        // get wasd / arrow keys input
        vel.x = Input.GetAxis("Horizontal");
        vel.z = Input.GetAxis("Vertical");
        vel.y = 0;

        // convert simple keyboard input to actual in-game movement
        vel = transform.rotation * (Vector3.ClampMagnitude(vel, 1) * speed);

        // maintain vertical velocity from rigidbody
        vel.y = rb.velocity.y;

        // if grounded and space key pressed set vertical velocity to jump force if not grounded accelerate downwards at the speed of gravity
        if (jump)
        {
            jump = false;
            vel.y = jForce;
        }
        else if (!grounded)
        {
            vel.y -= gravity * Time.deltaTime;
        }

        // set player velocity
        rb.velocity = vel;

        // set player rotation, y rotates head, x rotates entire player body
        head.transform.localRotation = Quaternion.Euler(-Quaternion.AngleAxis(mouseLook.y, Vector3.up).eulerAngles.y, 0, 0);
        transform.localRotation = Quaternion.Euler(0, mouseLook.x, 0);
    }

    /// <summary>
    /// Determines if the player is standing on something
    /// </summary>
    /// <returns>returns true if the player is standing on something</returns>
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, height / 1.9f);
    }
}
