using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Machine", menuName = "Battle Machines/Machine")]
/// <summary>Asset defining a machine: stats, AI strategy, and starting abilities.</summary>
public class MachineData : ScriptableObject
{
    public string machineName;
    public int maxHP;
    public int defense;
    public int speed;
    public EnemyStrategyType strategy;
    public Sprite sprite;
    public List<AbilityData> startingAbilities = new();
}
