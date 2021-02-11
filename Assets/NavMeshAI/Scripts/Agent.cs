using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public Transform target = null;
    [Space(10)]
    public bool isHostile = true;
    [Space(10)]
    public float stoppingDistance = 1;
    public float range = 1.5f;
    public Timer attackDelay = new Timer(1);

    NavMeshAgent agent = null;

    float dist = float.MaxValue;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        attackDelay.Reset(attackDelay.Delay);
    }

    void Update()
    {
        Move();
        Attack();
    }

    /// <summary>
    /// Manages agent attacking
    /// </summary>
    void Attack()
    {
        if (isHostile && (attackDelay.Check(false) && dist < range))
        {
            attackDelay.Reset();
            print("ATTACK!");
        }
    }

    /// <summary>
    /// Manages agent movement
    /// </summary>
    void Move()
    {
        dist = Vector3.Distance(transform.position, target.position);

        if (dist > stoppingDistance)
        {
            agent.destination = target.position;
            return;
        }

        if (dist <= stoppingDistance)
        {
            agent.destination = target.position + ((transform.position - target.position).normalized * stoppingDistance);
            return;
        }
    }

    // Idon't know why this is here but it is now
    void BubbleV2()
    {
        string output = "";

        int[] randomMagic = new int[10];

        for (int x = 0; x < 10; x++)
            randomMagic[x] = Random.Range(1, 120);

        for (int x = 0; x < 10; x++)
            output += randomMagic[x] + "\n";

        output += "\n\n";

        bool swapped = true;
        int iteration = 0;

        while (swapped)
        {
            swapped = false;

            for (int x = 0; x < randomMagic.Length - iteration - 1; x++)
            {
                if (randomMagic[x] > randomMagic[x + 1])
                {
                    int tmpItem = randomMagic[x + 1];
                    randomMagic[x + 1] = randomMagic[x];
                    randomMagic[x] = tmpItem;

                    swapped = true;
                }
            }

            if (!swapped)
                break;

            swapped = false;

            for (int x = randomMagic.Length - 1; x > 0 + iteration; x--)
            {
                if (randomMagic[x] < randomMagic[x - 1])
                {
                    int tmpItem = randomMagic[x - 1];
                    randomMagic[x - 1] = randomMagic[x];
                    randomMagic[x] = tmpItem;

                    swapped = true;
                }
            }

            iteration++;
        }

        for (int x = 0; x < 10; x++)
            output += randomMagic[x] + "\n";

        print(output);
    }
}
