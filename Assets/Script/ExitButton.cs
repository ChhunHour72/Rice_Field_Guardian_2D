using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ExitButton : MonoBehaviour
{
    // Call this method from your UI Button's OnClick event
    public void QuitGame()
    {
        #if UNITY_EDITOR
            // In the Unity Editor: Stops Play Mode and returns to Edit Mode
            EditorApplication.isPlaying = false;
        #else
            // In a built executable: Properly quits the application
            Application.Quit();
        #endif
    }

    // Optional: Add a confirmation dialog before quitting (useful for builds)
    // You can delete or comment this out if you don't want confirmation
    public void QuitGameWithConfirmation()
    {
        #if UNITY_EDITOR
            // In Editor, just stop immediately (no need for popup during testing)
            EditorApplication.isPlaying = false;
        #else
            // In builds, you could show a simple confirmation if desired
            // For now, we'll just quit directly. Replace with your own UI popup if needed.
            Application.Quit();
        #endif
    }
}