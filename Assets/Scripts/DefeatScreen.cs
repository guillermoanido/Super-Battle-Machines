using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatScreen : MonoBehaviour
{
    private const float NormalTimeScale = 1f;

    [SerializeField] private BattleManager battleManager;
    [SerializeField] private GameObject panel;
    [SerializeField] private string mainMenuScene = "Main Menu";

    private void OnEnable()
    {
        if (battleManager != null)
            battleManager.PlayerLost += Show;
    }

    private void OnDisable()
    {
        if (battleManager != null)
            battleManager.PlayerLost -= Show;
    }

    private void Start()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    private void Show() => SetActiveSafe(panel, true);

    public void Continue()
    {
        Time.timeScale = NormalTimeScale;
        var current = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(current);
    }

    public void ExitToMenu()
    {
        Time.timeScale = NormalTimeScale;
        SceneManager.LoadScene(mainMenuScene);
    }

    private static void SetActiveSafe(GameObject target, bool active)
    {
        if (target != null)
            target.SetActive(active);
    }
}
