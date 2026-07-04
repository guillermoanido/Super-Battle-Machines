using System;
using System.Collections.Generic;
using UnityEngine;

public class Combatant : MonoBehaviour
{
    private const int MinDamage = 1;
    private const int MinHP = 0;
    private const int MinDefense = 0;
    private const int MinGain = 0;

    [SerializeField] private MachineData data;

    public string Name { get; private set; }
    public int MaxHP { get; private set; }
    public int CurrentHP { get; private set; }
    public int MaxDefense { get; private set; }
    public int CurrentDefense { get; private set; }
    public int Speed { get; private set; }
    public EnemyStrategyType StrategyType { get; private set; }
    public List<AbilityData> Abilities { get; private set; } = new();

    public bool IsAlive => CurrentHP > MinHP;

    public event Action<Combatant> StatsChanged;
    public event Action<Combatant> Died;

    private void Awake()
    {
        if (data != null)
            Initialize(data);
    }

    public void Initialize(MachineData source)
    {
        data = source;
        Name = source.machineName;
        MaxHP = source.maxHP;
        CurrentHP = source.maxHP;
        MaxDefense = source.defense;
        CurrentDefense = source.defense;
        Speed = source.speed;
        StrategyType = source.strategy;
        Abilities = new List<AbilityData>(source.startingAbilities);

        StatsChanged?.Invoke(this);
    }

    public void ReceiveDamage(int amount)
    {
        var damage = Mathf.Max(MinDamage, amount);

        var absorbed = Mathf.Min(CurrentDefense, damage);
        CurrentDefense = Mathf.Max(MinDefense, CurrentDefense - absorbed);
        damage -= absorbed;

        CurrentHP = Mathf.Max(MinHP, CurrentHP - damage);
        StatsChanged?.Invoke(this);

        if (!IsAlive)
            Died?.Invoke(this);
    }

    public void Heal(int amount)
    {
        CurrentHP = Mathf.Min(MaxHP, CurrentHP + Mathf.Max(MinGain, amount));
        StatsChanged?.Invoke(this);
    }

    public void GainDefense(int amount)
    {
        CurrentDefense += Mathf.Max(MinGain, amount);
        MaxDefense = Mathf.Max(MaxDefense, CurrentDefense);
        StatsChanged?.Invoke(this);
    }

    public void GainSpeed(int amount)
    {
        Speed += Mathf.Max(MinGain, amount);
        StatsChanged?.Invoke(this);
    }

    public void LearnAbility(AbilityData ability)
    {
        if (ability != null && !Abilities.Contains(ability))
            Abilities.Add(ability);
    }
}
