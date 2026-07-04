using UnityEngine;
using TMPro;

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
        if (panel != null)
            panel.SetActive(true);

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
        if (panel != null)
            panel.SetActive(false);
    }
}
