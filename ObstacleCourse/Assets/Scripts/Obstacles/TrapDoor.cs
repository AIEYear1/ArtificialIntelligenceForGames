using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    [SerializeField]
    NavNode startNode = null;
    [SerializeField]
    float numOfTraps = 4;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float probabilityOfTrap = .1f;

    List<BoxCollider> colliders = new List<BoxCollider>();

    private void Start()
    {
        float curTraps = 0;
        for (int x = 0; curTraps < numOfTraps; ++x)
        {
            if (x >= transform.childCount - 1)
                x = 0;

            if (!transform.GetChild(x).GetComponent<BoxCollider>().enabled || Random.value > probabilityOfTrap)
                continue;

            transform.GetChild(x).GetComponent<BoxCollider>().enabled = false;
            colliders.Add(transform.GetChild(x).GetComponent<BoxCollider>());
            ++curTraps;
        }

        Queue<Node> path = CalculatePath((DijkNode)startNode.nextNode[0], (DijkNode)startNode.nextNode[startNode.nextNode.Length - 1]);

        int attempts = 1;
        while (path == null)
        {
            print("bad gen");
            ++attempts;
            for (int x = 0; x < colliders.Count; ++x)
            {
                colliders[x].enabled = true;
            }
            colliders.Clear();
            curTraps = 0;
            for (int x = 0; curTraps < numOfTraps; ++x)
            {
                if (x >= transform.childCount - 1)
                    x = 0;

                if (!transform.GetChild(x).GetComponent<BoxCollider>().enabled || Random.value > probabilityOfTrap)
                    continue;

                transform.GetChild(x).GetComponent<BoxCollider>().enabled = false;
                colliders.Add(transform.GetChild(x).GetComponent<BoxCollider>());
                ++curTraps;
            }

            for (int x = 0; x < startNode.nextNode.Length; ++x)
            {
                (startNode.nextNode[x] as DijkNode).prevNode = null;
            }
            path = CalculatePath((DijkNode)startNode.nextNode[0], (DijkNode)startNode.nextNode[startNode.nextNode.Length - 1]);
        }

        print(attempts);
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
                if (Physics.Raycast(openList[0].neighbors[x].transform.position, Vector3.down, 2f) && !closedList.Contains(openList[0].neighbors[x]))
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
}
