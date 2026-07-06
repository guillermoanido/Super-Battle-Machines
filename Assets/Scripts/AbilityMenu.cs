using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityMenu : MonoBehaviour
{
    public event Action<AbilityData> AbilityChosen;

    private const int VisibleSlots = 4;
    private const int MinOffset = 0;
    private const int EmptyCount = 0;
    private const int PageStep = VisibleSlots;
    private const int LastItemOffset = 1;

    [Header("Owner (whose abilities to show)")]
    [SerializeField] private Combatant owner;

    [Header("Slots (assign the 4 grid buttons)")]
    [SerializeField] private AbilityButton[] slots = new AbilityButton[VisibleSlots];

    [Header("Description")]
    [SerializeField] private TMP_Text descriptionText;

    private int scrollOffset;

    private List<AbilityData> Abilities => owner != null ? owner.Abilities : null;
    private int Count => Abilities != null ? Abilities.Count : EmptyCount;

    private void Start() => Rebuild();

    public void Rebuild()
    {
        Refresh();
        ShowFirstVisibleDescription();
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
        if (Count == EmptyCount)
            return MinOffset;

        return (Count - LastItemOffset) / VisibleSlots * VisibleSlots;
    }

    private void Refresh()
    {
        var abilities = Abilities;
        for (var i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];
            if (slot == null)
                continue;

            var abilityIndex = scrollOffset + i;
            if (abilities != null && abilityIndex < abilities.Count)
                slot.Bind(abilities[abilityIndex], ShowDescription, SelectAbility);
            else
                slot.Clear();
        }
    }

    private void ShowFirstVisibleDescription()
    {
        var abilities = Abilities;
        if (abilities != null && abilities.Count > scrollOffset)
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
