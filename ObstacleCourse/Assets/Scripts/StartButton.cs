using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public UITween tweener = null;
    
    public GameObject startMenu = null;
    public GameObject mainOverlay = null;

    public GameObject startCamera = null;

    public void Begin()
    {
        tweener.Close(startMenu, startCamera, mainOverlay);

        Cursor.lockState = CursorLockMode.Locked;
    }
}
