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
public class AbilityData : ScriptableObject
{
    public string abilityName;
    [TextArea] public string description;
    public AbilityEffect effect = AbilityEffect.Damage;
    public int power;
    public Sprite icon;
}
