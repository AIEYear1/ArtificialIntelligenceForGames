using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    public Color beginner = new Color();
    public Color intermediate = new Color();
    public Color expert = new Color();

    public Agent ai = null;

    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void ChangeDifficulty()
    {
        ai.AlterDifficulty();

        button.image.color = (ai.difficulty != AIDifficulty.Beginner) ? (ai.difficulty != AIDifficulty.Intermediate) ? expert : intermediate : beginner;

        button.GetComponentInChildren<Text>().text = "Difficulty:\n" + ai.difficulty;
    }
}
