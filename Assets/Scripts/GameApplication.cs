using UnityEngine;

/// <summary>Application-level actions shared across menus.</summary>
public static class GameApplication
{
    /// <summary>Quits the game: stops Play Mode in the editor, closes the build otherwise.</summary>
    public static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
