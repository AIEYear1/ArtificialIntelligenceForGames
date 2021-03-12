using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITween : MonoBehaviour
{
    public GameObject[] elements;

    public float speed = .3f;
    public float delay = .5f;

    GameObject menu = null;
    GameObject cam = null;
    GameObject overlay = null;

    public void OnEnable()
    {
        for (int x = 0; x < elements.Length; ++x)
        {
            elements[x].transform.localScale = Vector3.zero;
        }

        for (int x = 0; x < elements.Length; ++x)
        {
            LeanTween.scale(elements[x], Vector3.one, speed).setDelay(delay * x);
        }
    }

    public void Close(GameObject relatedMenu, GameObject relatedCam, GameObject mainOverlay)
    {
        menu = relatedMenu;
        cam = relatedCam;
        overlay = mainOverlay;

        print(elements.Length - 1);
        for (int x = 0; x < elements.Length; ++x)
        {
            if(x == 0)
                LeanTween.scale(elements[x], Vector3.zero, speed).setDelay(delay * (elements.Length - 1 - x)).setOnComplete(End);
            else
                LeanTween.scale(elements[x], Vector3.zero, speed).setDelay(delay * (elements.Length - 1 - x));
        }
    }
    public void End()
    {
        menu.SetActive(false);
        cam.SetActive(false);
        overlay.SetActive(true);
    }
}
