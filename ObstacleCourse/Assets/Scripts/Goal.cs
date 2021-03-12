using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    public GameObject endCamera = null;
    public GameObject endMenu = null;
    public GameObject mainOverlay = null;
    public Text endText = null;

    private void OnTriggerEnter(Collider other)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        endText.text = other.name + " Wins";

        mainOverlay.SetActive(false);
        endMenu.SetActive(true);
        endCamera.SetActive(true);

        Destroy(gameObject);
    }
}
