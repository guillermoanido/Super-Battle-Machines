using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityMenu : MonoBehaviour
{
    public event Action<AbilityData> AbilityChosen;

    private const int VisibleSlots = 4;
    private const int MinOffset = 0;
    private const int FirstAbility = 0;
    private const int PageStep = VisibleSlots;
    private const int LastItemOffset = 1;

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
        scrollOffset = Mathf.Min(scrollOffset + PageStep, LastPageOffset());
        Refresh();
    }

    public void ScrollUp()
    {
        scrollOffset = Mathf.Max(scrollOffset - PageStep, MinOffset);
        Refresh();
    }

    private int LastPageOffset()
    {
        if (abilities.Count == FirstAbility)
            return MinOffset;

        return (abilities.Count - LastItemOffset) / VisibleSlots * VisibleSlots;
    }

    private void Refresh()
    {
        for (var i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];
            if (slot == null)
                continue;

            var abilityIndex = scrollOffset + i;
            if (abilityIndex < abilities.Count)
                slot.Bind(abilities[abilityIndex], ShowDescription, SelectAbility);
            else
                slot.Clear();
        }
    }

    private void ShowFirstVisibleDescription()
    {
        if (abilities.Count > FirstAbility)
            ShowDescription(abilities[scrollOffset]);
    }

    private void SelectAbility(AbilityData ability)
    {
        ShowDescription(ability);
        AbilityChosen?.Invoke(ability);
    }

    private void ShowDescription(AbilityData ability)
    {
        if (descriptionText != null)
            descriptionText.text = ability.description;
    }
}
