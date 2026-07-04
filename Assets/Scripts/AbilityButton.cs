using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class AbilityButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button button;

    private AbilityData ability;
    private Action<AbilityData> onFocused;
    private Action<AbilityData> onChosen;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();
        button.onClick.AddListener(Choose);
    }

    public void Bind(AbilityData data, Action<AbilityData> focusedCallback, Action<AbilityData> chosenCallback)
    {
        ability = data;
        onFocused = focusedCallback;
        onChosen = chosenCallback;

        if (label != null)
            label.text = data.abilityName;
        if (iconImage != null)
        {
            iconImage.sprite = data.icon;
            iconImage.enabled = data.icon != null;
        }

        button.interactable = true;
    }

    public void Clear()
    {
        ability = null;
        onFocused = null;
        onChosen = null;

        if (label != null)
            label.text = string.Empty;
        if (iconImage != null)
            iconImage.enabled = false;

        button.interactable = false;
    }

    public void OnPointerEnter(PointerEventData eventData) => Focus();

    public void OnSelect(BaseEventData eventData) => Focus();

    private void Focus()
    {
        if (ability != null)
            onFocused?.Invoke(ability);
    }

    private void Choose()
    {
        if (ability != null)
            onChosen?.Invoke(ability);
    }
}
