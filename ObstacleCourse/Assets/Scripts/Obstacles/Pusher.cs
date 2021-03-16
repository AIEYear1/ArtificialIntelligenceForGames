using UnityEngine;

public class Pusher : MonoBehaviour
{
    public Vector2 LerpVals = new Vector2(9, 13);
    public float timeToMove = 2f;
    public float moveDelay = 0.5f;
    public bool moveRight = true;
    public AudioClip[] sounds;
    public int soundIndex = 0;

    Vector3 pos = new Vector3();
    Timer mover = new Timer();

    AudioSource source = null;

    [HideInInspector]
    public Timer stall = new Timer();

    private void Start()
    {
        source = GetComponent<AudioSource>();

        source.clip = sounds[soundIndex];
        source.Play();

        pos = transform.position;
        mover = new Timer(timeToMove);
        stall = new Timer(moveDelay);
    }

    private void FixedUpdate()
    {
        if (moveRight) pos.x = Mathf.Lerp(LerpVals.x, LerpVals.y, mover.PercentComplete);
        else pos.x = Mathf.Lerp(LerpVals.y, LerpVals.x, mover.PercentComplete);

        if (mover.Check(false) && stall.Check())
        {
            if (++soundIndex == sounds.Length)
                soundIndex = 0;

            source.clip = sounds[soundIndex];
            source.Play();

            mover.Reset();
            moveRight = !moveRight;
        }

        transform.position = pos;
    }
}
