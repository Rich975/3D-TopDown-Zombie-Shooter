using Unity.VisualScripting;
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

    public enum MenuScreenState {Title, Start, Pause, GameOver, Game };
    public MenuScreenState state;

    private bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(MenuScreenState.Title);
        

    }

    // Update is called once per frame
    void Update()
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
                ScreenManager(false, true, false, true);
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
            ChangeState(MenuScreenState.Start);

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

    public void ResumeGame()
    {
        isPaused = false;
        ChangeState(MenuScreenState.Game);
    }
}

