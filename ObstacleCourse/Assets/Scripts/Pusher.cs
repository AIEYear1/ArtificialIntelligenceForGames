using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    public Vector2 LerpVals = new Vector2(9, 13);
    public float timeToMove = 2f;
    public float moveDelay = 0.5f;
    public bool moveRight = true;

    Vector3 pos = new Vector3();
    Timer mover = new Timer();
    Timer stall = new Timer();

    private void Start()
    {
        pos = transform.position;
        mover = new Timer(timeToMove);
        stall = new Timer(moveDelay);
    }

    private void FixedUpdate()
    {
        if (moveRight) pos.x = Mathf.Lerp(LerpVals.x, LerpVals.y, mover.PercentComplete);
        else  pos.x = Mathf.Lerp(LerpVals.y, LerpVals.x, mover.PercentComplete);

        if (mover.Check(false) && stall.Check())
        {
            mover.Reset();
            moveRight = !moveRight;
        }

        transform.position = pos;
    }
}
