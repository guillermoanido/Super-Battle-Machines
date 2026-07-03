using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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
        SetActiveSafe(pauseMenuPanel, true);
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
        SetActiveSafe(pauseMenuPanel, false);
        SetActiveSafe(settingsPanel, false);
    }

    public void OpenSettings() => SetActiveSafe(settingsPanel, true);

    public void CloseSettings() => SetActiveSafe(settingsPanel, false);

    public void ReturnToMainMenu()
    {
        Time.timeScale = NormalTimeScale;
        IsPaused = false;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private static void SetActiveSafe(GameObject target, bool active)
    {
        if (target != null)
            target.SetActive(active);
    }
}
