using UnityEngine;

/// <summary>Shared GameObject helpers.</summary>
public static class GameObjectExtensions
{
    /// <summary>Sets active state only when the target is assigned, removing null checks at call sites.</summary>
    public static void SetActiveSafe(this GameObject target, bool active)
    {
        if (target != null)
            target.SetActive(active);
    }
}
