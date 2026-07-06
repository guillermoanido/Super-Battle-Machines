using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>Binds a Combatant's stats to UI: HP bar + text, Defense, and Speed. Updates on StatsChanged.</summary>
public class CombatantHUD : MonoBehaviour
{
    [SerializeField] private Combatant combatant;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text defenseText;
    [SerializeField] private TMP_Text speedText;

    private void OnEnable()
    {
        if (combatant != null)
            combatant.StatsChanged += Refresh;
    }

    private void OnDisable()
    {
        if (combatant != null)
            combatant.StatsChanged -= Refresh;
    }

    private void Start() => Refresh(combatant);

    private void Refresh(Combatant source)
    {
        if (source == null)
            return;

        if (healthBar != null)
        {
            healthBar.maxValue = source.MaxHP;
            healthBar.value = source.CurrentHP;
        }
        if (healthText != null)
            healthText.text = $"{source.CurrentHP}/{source.MaxHP}";
        if (defenseText != null)
            defenseText.text = source.CurrentDefense.ToString();
        if (speedText != null)
            speedText.text = source.Speed.ToString();
    }
}
