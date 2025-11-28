using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    public TMP_Text messageText;
    public TMP_Text livesText;

    [Header("Game Settings")]
    public float winTime = 10f;
    public int maxLives = 3;

    private float timer;
    private int lives;
    private GameState state = GameState.StartScreen;

    public bool IsGameOver => state == GameState.GameOver;
    public bool IsRunning => state == GameState.Playing;
    public int Lives => lives;

    public event Action<int> LivesChanged;

    private enum GameState
    {
        StartScreen,
        Playing,
        Paused,
        LifeLost,
        GameOver
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        if (messageText == null)
            messageText = FindObjectOfType<TMP_Text>();

        if (livesText == null)
            livesText = FindLivesText();

        lives = Mathf.Max(1, maxLives);
        UpdateLivesUI();

        messageText.SetText("Press Space to Start");
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (state == GameState.StartScreen)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                StartGame();
        }
        else if (state == GameState.Playing)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                PauseGame();
                return;
            }

            timer += Time.deltaTime;

            if (timer >= winTime)
                Win();
        }
        else if (state == GameState.Paused)
        {
            if (Input.GetKeyDown(KeyCode.P))
                ResumeGame();
        }
        else if (state == GameState.LifeLost)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ResumeAfterLifeLost();
        }
        else if (state == GameState.GameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void Lose()
    {
        if (state != GameState.Playing)
            return;

        lives--;
        UpdateLivesUI();

        if (lives > 0)
        {
            state = GameState.LifeLost;
            Time.timeScale = 0f;
            messageText.SetText($"Hit! Lives left: {lives}. Press Space to Continue");
        }
        else
        {
            state = GameState.GameOver;
            Time.timeScale = 0f;
            messageText.SetText("You Lost! Press R to Restart");
        }
    }

    private void StartGame()
    {
        timer = 0f;
        state = GameState.Playing;
        Time.timeScale = 1f;
        messageText.SetText("Avoid the obstacles!");
    }

    private void PauseGame()
    {
        state = GameState.Paused;
        Time.timeScale = 0f;
        messageText.SetText("Paused - Press P to Resume");
    }

    private void ResumeGame()
    {
        state = GameState.Playing;
        Time.timeScale = 1f;
        messageText.SetText("Avoid the obstacles!");
    }

    private void ResumeAfterLifeLost()
    {
        state = GameState.Playing;
        Time.timeScale = 1f;
        messageText.SetText("Avoid the obstacles!");
    }

    public void Win()
    {
        state = GameState.GameOver;
        Time.timeScale = 0f;
        messageText.SetText("You Win! Press R to Restart");
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.SetText($"Lives: {lives}");

        LivesChanged?.Invoke(lives);
    }

    private TMP_Text FindLivesText()
    {
        var texts = FindObjectsOfType<TMP_Text>();
        foreach (var tmp in texts)
        {
            if (tmp != null && tmp.name == "LivesText")
                return tmp;
        }
        return null;
    }
}
