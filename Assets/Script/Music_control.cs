using UnityEngine;
using UnityEngine.SceneManagement;
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource musicSource;
    private void Start()
{
    // Load your actual first gameplay/menu scene
    SceneManager.LoadScene("Home"); 
}

    private void Awake()
    {
        // If there's already an instance, destroy this duplicate
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set this as the singleton instance and make it persist
        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = GetComponent<AudioSource>();

        // Load the saved mute state (0 = unmuted, 1 = muted)
        bool isMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        musicSource.mute = isMuted;
    }

    public void MuteMusic()
    {
        if (musicSource != null)
        {
            musicSource.mute = true;
            PlayerPrefs.SetInt("MusicMuted", 1);
            PlayerPrefs.Save();
        }
    }

    public void UnmuteMusic()
    {
        if (musicSource != null)
        {
            musicSource.mute = false;
            PlayerPrefs.SetInt("MusicMuted", 0);
            PlayerPrefs.Save();
        }
    }

    // Optional: Useful if you want to check current state elsewhere
    public bool IsMuted()
    {
        return musicSource != null && musicSource.mute;
    }
}