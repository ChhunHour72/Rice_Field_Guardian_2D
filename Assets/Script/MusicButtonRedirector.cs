using UnityEngine;
using UnityEngine.UI;

public class MusicButtonRedirector : MonoBehaviour
{
    [Header("Type (Select ONE for each button)")]
    [SerializeField] private bool isMusicMute = false;
    [SerializeField] private bool isMusicUnmute = false;
    [SerializeField] private bool isSFXMute = false;
    [SerializeField] private bool isSFXUnmute = false;

    [Header("Visual Feedback (Recommended)")]
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite onSprite;   // e.g., speaker with waves
    [SerializeField] private Sprite offSprite;  // e.g., speaker crossed out

    [Header("Which state does this button represent?")]
    [SerializeField] private bool controlsMusic = true;  // true = music buttons, false = SFX buttons

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("No Button component found!");
            return;
        }

        button.onClick.AddListener(OnButtonClicked);
        UpdateVisual();
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        if (controlsMusic)
        {
            if (isMusicMute) AudioManager.StaticMuteMusic();
            else if (isMusicUnmute) AudioManager.StaticUnmuteMusic();
        }
        else
        {
            if (isSFXMute) AudioManager.StaticMuteSFX();
            else if (isSFXUnmute) AudioManager.StaticUnmuteSFX();
        }

        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (buttonImage == null || onSprite == null || offSprite == null) return;

        bool isOn = controlsMusic ? 
            (AudioManager.Instance != null && AudioManager.Instance.IsMusicOn()) :
            (AudioManager.Instance != null && AudioManager.Instance.IsSFXOn());

        buttonImage.sprite = isOn ? onSprite : offSprite;
    }

    public void RefreshVisual()
    {
        UpdateVisual();
    }
}