using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject pressSpaceText;

    [SerializeField] private Image gameTitle;

    public static MenuManager Instance;

    public enum MenuScreenState
    { Title, Start, Pause, GameOver, Game };

    public MenuScreenState state;
    private bool isPaused;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        ChangeState(MenuScreenState.Title);
    }

    // Update is called once per frame
    private void Update()
    {
        HandlePause();
        HandleTitle();
    }

    private void ChangeState(MenuScreenState newState)
    {
        state = newState;

        switch (state)
        {
            case MenuScreenState.Title:
                ScreenManager(false, false, false, true);
                Time.timeScale = 1;
                break;

            case MenuScreenState.Start:
                ScreenManager(true, false, false, true);
                pressSpaceText.SetActive(false);
                Time.timeScale = 1; // Stop the game
                break;

            case MenuScreenState.Pause:
                ScreenManager(false, true, false, false);
                Time.timeScale = 0; // Pause the game
                break;

            case MenuScreenState.GameOver:
                ScreenManager(false, false, true, true);
                Time.timeScale = 1; // Stop the game
                break;

            case MenuScreenState.Game:
                ScreenManager(false, false, false, false);
                Time.timeScale = 1;
                break;

            default:
                ScreenManager(false, false, false, false);
                Time.timeScale = 1; // Resume the game
                break;
        }
    }

    private void ScreenManager(bool showStart, bool showPause, bool showGameOver, bool showTitle)
    {
        startScreen.SetActive(showStart);
        pauseScreen.SetActive(showPause);
        gameOverScreen.SetActive(showGameOver);
        titleScreen.SetActive(showTitle);
    }

    private void HandleTitle()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (state == MenuScreenState.Title)
            {
                ChangeState(MenuScreenState.Start);
            }

            return;
        }
    }

    private void HandlePause()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (state == MenuScreenState.Game)
            {
                isPaused = !isPaused;
                ChangeState(isPaused ? MenuScreenState.Pause : MenuScreenState.Game);
            }
        }
    }

    public void OnGameOver()
    {
        ChangeState(MenuScreenState.GameOver);
    }

    public void OnGameStart()
    {
        ChangeState(MenuScreenState.Start);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(1);
        ChangeState(MenuScreenState.Game);
    }


    public void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene + 1);
    }

    public void ResumeGame()
    {
        isPaused = false;
        ChangeState(MenuScreenState.Game);
    }

    public void BackToMainMenu()
    {
        ChangeState(MenuScreenState.Title);
    }
}