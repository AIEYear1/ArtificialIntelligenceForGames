using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCountDown : MonoBehaviour
{
    public Player player = null;
    public Agent ai = null;

    public GameObject crosshair = null;

    Text text;

    int index = 3;

    private void OnEnable()
    {
        text = GetComponent<Text>();

        text.text = index.ToString();

        LeanTween.scale(gameObject, Vector2.one * .4f, 1f).setOnComplete(UpdateNumber);
    }
    public void UpdateNumber()
    {
        transform.localScale = Vector3.one;

        --index;

        if(index <= 0)
        {
            crosshair.SetActive(true);
            player.locked = false;
            ai.curState = Agent.State.MOVETOOBSTACLE;
            Destroy(gameObject);
            return;
        }

        text.text = index.ToString();

        LeanTween.scale(gameObject, Vector2.one * .3f, 1f).setOnComplete(UpdateNumber);
    }
}
