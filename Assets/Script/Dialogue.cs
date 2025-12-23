using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Dialogue : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Video player component
    public VideoClip[] videoClips; // Array of video clips to play sequentially
    
    private int currentIndex;
    private bool isDialogueActive;

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("Dialogue: videoPlayer is not assigned!");
            return;
        }
        
        if (videoClips == null || videoClips.Length == 0)
        {
            Debug.LogError("Dialogue: videoClips array is empty or not assigned!");
            return;
        }
        
        StartDialogue();
    }

    void Update()
    {
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            AdvanceToNextBanner();
        }
    }

    void StartDialogue()
    {
        currentIndex = 0;
        isDialogueActive = true;
        DisplayBanner();
    }

    void DisplayBanner()
    {
        if (currentIndex >= videoClips.Length)
        {
            Debug.LogWarning($"Dialogue: currentIndex {currentIndex} is out of videoClips array bounds!");
            return;
        }
        
        if (videoClips[currentIndex] == null)
        {
            Debug.LogWarning($"Dialogue: videoClips[{currentIndex}] is null!");
            return;
        }
        
        videoPlayer.clip = videoClips[currentIndex];
        videoPlayer.Play();
    }

    void AdvanceToNextBanner()
    {
        if (currentIndex < videoClips.Length - 1)
        {
            currentIndex++;
            DisplayBanner();
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        gameObject.SetActive(false);
    }
}
