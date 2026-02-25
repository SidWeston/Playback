#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class BootstrapSceneLoader
{
    private const string BootstrapScenePath = "Assets/Scenes/Bootstrap.unity";
    private const string PreviousSceneKey = "BootstrapLoader_PreviousScene";
    private const string ShouldReturnKey = "BootstrapLoader_ShouldReturn";

    static BootstrapSceneLoader()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            string currentScenePath = EditorSceneManager.GetActiveScene().path;

            // Already in bootstrap, just let it play normally
            if (currentScenePath == BootstrapScenePath)
            {
                SessionState.SetBool(ShouldReturnKey, false);
                return;
            }

            // Store the current scene path so we can return to it after bootstrap loads
            SessionState.SetString(PreviousSceneKey, currentScenePath);
            SessionState.SetBool(ShouldReturnKey, true);

            // Check for unsaved changes before switching
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                // User cancelled, abort entering play mode
                EditorApplication.isPlaying = false;
                return;
            }

            // Open bootstrap scene before play mode starts
            EditorSceneManager.OpenScene(BootstrapScenePath);
        }

        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            if (!SessionState.GetBool(ShouldReturnKey, false)) return;

            string previousScene = SessionState.GetString(PreviousSceneKey, string.Empty);
            if (string.IsNullOrEmpty(previousScene)) return;

            // Load the original scene additively on top of bootstrap
            SceneManager.LoadScene(previousScene, LoadSceneMode.Additive);
        }

        if (state == PlayModeStateChange.EnteredEditMode)
        {
            if (!SessionState.GetBool(ShouldReturnKey, false)) return;

            string previousScene = SessionState.GetString(PreviousSceneKey, string.Empty);
            if (string.IsNullOrEmpty(previousScene)) return;

            // Return to the original scene in the editor when play mode ends
            EditorSceneManager.OpenScene(previousScene);
            SessionState.SetBool(ShouldReturnKey, false);
        }
    }
}
#endif
