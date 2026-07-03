using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityMenu : MonoBehaviour
{
    private const int VisibleSlots = 4;
    private const int MinOffset = 0;
    private const int FirstAbility = 0;
    private const int ScrollStep = 1;

    [Header("Data")]
    [SerializeField] private List<AbilityData> abilities = new();

    [Header("Slots (assign the 4 grid buttons)")]
    [SerializeField] private AbilityButton[] slots = new AbilityButton[VisibleSlots];

    [Header("Description")]
    [SerializeField] private TMP_Text descriptionText;

    private int scrollOffset;

    private void Start()
    {
        Refresh();
        ShowFirstVisibleDescription();
    }

    public void AddAbility(AbilityData ability)
    {
        if (ability == null || abilities.Contains(ability))
            return;

        abilities.Add(ability);
        Refresh();
    }

    public void ScrollDown()
    {
        scrollOffset = Mathf.Min(scrollOffset + ScrollStep, MaxOffset());
        Refresh();
    }

    public void ScrollUp()
    {
        scrollOffset = Mathf.Max(scrollOffset - ScrollStep, MinOffset);
        Refresh();
    }

    private int MaxOffset() => Mathf.Max(MinOffset, abilities.Count - VisibleSlots);

    private void Refresh()
    {
        for (var i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];
            if (slot == null)
                continue;

            var abilityIndex = scrollOffset + i;
            if (abilityIndex < abilities.Count)
                slot.Bind(abilities[abilityIndex], ShowDescription);
            else
                slot.Clear();
        }
    }

    private void ShowFirstVisibleDescription()
    {
        if (abilities.Count > FirstAbility)
            ShowDescription(abilities[scrollOffset]);
    }

    private void ShowDescription(AbilityData ability)
    {
        if (descriptionText != null)
            descriptionText.text = ability.description;
    }
}
