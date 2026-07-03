using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

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
        SetActiveSafe(settingsPanel, false);
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

        SetActiveSafe(mainButtons, false);
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

    public void OpenSettings() => SetActiveSafe(settingsPanel, true);

    public void CloseSettings() => SetActiveSafe(settingsPanel, false);

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
