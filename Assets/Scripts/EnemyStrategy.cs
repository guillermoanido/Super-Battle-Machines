using System.Collections.Generic;
using UnityEngine;

public enum EnemyStrategyType
{
    Aggressive,
    Random,
    Defensive
}

/// <summary>An enemy decision-making behavior. Implement this to add new AI styles.</summary>
public interface IEnemyStrategy
{
    AbilityData ChooseAbility(Combatant self, Combatant opponent);
}

/// <summary>Creates the strategy instance for a given strategy type.</summary>
public static class EnemyStrategyFactory
{
    public static IEnemyStrategy Create(EnemyStrategyType type)
    {
        switch (type)
        {
            case EnemyStrategyType.Random:
                return new RandomStrategy();
            case EnemyStrategyType.Defensive:
                return new DefensiveStrategy();
            default:
                return new AggressiveStrategy();
        }
    }
}

/// <summary>Always throws its hardest-hitting ability.</summary>
public class AggressiveStrategy : IEnemyStrategy
{
    public AbilityData ChooseAbility(Combatant self, Combatant opponent)
    {
        AbilityData strongest = null;

        foreach (var ability in self.Abilities)
        {
            if (ability == null)
                continue;

            if (strongest == null || ability.power > strongest.power)
                strongest = ability;
        }

        return strongest;
    }
}

/// <summary>Picks any valid ability at random.</summary>
public class RandomStrategy : IEnemyStrategy
{
    private const int FirstIndex = 0;

    public AbilityData ChooseAbility(Combatant self, Combatant opponent)
    {
        var options = new List<AbilityData>();
        foreach (var ability in self.Abilities)
        {
            if (ability != null)
                options.Add(ability);
        }

        if (options.Count == 0)
            return null;

        return options[Random.Range(FirstIndex, options.Count)];
    }
}

/// <summary>Heals or shields when hurt, otherwise attacks hardest.</summary>
public class DefensiveStrategy : IEnemyStrategy
{
    private const float LowHealthRatio = 0.5f;

    public AbilityData ChooseAbility(Combatant self, Combatant opponent)
    {
        var lowHealth = self.CurrentHP <= self.MaxHP * LowHealthRatio;
        AbilityData strongest = null;

        foreach (var ability in self.Abilities)
        {
            if (ability == null)
                continue;

            if (lowHealth && IsProtective(ability))
                return ability;

            if (strongest == null || ability.power > strongest.power)
                strongest = ability;
        }

        return strongest;
    }

    private static bool IsProtective(AbilityData ability) =>
        ability.effect == AbilityEffect.Heal || ability.effect == AbilityEffect.GainDefense;
}
