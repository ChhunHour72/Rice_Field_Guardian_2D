using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    
    [Header("Remaining Time")]
    [SerializeField] private float seconds = 60f;

    private bool hasLoadedWinScene = false; // Prevents loading multiple times

    void Update()
    {
        if (seconds > 0)
        {
            seconds -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(seconds / 60);
            int secs = Mathf.FloorToInt(seconds % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, secs);
        }
        else if (!hasLoadedWinScene)
        {
            // Timer reached zero
            seconds = 0;
            timerText.text = "00:00";

            LoadCorrespondingWinScene();

            hasLoadedWinScene = true; // Ensure it only happens once
        }
    }

    private void LoadCorrespondingWinScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        string winSceneName;

        // Detect which level we're on based on scene name
        if (currentSceneName.Contains("1") || currentSceneName.ToLower().Contains("level1") || currentSceneName.ToLower().Contains("level_1"))
        {
            winSceneName = "WinScene_1";
        }
        else if (currentSceneName.Contains("2") || currentSceneName.ToLower().Contains("level2") || currentSceneName.ToLower().Contains("level_2"))
        {
            winSceneName = "WinScene_2";
        }
        else
        {
            // Default to WinScene_3 (for level 3 or any other case)
            winSceneName = "WinScene_3";
        }

        Debug.Log($"Time's up! Loading win scene: {winSceneName} from level: {currentSceneName}");

        SceneManager.LoadScene(winSceneName);
    }
}