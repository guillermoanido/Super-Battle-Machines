public static class AbilityResolver
{
    public static void Apply(AbilityData ability, Combatant user, Combatant opponent)
    {
        if (ability == null)
            return;

        switch (ability.effect)
        {
            case AbilityEffect.Damage:
                opponent.ReceiveDamage(ability.power);
                break;
            case AbilityEffect.Heal:
                user.Heal(ability.power);
                break;
            case AbilityEffect.GainDefense:
                user.GainDefense(ability.power);
                break;
            case AbilityEffect.IncreaseSpeed:
                user.GainSpeed(ability.power);
                break;
            case AbilityEffect.Status:
                // TODO: status effects (defined later)
                break;
        }
    }
}
