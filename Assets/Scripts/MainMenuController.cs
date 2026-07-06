using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

/// <summary>Main menu: Start (optionally plays a cinematic then loads a scene), Settings, and Quit.</summary>
public class MainMenuController : MonoBehaviour
{
    private const float NormalTimeScale = 1f;

    [Header("Start / Scene Flow")]
    [SerializeField] private string sceneToLoad = "Machine Maker";
    [SerializeField] private PlayableDirector cinematicDirector;

    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject mainButtons;

    private bool isStarting;

    private void Start()
    {
        Time.timeScale = NormalTimeScale;
        settingsPanel.SetActiveSafe(false);
    }

    public void StartGame()
    {
        if (isStarting)
            return;
        isStarting = true;

        if (cinematicDirector == null)
        {
            LoadGameScene();
            return;
        }

        mainButtons.SetActiveSafe(false);
        cinematicDirector.stopped += OnCinematicFinished;
        cinematicDirector.Play();
    }

    private void OnCinematicFinished(PlayableDirector director)
    {
        cinematicDirector.stopped -= OnCinematicFinished;
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("MainMenuController: 'Scene To Load' is empty.", this);
            return;
        }

        SceneManager.LoadScene(sceneToLoad);
    }

    public void OpenSettings() => settingsPanel.SetActiveSafe(true);

    public void CloseSettings() => settingsPanel.SetActiveSafe(false);

    public void QuitGame() => GameApplication.Quit();
}
