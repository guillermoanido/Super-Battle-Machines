using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>Shows the BattleManager's recent log lines on screen; clears each round.</summary>
public class CombatLog : MonoBehaviour
{
    private const int MaxLines = 6;

    [SerializeField] private BattleManager battleManager;
    [SerializeField] private TMP_Text logText;

    private readonly Queue<string> lines = new();

    private void OnEnable()
    {
        if (battleManager != null)
        {
            battleManager.LogMessage += Append;
            battleManager.LogCleared += Clear;
        }
    }

    private void OnDisable()
    {
        if (battleManager != null)
        {
            battleManager.LogMessage -= Append;
            battleManager.LogCleared -= Clear;
        }
    }

    private void Clear()
    {
        lines.Clear();
        if (logText != null)
            logText.text = string.Empty;
    }

    private void Append(string message)
    {
        lines.Enqueue(message);
        while (lines.Count > MaxLines)
            lines.Dequeue();

        if (logText != null)
            logText.text = string.Join("\n", lines);
    }
}
