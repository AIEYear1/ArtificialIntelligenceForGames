using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
// TODO: Code up everything for Paddle puzzle, likely use Navmesh calc path then rigid body move across that path or use object nodes
public enum AIDifficulty { Beginner, Intermediate, Expert }
public class Agent : MonoBehaviour
{
    public enum State { STOP, MOVETOOBSTACLE, JUMP, SKINNY, PUSHER, PADDLE, TRAPDOOR, WRECKER }

    #region Variables
    public AIDifficulty difficulty = AIDifficulty.Beginner;
    // Array with nodes for the start of each obstacle
    public Node[] baseNodes;
    [Space(10)]
    public float stoppingDistance = 1f;
    [Space(10)]
    public float speed = 10f;
    public float jumpForce = 7f;
    public float imperfection = 0f;
    public float gravity = 20f;

    Queue<Node> path = new Queue<Node>();

    Node target = null;

    NavMeshAgent agent = null;
    Rigidbody rb = null;

    float dist = float.MaxValue;
    int curObstacle = 0;

    public State curState = State.MOVETOOBSTACLE;

    Vector3 vel = new Vector3();

    public bool obstacleBool = false;
    Transform obstacleObj = null;
    Timer obstacleTimer = new Timer();
    #endregion

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        target = baseNodes[curObstacle];
    }

    void Update()
    {
        switch (curState)
        {
            case State.STOP:
                break;
            case State.MOVETOOBSTACLE:
                MoveToObstacle();
                break;
            case State.JUMP:
                Jump();
                break;
            case State.SKINNY:
                Skinny();
                break;
            case State.PUSHER:
                Pusher();
                break;
            case State.PADDLE:
                Paddle();
                break;
            case State.TRAPDOOR:
                TrapDoor();
                break;
            case State.WRECKER:
                Wrecker();
                break;
        }
    }

    void MoveToObstacle()
    {
        dist = Vector3.Distance(transform.position, target.transform.position);

        if (dist > stoppingDistance)
        {
            agent.destination = target.transform.position;

            //if(agent.pathStatus != NavMeshPathStatus.PathComplete)
            //{
            //    --curObstacle;
            //    if (curObstacle < 0)
            //        curObstacle = 0;
            //}
        }
        else
        {
            agent.destination = target.transform.position + ((transform.position - target.transform.position).normalized * stoppingDistance);

            if (curObstacle == baseNodes.Length - 1)
            {
                agent.isStopped = true;
                curState = State.STOP;

                return;
            }

            agent.enabled = false;
            rb.isKinematic = false;
            curState = (State)(curObstacle + 2);
            StartObstacle();
        }
    }

    void Jump()
    {
        print(obstacleBool);
        // only apply set jump velocity when obstacle bool is false
        if (!obstacleBool && obstacleTimer.Check(false))
        {
            float jForceMod = Vector3.Distance(transform.position, target.transform.position) / (speed * 0.65f);

            vel = new Vector3(Random.Range(-1f, 1f) * imperfection, jumpForce, speed * jForceMod);

            vel = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up) * vel;

        }

        if (IsGrounded())
        {
            if (transform.position.y < -1)
            {
                print("Jump failed");

                obstacleBool = false;
                rb.velocity = Vector3.zero;

                target = baseNodes[curObstacle];
                rb.isKinematic = true;
                agent.enabled = true;
                curState = State.MOVETOOBSTACLE;
                return;
            }

            if (obstacleBool)
            {
                obstacleBool = false;
                obstacleTimer.Reset();
                vel = Vector3.zero;
                rb.velocity = vel;

                if (path.Count == 0)
                {
                    print("Jump finished");

                    target = baseNodes[++curObstacle];
                    rb.isKinematic = true;
                    agent.enabled = true;
                    curState = State.MOVETOOBSTACLE;
                    return;
                }

                target = (DijkNode)path.Dequeue();

                return;
            }
        }

        obstacleBool = obstacleBool || (obstacleTimer.IsComplete(false) && !IsGrounded());

        vel.y -= gravity * Time.deltaTime;
        rb.velocity = vel;
    }

    void Skinny()
    {
        dist = Vector3.Distance(transform.position, target.transform.position);

        if (IsGrounded())
        {
            if (transform.position.y < -1)
            {
                print("Skinny failed");

                rb.velocity = Vector3.zero;
                target = baseNodes[curObstacle];
                rb.isKinematic = true;
                agent.enabled = true;
                curState = State.MOVETOOBSTACLE;
                return;
            }

            if (dist > stoppingDistance)
            {
                vel = new Vector3(Random.Range(-.5f, .5f) * imperfection, 0, speed * Mathf.Clamp(dist / (speed * .3f), stoppingDistance, 1f));

                vel = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up) * vel;
            }
            else
            {
                vel = Vector3.zero;

                if (path.Count == 0)
                {
                    print("Skinny finished");

                    rb.velocity = vel;

                    target = baseNodes[++curObstacle];
                    rb.isKinematic = true;
                    agent.enabled = true;
                    curState = State.MOVETOOBSTACLE;
                    return;
                }

                target = path.Dequeue();
            }
        }
        else
        {
            vel.y -= gravity * Time.deltaTime;
        }

        rb.velocity = vel;
    }

    void Pusher()
    {
        // only move when obstaclebool is true
        obstacleBool = !(target is ObjectNode node) || (Mathf.Abs(node.obj.position.x) == 7 && node.obj.GetComponent<Pusher>().stall.PercentComplete > .8f) | obstacleBool;
        print(obstacleBool);
        if (IsGrounded())
        {
            if (transform.position.y < -1)
            {
                print("Pusher failed");

                obstacleObj = null;

                rb.velocity = Vector3.zero;
                target = baseNodes[curObstacle];
                rb.isKinematic = true;
                agent.enabled = true;
                curState = State.MOVETOOBSTACLE;
                return;
            }

            if (obstacleBool)
            {
                vel = target.transform.position - ((obstacleObj == null) ? transform.position : obstacleObj.position);

                vel.Normalize();
                vel *= speed;

                if ((!(target is ObjectNode) && Vector3.Distance(target.transform.position, transform.position) < stoppingDistance) || target.transform.position.z < transform.position.z + stoppingDistance)
                {
                    vel = Vector3.zero;
                    obstacleBool = false;

                    if (path.Count == 0)
                    {
                        print("Pusher finished");

                        rb.velocity = vel;

                        target = baseNodes[++curObstacle];
                        rb.isKinematic = true;
                        agent.enabled = true;
                        curState = State.MOVETOOBSTACLE;
                        return;
                    }

                    obstacleObj = target.transform;
                    target = path.Dequeue();
                }
            }
            else
            {
                vel = Vector3.zero;
            }
        }
        else
        {
            vel.y -= gravity * Time.deltaTime;
        }

        rb.velocity = vel;
    }

    void Paddle()
    {
        if (IsGrounded())
        {
            if (transform.position.y < -1)
            {
                print("Paddle failed");

                rb.velocity = Vector3.zero;
                target = baseNodes[curObstacle];
                rb.isKinematic = true;
                agent.enabled = true;
                curState = State.MOVETOOBSTACLE;
                return;
            }

            if (Vector3.Distance(transform.position, target.transform.position) > stoppingDistance)
            {
                NavMeshPath navPath = new NavMeshPath();
                if (NavMesh.CalculatePath(transform.position, target.transform.position, agent.areaMask, navPath))
                {
                    vel = navPath.corners[1] - transform.position;
                    vel.Normalize();
                    vel *= speed;
                }
                else
                {
                    vel = Vector3.zero;
                }
            }
            else
            {
                print("Paddle finished");

                vel = Vector3.zero;

                rb.velocity = vel;

                target = baseNodes[++curObstacle];
                rb.isKinematic = true;
                agent.enabled = true;
                curState = State.MOVETOOBSTACLE;
                return;
            }
        }
        else
        {
            vel.y -= gravity * Time.deltaTime;
        }

        rb.velocity = vel;
    }

    void TrapDoor()
    {
        dist = Vector3.Distance(transform.position, target.transform.position);

        if (IsGrounded())
        {
            if (transform.position.y < -1)
            {
                print("Trap failed");

                rb.velocity = Vector3.zero;
                target = baseNodes[curObstacle];
                rb.isKinematic = true;
                agent.enabled = true;
                curState = State.MOVETOOBSTACLE;
                return;
            }

            if (dist > stoppingDistance)
            {
                vel = target.transform.position - transform.position;
                vel.Normalize();
                vel *= speed;
            }
            else
            {
                vel = Vector3.zero;

                if (path.Count == 0)
                {
                    print("Trap finished");

                    rb.velocity = vel;

                    target = baseNodes[++curObstacle];
                    rb.isKinematic = true;
                    agent.enabled = true;
                    curState = State.MOVETOOBSTACLE;
                    return;
                }

                target = path.Dequeue();
            }
        }
        else
        {
            vel.y -= gravity * Time.deltaTime;

            if (transform.position.y < -1 && Random.value < .5f + (.5f * ((float)difficulty / 2f)))
            {
                (target as DijkNode).locked = true;
            }
        }

        rb.velocity = vel;
    }

    void Wrecker()
    {
        dist = Vector3.Distance(transform.position, target.transform.position);
        // only move when obstaclebool is true
        obstacleBool = !(target is ObjectNode node) || (Mathf.Abs(node.obj.rotation.z) < 7) | obstacleBool;
        print(obstacleBool);
        if (IsGrounded())
        {
            if (transform.position.y < -1)
            {
                print("Wrecker failed");

                rb.velocity = Vector3.zero;
                target = baseNodes[curObstacle];
                rb.isKinematic = true;
                agent.enabled = true;
                curState = State.MOVETOOBSTACLE;
                return;
            }

            if (obstacleBool)
            {
                if (dist > stoppingDistance)
                {
                    vel = new Vector3(Random.Range(-.25f, .25f) * imperfection, 0, speed * Mathf.Clamp(dist / (speed * .3f), stoppingDistance, 1f));

                    vel = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up) * vel;
                }
                else
                {
                    vel = Vector3.zero;
                    obstacleBool = false;

                    if (path.Count == 0)
                    {
                        print("Pusher finished");

                        rb.velocity = vel;

                        target = baseNodes[++curObstacle];
                        rb.isKinematic = true;
                        agent.enabled = true;
                        curState = State.MOVETOOBSTACLE;
                        return;
                    }

                    obstacleObj = target.transform;
                    target = path.Dequeue();
                }
            }
            else
            {
                vel = Vector3.zero;
            }
        }
        else
        {
            vel.y -= gravity * Time.deltaTime;
        }

        rb.velocity = vel;
    }

    void StartObstacle()
    {
        NavNode node = (NavNode)baseNodes[curObstacle];
        bool takeExpert = DifficultyCheck();
        switch (curObstacle)
        {
            case 0: // Jump
                path = CalculatePath((DijkNode)node.nextNode[0], (DijkNode)node.nextNode[node.nextNode.Length - 1], takeExpert);
                obstacleTimer = new Timer(0.25f * (2 - (int)difficulty));
                vel = Vector3.zero;
                break;
            case 1: // Skinny
                path = new Queue<Node>();
                for (int x = 0; x < node.nextNode.Length; ++x)
                {
                    path.Enqueue(node.nextNode[x]);
                }
                break;
            case 2: // Pusher
                path = new Queue<Node>();
                if (takeExpert)
                {
                    for (int x = 5; x < node.nextNode.Length; ++x)
                    {
                        path.Enqueue(node.nextNode[x]);
                    }

                    break;
                }

                for (int x = 0; x < 5; ++x)
                {
                    path.Enqueue(node.nextNode[x]);
                }
                break;
            case 3: // Paddle
                path.Enqueue(node.nextNode[0]);
                break;
            case 4: // Trap
                path = CalculatePath((DijkNode)node.nextNode[0], (DijkNode)node.nextNode[node.nextNode.Length - 1]);
                break;
            case 5: // Wrecker
                path = new Queue<Node>();
                for (int x = 0; x < node.nextNode.Length; ++x)
                {
                    path.Enqueue(node.nextNode[x]);
                }
                break;
        }

        target = path.Dequeue();
    }

    Queue<Node> CalculatePath(DijkNode startNode, DijkNode targetNode, bool calcShortest = true, bool useAStar = true)
    {
        List<DijkNode> openList = new List<DijkNode>();
        List<DijkNode> closedList = new List<DijkNode>();

        openList.Add(startNode);
        openList[0].SetScores(targetNode);

        while (openList.Count != 0)
        {
            if (openList[0] == targetNode)
            {
                break;
            }

            for (int x = 0; x < openList[0].neighbors.Length; ++x)
            {
                if (!openList[0].neighbors[x].locked && !closedList.Contains(openList[0].neighbors[x]))
                {
                    if (openList.Contains(openList[0].neighbors[x]))
                    {
                        float tmpInt = openList[0].gScore + openList[0].neighbors[x].baseGScore;

                        if (calcShortest && tmpInt < openList[0].neighbors[x].gScore)
                        {
                            int y = openList.IndexOf(openList[0].neighbors[x]);
                            openList[y].prevNode = openList[0];
                            openList[y].SetScores(targetNode);
                        }
                        else if (!calcShortest && tmpInt > openList[0].neighbors[x].gScore)
                        {
                            int y = openList.IndexOf(openList[0].neighbors[x]);
                            openList[y].prevNode = openList[0];
                            openList[y].SetScores(targetNode);
                        }

                        continue;
                    }

                    openList[0].neighbors[x].prevNode = openList[0];
                    openList[0].neighbors[x].SetScores(targetNode);
                    openList.Add(openList[0].neighbors[x]);
                }
            }

            closedList.Add(openList[0]);
            openList.RemoveAt(0);

            if (useAStar)
            {
                if (calcShortest)
                    openList.Sort((node1, node2) => node1.fScore.CompareTo(node2.fScore));
                else
                    openList.Sort((node1, node2) => node2.fScore.CompareTo(node1.fScore));
            }
            else
            {
                if (calcShortest)
                    openList.Sort((node1, node2) => node1.gScore.CompareTo(node2.gScore));
                else
                    openList.Sort((node1, node2) => node2.gScore.CompareTo(node1.gScore));
            }
        }

        if (targetNode.prevNode == null)
        {
            curState = State.STOP;
            return null;
        }

        List<DijkNode> tmpPath = new List<DijkNode>();
        Queue<Node> finPath = new Queue<Node>();

        tmpPath.Add(targetNode);

        while (tmpPath[0].prevNode != null)
        {
            tmpPath.Insert(0, tmpPath[0].prevNode);
        }
        for (int x = 0; x < tmpPath.Count; ++x)
        {
            finPath.Enqueue(tmpPath[x]);
        }

        return finPath;
    }

    bool DifficultyCheck()
    {
        return difficulty == AIDifficulty.Expert || (difficulty == AIDifficulty.Intermediate && Random.value >= 0.5f);
    }

    /// <summary>
    /// Determines if the player is standing on something
    /// </summary>
    /// <returns>returns true if the player is standing on something</returns>
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, agent.height / 1.9f);
    }

    public void AlterDifficulty()
    {
        switch (difficulty)
        {
            case AIDifficulty.Beginner:
                difficulty = AIDifficulty.Intermediate;
                speed = 9f;
                agent.speed = 9f;
                imperfection = 2.5f;
                break;
            case AIDifficulty.Intermediate:
                difficulty = AIDifficulty.Expert;
                speed = 10f;
                agent.speed = 10f;
                imperfection = 2f;
                break;
            case AIDifficulty.Expert:
                difficulty = AIDifficulty.Beginner;
                speed = 8f;
                agent.speed = 8f;
                imperfection = 2.6f;
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (curState != State.MOVETOOBSTACLE && curState != State.STOP)
        {
            Queue<Node> tmpQueue = new Queue<Node>(path);
            Vector3 targ = target.transform.position;

            Debug.DrawLine(transform.position, target.transform.position, Color.magenta);

            while (tmpQueue.Count != 0)
            {
                Debug.DrawLine(targ, tmpQueue.Peek().transform.position, Color.magenta);
                targ = tmpQueue.Dequeue().transform.position;
            }
        }
    }
}
