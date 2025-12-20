using UnityEngine;
using UnityEngine.UI;

public class MusicButtonRedirector : MonoBehaviour
{
    [SerializeField] private bool isMuteButton = true; // true = mute button, false = unmute button
    [SerializeField] private bool isToggleButton = false; // Optional: add a toggle variant

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClicked);
        }
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }
    }

    private void OnButtonClicked()
    {
        if (isToggleButton)
        {
            AudioManager.StaticToggleMusic();
        }
        else if (isMuteButton)
        {
            AudioManager.StaticMuteMusic();
        }
        else
        {
            AudioManager.StaticUnmuteMusic();
        }
    }
}