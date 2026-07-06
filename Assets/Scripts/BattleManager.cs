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

    [Header("Testing")]
    [SerializeField] private bool autoPlayPlayer;

    public event Action<Combatant> PlayerWon;
    public event Action PlayerLost;
    public event Action<string> LogMessage;
    public event Action LogCleared;

    private AbilityData pendingPlayerAbility;
    private IEnemyStrategy enemyStrategy;
    private IEnemyStrategy autoPlayStrategy;

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

        autoPlayStrategy = EnemyStrategyFactory.Create(EnemyStrategyType.Aggressive);
        StartBattle();
    }

    public void StartBattle()
    {
        player.RestoreFull();

        if (enemyGenerator != null)
            enemy.Initialize(enemyGenerator.CreateRandomEnemy());

        enemyStrategy = EnemyStrategyFactory.Create(enemy.StrategyType);
        Log($"A wild {enemy.Name} appears!");

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
        var round = 0;

        while (player.IsAlive && enemy.IsAlive)
        {
            round++;
            LogCleared?.Invoke();
            Log($"— Round {round} —");

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
        Log($"{attacker.Name}'s turn ({actions} action(s)).");

        for (var i = 0; i < actions && defender.IsAlive; i++)
            yield return PerformAction(attacker, defender);
    }

    private IEnumerator PerformAction(Combatant attacker, Combatant defender)
    {
        if (IsPlayer(attacker) && !autoPlayPlayer)
        {
            yield return WaitForPlayerChoice();
            UseAbility(attacker, defender, pendingPlayerAbility);
            pendingPlayerAbility = null;
            yield break;
        }

        yield return new WaitForSeconds(EnemyActionDelay);
        var strategy = IsPlayer(attacker) ? autoPlayStrategy : enemyStrategy;
        UseAbility(attacker, defender, strategy.ChooseAbility(attacker, defender));
    }

    private IEnumerator WaitForPlayerChoice()
    {
        pendingPlayerAbility = null;
        while (pendingPlayerAbility == null)
            yield return null;
    }

    private void UseAbility(Combatant user, Combatant opponent, AbilityData ability)
    {
        if (ability == null)
            return;

        Log($"{user.Name} used {ability.abilityName}!");
        AbilityResolver.Apply(ability, user, opponent);
    }

    private bool IsPlayer(Combatant combatant) => combatant == player;

    private void Log(string message)
    {
        Debug.Log(message);
        LogMessage?.Invoke(message);
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
            abilityMenu.Rebuild();
    }

    private void EndBattle()
    {
        if (player.IsAlive)
            PlayerWon?.Invoke(enemy);
        else
            PlayerLost?.Invoke();
    }
}
