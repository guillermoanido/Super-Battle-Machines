using System;
using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private const int MinActions = 1;
    private const int MaxActions = 3;
    private const int MinSpeed = 1;
    private const float EnemyActionDelay = 1f;

    [SerializeField] private Combatant player;
    [SerializeField] private Combatant enemy;
    [SerializeField] private AbilityMenu abilityMenu;
    [SerializeField] private EnemyGenerator enemyGenerator;

    public event Action<Combatant> PlayerWon;
    public event Action PlayerLost;

    private AbilityData pendingPlayerAbility;

    private void OnEnable()
    {
        if (abilityMenu != null)
            abilityMenu.AbilityChosen += OnPlayerChoseAbility;
    }

    private void OnDisable()
    {
        if (abilityMenu != null)
            abilityMenu.AbilityChosen -= OnPlayerChoseAbility;
    }

    private void Start()
    {
        if (!HasRequiredReferences())
            return;

        if (enemyGenerator != null)
            enemy.Initialize(enemyGenerator.CreateRandomEnemy());

        StartCoroutine(RunBattle());
    }

    private bool HasRequiredReferences()
    {
        if (player == null)
            Debug.LogError("BattleManager: Player is not assigned.", this);
        if (enemy == null)
            Debug.LogError("BattleManager: Enemy is not assigned.", this);

        return player != null && enemy != null;
    }

    private IEnumerator RunBattle()
    {
        while (player.IsAlive && enemy.IsAlive)
        {
            var playerFirst = player.Speed >= enemy.Speed;
            var first = playerFirst ? player : enemy;
            var second = playerFirst ? enemy : player;

            yield return TakeTurn(first, second);
            if (!second.IsAlive)
                break;

            yield return TakeTurn(second, first);
            if (!first.IsAlive)
                break;
        }

        EndBattle();
    }

    private IEnumerator TakeTurn(Combatant attacker, Combatant defender)
    {
        var actions = ActionsFor(attacker, defender);
        for (var i = 0; i < actions && defender.IsAlive; i++)
            yield return PerformAction(attacker, defender);
    }

    private IEnumerator PerformAction(Combatant attacker, Combatant defender)
    {
        var ability = attacker == player
            ? null
            : EnemyAI.ChooseAbility(attacker);

        if (attacker == player)
        {
            pendingPlayerAbility = null;
            while (pendingPlayerAbility == null)
                yield return null;

            ability = pendingPlayerAbility;
            pendingPlayerAbility = null;
        }
        else
        {
            yield return new WaitForSeconds(EnemyActionDelay);
        }

        if (ability != null)
        {
            Debug.Log($"{attacker.Name} used {ability.abilityName} on {defender.Name}.");
            defender.ReceiveAttack(ability);
        }
    }

    private int ActionsFor(Combatant attacker, Combatant defender)
    {
        var ratio = attacker.Speed / Mathf.Max(MinSpeed, defender.Speed);
        return Mathf.Clamp(ratio, MinActions, MaxActions);
    }

    private void OnPlayerChoseAbility(AbilityData ability) => pendingPlayerAbility = ability;

    public void ClaimAbility(AbilityData ability)
    {
        player.LearnAbility(ability);
        if (abilityMenu != null)
            abilityMenu.AddAbility(ability);
    }

    private void EndBattle()
    {
        if (player.IsAlive)
            PlayerWon?.Invoke(enemy);
        else
            PlayerLost?.Invoke();
    }
}
