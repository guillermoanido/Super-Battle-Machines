using UnityEngine;

/// <summary>On player defeat, opens the shared pause menu (which offers "return to main menu").</summary>
public class DefeatScreen : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private PauseMenu pauseMenu;

    private void OnEnable()
    {
        if (battleManager != null)
            battleManager.PlayerLost += OnPlayerLost;
    }

    private void OnDisable()
    {
        if (battleManager != null)
            battleManager.PlayerLost -= OnPlayerLost;
    }

    private void OnPlayerLost()
    {
        if (pauseMenu != null)
            pauseMenu.Pause();
    }
}
