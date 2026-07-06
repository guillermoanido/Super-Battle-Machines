using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

/// <summary>In-game pause menu: Esc toggles it, freezes time, and can exit to the main menu.</summary>
public class PauseMenu : MonoBehaviour
{
    private const float PausedTimeScale = 0f;
    private const float NormalTimeScale = 1f;

    [Header("References")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Scene Flow")]
    [SerializeField] private string mainMenuScene = "Main Menu";

    public bool IsPaused { get; private set; }

    private void Start() => ResumeInternal();

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            TogglePause();
    }

    public void TogglePause()
    {
        if (IsPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = PausedTimeScale;
        pauseMenuPanel.SetActiveSafe(true);
    }

    public void Resume()
    {
        if (settingsPanel != null && settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
            return;
        }

        ResumeInternal();
    }

    private void ResumeInternal()
    {
        IsPaused = false;
        Time.timeScale = NormalTimeScale;
        pauseMenuPanel.SetActiveSafe(false);
        settingsPanel.SetActiveSafe(false);
    }

    public void OpenSettings() => settingsPanel.SetActiveSafe(true);

    public void CloseSettings() => settingsPanel.SetActiveSafe(false);

    public void ReturnToMainMenu()
    {
        Time.timeScale = NormalTimeScale;
        IsPaused = false;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitGame() => GameApplication.Quit();
}
