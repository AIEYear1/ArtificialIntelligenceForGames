using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":

                break;
            case "AI":

                break;
        }

        Destroy(gameObject);
    }
}
