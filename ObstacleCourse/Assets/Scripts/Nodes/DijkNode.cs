using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkNode : Node
{
    public DijkNode[] neighbors;

    public float baseGScore = 0;
    public float gScore = 0;
    public float hScore = 0;
    public float fScore { get => gScore + hScore; }

    public bool locked = false;

    public DijkNode prevNode = null;

    public void  SetScores(DijkNode targetNode)
    {
        gScore = baseGScore + ((prevNode == null) ? 0 : prevNode.gScore);

        hScore = Vector3.Distance(targetNode.transform.position, transform.position);
    }
}
