using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public float winTime = 10f;

    private float timer;
    private bool gameOver;

    void Start()
    {
        if (messageText == null)
        {
            messageText = FindObjectOfType<TextMeshProUGUI>();
        }

        if (messageText != null)
        {
            messageText.text = "";
        }
        else
        {
            Debug.LogError("MessageText not found in scene");
        }
    }

    void Update()
    {
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return;
        }

        timer += Time.deltaTime;

        if (timer >= winTime)
        {
            Win();
        }
    }

    public void Lose()
    {
        gameOver = true;
        if (messageText != null)
            messageText.text = "You Lost! Press R to Restart";
    }

    void Win()
    {
        gameOver = true;
        if (messageText != null)
            messageText.text = "You Win! Press R to Restart";
    }
}
