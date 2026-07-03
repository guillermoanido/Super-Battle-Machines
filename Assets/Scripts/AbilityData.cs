using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Battle Machines/Ability")]
public class AbilityData : ScriptableObject
{
    public string abilityName;
    [TextArea] public string description;
    public int power;
    public Sprite icon;
}
