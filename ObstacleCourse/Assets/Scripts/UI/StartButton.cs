using UnityEngine;

public class StartButton : MonoBehaviour
{
    public UITween tweener = null;

    public GameObject startMenu = null;
    public GameObject mainOverlay = null;

    public GameObject startCamera = null;

    public AudioListener playerListener = null;

    public void Begin()
    {
        tweener.Close(startMenu, startCamera, mainOverlay, playerListener);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
