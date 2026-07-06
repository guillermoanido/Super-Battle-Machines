using UnityEngine;

public enum AbilityEffect
{
    Damage,
    Heal,
    GainDefense,
    IncreaseSpeed,
    Status
}

[CreateAssetMenu(fileName = "Ability", menuName = "Battle Machines/Ability")]
/// <summary>Asset defining an ability: its effect kind and magnitude (power), plus display info.</summary>
public class AbilityData : ScriptableObject
{
    public string abilityName;
    [TextArea] public string description;
    public AbilityEffect effect = AbilityEffect.Damage;
    public int power;
    public Sprite icon;
}
