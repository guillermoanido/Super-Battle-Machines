using System.Collections.Generic;
using UnityEngine;

/// <summary>Builds a random enemy MachineData at runtime: random stats, strategy, and 4 abilities from the pool.</summary>
public class EnemyGenerator : MonoBehaviour
{
    private const int AbilitiesPerEnemy = 4;
    private const int FirstIndex = 0;
    private const int RangeInclusive = 1;

    [Header("Ability Pool")]
    [SerializeField] private List<AbilityData> abilityPool = new();

    [Header("Identity")]
    [SerializeField] private string enemyName = "Rogue Machine";

    [Header("Stat Ranges (inclusive)")]
    [SerializeField] private int minHP = 20;
    [SerializeField] private int maxHP = 60;
    [SerializeField] private int minDefense = 0;
    [SerializeField] private int maxDefense = 10;
    [SerializeField] private int minSpeed = 5;
    [SerializeField] private int maxSpeed = 20;

    public MachineData CreateRandomEnemy()
    {
        var machine = ScriptableObject.CreateInstance<MachineData>();
        machine.machineName = enemyName;
        machine.maxHP = RandomInRange(minHP, maxHP);
        machine.defense = RandomInRange(minDefense, maxDefense);
        machine.speed = RandomInRange(minSpeed, maxSpeed);
        machine.strategy = RandomStrategyType();
        machine.startingAbilities = PickRandomAbilities();
        return machine;
    }

    private static EnemyStrategyType RandomStrategyType()
    {
        var values = System.Enum.GetValues(typeof(EnemyStrategyType));
        return (EnemyStrategyType)values.GetValue(Random.Range(FirstIndex, values.Length));
    }

    private List<AbilityData> PickRandomAbilities()
    {
        var available = new List<AbilityData>();
        foreach (var ability in abilityPool)
        {
            if (ability != null)
                available.Add(ability);
        }

        var chosen = new List<AbilityData>();
        var count = Mathf.Min(AbilitiesPerEnemy, available.Count);

        for (var i = 0; i < count; i++)
        {
            var index = Random.Range(FirstIndex, available.Count);
            chosen.Add(available[index]);
            available.RemoveAt(index);
        }

        return chosen;
    }

    private static int RandomInRange(int min, int max) => Random.Range(min, max + RangeInclusive);
}
