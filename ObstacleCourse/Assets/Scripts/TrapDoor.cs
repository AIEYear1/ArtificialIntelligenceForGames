using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    [SerializeField]
    float numOfTraps = 4;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float probabilityOfTrap = .1f;

    private void Start()
    {
        float curTraps = 0;
        for (int x = 0; curTraps < numOfTraps; ++x)
        {
            if (x >= transform.childCount)
                x = 0;

            if (!transform.GetChild(x).GetComponent<BoxCollider>().enabled || Random.value > probabilityOfTrap)
                continue;

            transform.GetChild(x).GetComponent<BoxCollider>().enabled = false;
            ++curTraps;
        }
    }
}
