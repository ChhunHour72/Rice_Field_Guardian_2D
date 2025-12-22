using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;  // Needed for Image

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // BGM Clips
    [SerializeField] private AudioClip generalBGM;
    [SerializeField] private AudioClip gameBGM;
    [SerializeField] private AudioClip winBGM;
    [SerializeField] private AudioClip loseBGM;

    // UI Click Sound
    [SerializeField] private AudioClip buttonClickClip;

    // AudioSources
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    // Mixer
    [SerializeField] private AudioMixer audioMixer;

    // Icon references (assign in Inspector from Settings scene)
    [Header("Settings UI Icons (Optional - Assign if you have Settings scene)")]
    [SerializeField] private Image musicIconImage;
    [SerializeField] private Image sfxIconImage;

    [SerializeField] private Sprite iconOnSprite;
    [SerializeField] private Sprite iconOffSprite;

    private bool isMusicOn = true;
    private bool isSFXOn = true;

    private const string MusicPrefKey = "MusicOn";
    private const string SFXPrefKey = "SFXOn";
    private const string ExposedMusicVolumeParam = "MusicVolume";
    private const string ExposedSFXVolumeParam = "SFXVolume";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            isMusicOn = PlayerPrefs.GetInt(MusicPrefKey, 1) == 1;
            isSFXOn = PlayerPrefs.GetInt(SFXPrefKey, 1) == 1;

            SetupAudioSources();
            SceneManager.sceneLoaded += OnSceneLoaded;

            ApplyMusicState();
            ApplySFXState();
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

    private void SetupAudioSources()
    {
        if (musicSource == null || sfxSource == null)
        {
            Debug.LogError("AudioSources not assigned!");
            return;
        }

        musicSource.loop = true;
        musicSource.playOnAwake = false;
        sfxSource.playOnAwake = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SwitchToCurrentSceneBGM();
        ApplyMusicState();
        ApplySFXState();

        // Refresh icons when entering Settings scene
        if (scene.name.Contains("Settings") || scene.name.Contains("Setting"))
        {
            UpdateAllIcons();
        }
    }

    private void SwitchToCurrentSceneBGM()
    {
        if (musicSource == null) return;

        string sceneName = SceneManager.GetActiveScene().name;
        AudioClip targetClip = sceneName.StartsWith("GameScene_") ? gameBGM :
                               sceneName.StartsWith("WinScene_") ? winBGM :
                               sceneName.StartsWith("LooseScene_") ? loseBGM :
                               generalBGM;

        if (musicSource.clip != targetClip || !musicSource.isPlaying)
        {
            musicSource.clip = targetClip;
            musicSource.Play();
        }
    }

    public void PlayButtonClickSound()
    {
        if (sfxSource != null && buttonClickClip != null)
        {
            sfxSource.PlayOneShot(buttonClickClip);
        }
    }

    public bool IsMusicOn() => isMusicOn;
    public bool IsSFXOn() => isSFXOn;

    // Static methods for buttons
    public static void StaticMuteMusic() => Instance?.SetMusicOn(false);
    public static void StaticUnmuteMusic() => Instance?.SetMusicOn(true);
    public static void StaticMuteSFX() => Instance?.SetSFXOn(false);
    public static void StaticUnmuteSFX() => Instance?.SetSFXOn(true);

    private void SetMusicOn(bool on)
    {
        if (isMusicOn == on) return;
        isMusicOn = on;
        ApplyMusicState();
        PlayerPrefs.SetInt(MusicPrefKey, isMusicOn ? 1 : 0);
        PlayerPrefs.Save();
        UpdateMusicIcon();
    }

    private void SetSFXOn(bool on)
    {
        if (isSFXOn == on) return;
        isSFXOn = on;
        ApplySFXState();
        PlayerPrefs.SetInt(SFXPrefKey, isSFXOn ? 1 : 0);
        PlayerPrefs.Save();
        UpdateSFXIcon();
    }

    private void ApplyMusicState()
    {
        if (audioMixer != null)
            audioMixer.SetFloat(ExposedMusicVolumeParam, isMusicOn ? 0f : -80f);
    }

    private void ApplySFXState()
    {
        if (audioMixer != null)
            audioMixer.SetFloat(ExposedSFXVolumeParam, isSFXOn ? 0f : -80f);
    }

    // Icon update methods
    private void UpdateMusicIcon()
    {
        if (musicIconImage != null && iconOnSprite != null && iconOffSprite != null)
        {
            musicIconImage.sprite = isMusicOn ? iconOnSprite : iconOffSprite;
        }
    }

    private void UpdateSFXIcon()
    {
        if (sfxIconImage != null && iconOnSprite != null && iconOffSprite != null)
        {
            sfxIconImage.sprite = isSFXOn ? iconOnSprite : iconOffSprite;
        }
    }

    private void UpdateAllIcons()
    {
        UpdateMusicIcon();
        UpdateSFXIcon();
    }
}