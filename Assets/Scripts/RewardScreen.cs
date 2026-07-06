using UnityEngine;
using TMPro;

/// <summary>On victory, shows the defeated enemy's abilities so the player can claim one, then starts the next fight.</summary>
public class RewardScreen : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private GameObject panel;
    [SerializeField] private AbilityButton[] rewardSlots;
    [SerializeField] private TMP_Text descriptionText;

    private void OnEnable()
    {
        if (battleManager != null)
            battleManager.PlayerWon += Show;
    }

    private void OnDisable()
    {
        if (battleManager != null)
            battleManager.PlayerWon -= Show;
    }

    private void Show(Combatant defeatedEnemy)
    {
        panel.SetActiveSafe(true);

        var rewards = defeatedEnemy.Abilities;
        for (var i = 0; i < rewardSlots.Length; i++)
        {
            var slot = rewardSlots[i];
            if (slot == null)
                continue;

            if (i < rewards.Count)
                slot.Bind(rewards[i], ShowDescription, Claim);
            else
                slot.Clear();
        }
    }

    private void ShowDescription(AbilityData ability)
    {
        if (descriptionText != null)
            descriptionText.text = ability.description;
    }

    private void Claim(AbilityData ability)
    {
        battleManager.ClaimAbility(ability);
        panel.SetActiveSafe(false);
        battleManager.StartBattle();
    }
}
