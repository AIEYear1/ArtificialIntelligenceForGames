using UnityEngine;

public class UITween : MonoBehaviour
{
    public GameObject[] elements;

    public float speed = .3f;
    public float delay = .5f;
    [Range(0.0f, 1.0f)]
    public float ratio = 0.8f;
    [Space(10)]
    public AudioClip mouseHover = null;
    public AudioClip mouseClick = null;
    public AudioSource source = null;
    public BackGroundMusic music = null;

    GameObject menu = null;
    GameObject cam = null;
    GameObject overlay = null;
    AudioListener playerListener = null;

    public void OnEnable()
    {
        for (int x = 0; x < elements.Length; ++x)
        {
            elements[x].transform.localScale = Vector3.zero;
        }

        for (int x = 0; x < elements.Length; ++x)
        {
            LeanTween.scale(elements[x], Vector3.one * 1.2f, speed * ratio).setDelay(delay * x);
            LeanTween.scale(elements[x], Vector3.one, speed * (1 - ratio)).setDelay((delay * x) + (speed * ratio));
        }
    }

    public void Close(GameObject relatedMenu, GameObject relatedCam, GameObject mainOverlay, AudioListener pListener)
    {

        menu = relatedMenu;
        cam = relatedCam;
        overlay = mainOverlay;
        playerListener = pListener;

        for (int x = 0; x < elements.Length; ++x)
        {
            if (x == 0)
                LeanTween.scale(elements[x], Vector3.zero, speed).setDelay((delay / 2) * (elements.Length - 1 - x)).setOnComplete(End);
            else
                LeanTween.scale(elements[x], Vector3.zero, speed).setDelay((delay / 2) * (elements.Length - 1 - x));
        }
    }
    public void End()
    {
        playerListener.enabled = true;
        music.ChangeSong();
        menu.SetActive(false);
        cam.SetActive(false);
        overlay.SetActive(true);
    }

    public void MouseEnter(GameObject obj)
    {
        source.clip = mouseHover;
        source.Play();
        LeanTween.scale(obj, Vector3.one * 1.1f, 0.1f);
    }
    public void MouseExit(GameObject obj)
    {
        LeanTween.scale(obj, Vector3.one, 0.1f);
    }

    public void MouseClick()
    {
        source.clip = mouseClick;
        source.Play();
    }
}
