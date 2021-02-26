using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public enum State { STOP, MOVE, JUMP }

    #region Variables
    public Transform[] target;
    [Space(10)]
    public float stoppingDistance = 1f;
    [Space(10)]
    public float jumpSpread = 0f;
    public float gravity = 20f;

    NavMeshAgent agent = null;
    Rigidbody rb = null;

    float dist = float.MaxValue;
    public int curTarget = 0;

    State curState = State.MOVE;

    bool jumpStarted = false;
    Vector3 vel = new Vector3();
    #endregion

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        switch (curState)
        {
            case State.STOP:
                break;
            case State.MOVE:
                Move();
                break;
            case State.JUMP:
                Jump();
                break;
        }
    }

    void Move()
    {
        dist = Vector3.Distance(transform.position, target[curTarget].position);

        if (dist > stoppingDistance)
        {
            agent.destination = target[curTarget].position;

            if(agent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                --curTarget;
                if (curTarget < 0)
                    curTarget = 0;
            }

            return;
        }

        if (dist <= stoppingDistance)
        {
            agent.destination = target[curTarget].position + ((transform.position - target[curTarget].position).normalized * stoppingDistance);

            if (curTarget == target.Length-1)
            {
                // TODO: GAMEOVER HERE

                agent.isStopped = true;
                curState = State.STOP;

                return;
            }

            agent.enabled = false;
            curState = State.JUMP;
        }
    }

    void Jump()
    {
        if (!jumpStarted)
        {
            vel = target[curTarget].GetComponent<Node>().jumpDirection;

            if(jumpSpread != 0)
                vel += new Vector3(Random.Range(-1f, 1f), -.5f * Random.value, -.5f * Random.value) * jumpSpread;

            vel = target[curTarget].rotation * vel;

        }

        vel.y -= gravity * Time.deltaTime;
        rb.velocity = vel;

        if (jumpStarted && IsGrounded())
        {
            jumpStarted = false;
            rb.velocity = Vector3.zero;

            agent.enabled = true;

            ++curTarget;

            //NavMeshPath path = new NavMeshPath();
            //NavMesh.CalculatePath(transform.position, target[curTarget].position, agent.areaMask, path);
            //print(path.status);
            //while (path.status != NavMeshPathStatus.PathComplete)
            //{
            //    --curTarget;

            //    if (curTarget < 0)
            //    {
            //        curTarget = 0;
            //        break;
            //    }

            //    NavMesh.CalculatePath(transform.position, target[curTarget].position, agent.areaMask, path);
            //}

            curState = State.MOVE;
            return;
        }
        jumpStarted = true;
    }

    /// <summary>
    /// Determines if the player is standing on something
    /// </summary>
    /// <returns>returns true if the player is standing on something</returns>
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, agent.height / 1.9f);
    }
}
