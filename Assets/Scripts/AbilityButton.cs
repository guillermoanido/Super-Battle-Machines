using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button button;

    private AbilityData ability;
    private Action<AbilityData> onSelected;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();
        button.onClick.AddListener(Select);
    }

    public void Bind(AbilityData data, Action<AbilityData> selectedCallback)
    {
        ability = data;
        onSelected = selectedCallback;

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
        onSelected = null;

        if (label != null)
            label.text = string.Empty;
        if (iconImage != null)
            iconImage.enabled = false;

        button.interactable = false;
    }

    private void Select()
    {
        if (ability != null)
            onSelected?.Invoke(ability);
    }
}
