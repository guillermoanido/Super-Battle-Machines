using System.Collections.Generic;
using UnityEngine;

public enum EnemyStrategyType
{
    Aggressive,
    Random,
    Defensive
}

public interface IEnemyStrategy
{
    AbilityData ChooseAbility(Combatant self, Combatant opponent);
}

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

// Always throws its hardest-hitting ability.
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

// Picks any valid ability at random.
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

// Heals or shields when hurt, otherwise attacks hardest.
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
