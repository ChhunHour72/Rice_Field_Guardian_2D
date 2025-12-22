using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioClip generalBGM;
    [SerializeField] private AudioClip gameBGM;
    [SerializeField] private AudioClip winBGM;      // NEW: For WinScene_*
    [SerializeField] private AudioClip loseBGM;     // NEW: For LooseScene_*
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioMixer musicMixer;

    private bool isMusicOn = true;
    private const string MusicPrefKey = "MusicOn";
    private const string ExposedMusicVolumeParam = "MusicVolume";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Load saved preference (default: on)
            isMusicOn = PlayerPrefs.GetInt(MusicPrefKey, 1) == 1;

            SetupAudioSource();

            // Subscribe to scene loads
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Apply mute state immediately (mixer should be ready now)
            ApplyMusicState();

            // Now handle the initial scene's BGM (clip switch + play if needed)
            // We do this AFTER applying mute state
            SwitchToCurrentSceneBGM();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void SetupAudioSource()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not assigned in AudioManager!");
            return;
        }

        audioSource.loop = true;
        audioSource.spatialBlend = 0f;
        audioSource.pitch = 1f;

        // Do NOT play here — we'll play in SwitchToCurrentSceneBGM() after mute is applied
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (audioSource == null) return;

        // Only switch clip and play — mute state is already applied globally
        SwitchToCurrentSceneBGM();

        // Re-apply mute state in case mixer was reset or something (defensive)
        ApplyMusicState();
    }

    // Updated helper: switches BGM based on current scene
    // - GameScene_* → gameBGM
    // - WinScene_* → winBGM
    // - LooseScene_* → loseBGM
    // - All others → generalBGM
    private void SwitchToCurrentSceneBGM()
    {
        if (audioSource == null) return;

        string sceneName = SceneManager.GetActiveScene().name;
        AudioClip targetClip;

        if (sceneName.StartsWith("GameScene_"))
        {
            targetClip = gameBGM;
        }
        else if (sceneName.StartsWith("WinScene_"))
        {
            targetClip = winBGM;
        }
        else if (sceneName.StartsWith("LooseScene_"))
        {
            targetClip = loseBGM;
        }
        else
        {
            targetClip = generalBGM;
        }

        if (audioSource.clip != targetClip)
        {
            audioSource.clip = targetClip;
        }

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // =============== BUTTON METHODS ===============
    public static void StaticMuteMusic()
    {
        if (Instance != null) Instance.SetMusicOn(false);
    }

    public static void StaticUnmuteMusic()
    {
        if (Instance != null) Instance.SetMusicOn(true);
    }

    public static void StaticToggleMusic()
    {
        if (Instance != null) Instance.SetMusicOn(!Instance.isMusicOn);
    }

    // =============== INTERNAL LOGIC ===============
    private void SetMusicOn(bool on)
    {
        if (isMusicOn == on) return;

        isMusicOn = on;
        ApplyMusicState();
        PlayerPrefs.SetInt(MusicPrefKey, isMusicOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void ApplyMusicState()
    {
        if (musicMixer == null)
        {
            Debug.LogWarning("Music Mixer not assigned!");
            return;
        }

        float volumeDB = isMusicOn ? 0f : -80f;
        musicMixer.SetFloat(ExposedMusicVolumeParam, volumeDB);
    }
}