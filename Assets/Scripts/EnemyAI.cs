public static class EnemyAI
{
    private const int NoAbilities = 0;

    public static AbilityData ChooseAbility(Combatant self)
    {
        var abilities = self.Abilities;
        if (abilities == null || abilities.Count == NoAbilities)
            return null;

        var best = abilities[0];

        foreach (var ability in abilities)
        {
            if (ability.power > best.power)
                best = ability;
        }

        return best;
    }
}
