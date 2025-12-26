using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public Image characterImage; // Character sprite for each line
    public Sprite[] lineImages; // Array of sprites corresponding to each line
    public string[] lines;
    public float textSpeed;
    public Image continueIndicator; // Optional: UI indicator for "ready to continue"
    public GameObject dialogueParent; // Parent GameObject containing this dialogue
    public Image darkOverlay; // Dark semi-transparent Image to block all input during dialogue

    private int index;
    private bool isAnimating = false; // Track animation state
    private bool animationComplete = false; // Track when current line animation finishes
    
    // Static flag to track if dialogue/tutorial is active
    public static bool isTutorialActive = false;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = String.Empty;
        if (continueIndicator != null)
            continueIndicator.enabled = false;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleUserInput();
        }
    }

    void HandleUserInput()
    {
        // If animation is still playing, skip it
        if (isAnimating && !animationComplete)
        {
            SkipAnimation();
        }
        // If animation is complete, advance to next line
        else if (animationComplete)
        {
            AdvanceToNextLine();
        }
    }

    void SkipAnimation()
    {
        // Stop the typing coroutine
        StopAllCoroutines();
        
        // Display the full text immediately
        textComponent.text = lines[index];
        
        // Mark animation as complete (separate from playback state)
        isAnimating = false;
        animationComplete = true;
        
        // Show visual feedback that animation was skipped
        ShowContinueIndicator();
    }

    void StartDialogue()
    {
        isTutorialActive = true;
        index = 0;
        animationComplete = false;
        isAnimating = true;
        if (continueIndicator != null)
            continueIndicator.enabled = false;
        // Enable dark overlay to block all input
        if (darkOverlay != null)
            darkOverlay.enabled = true;
        DisplayLineImage();
        StartCoroutine(Typeline());
    }

    void DisplayLineImage()
    {
        // Display the character image corresponding to current line
        if (characterImage == null)
        {
            Debug.LogError("Dialogue: characterImage is not assigned!");
            return;
        }
        
        if (lineImages == null || lineImages.Length == 0)
        {
            Debug.LogError("Dialogue: lineImages array is empty or not assigned!");
            return;
        }
        
        if (index >= lineImages.Length)
        {
            Debug.LogWarning($"Dialogue: index {index} is out of lineImages array bounds (length: {lineImages.Length})");
            return;
        }
        
        // If image is empty/null for this line, disable the character image
        if (lineImages[index] == null)
        {
            characterImage.enabled = false;
            return;
        }
        
        characterImage.enabled = true;
        characterImage.sprite = lineImages[index];
        Debug.Log($"Dialogue: Changed image to lineImages[{index}]");
    }

    IEnumerator Typeline()
    {
        textComponent.text = string.Empty;
        
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        
        // Animation completed naturally
        isAnimating = false;
        animationComplete = true;
        ShowContinueIndicator();
    }

    void ShowContinueIndicator()
    {
        // Visual feedback: show indicator that player can advance
        if (continueIndicator != null)
        {
            continueIndicator.enabled = true;
            // Optional: add animation to indicator
            StartCoroutine(PulseIndicator());
        }
    }

    IEnumerator PulseIndicator()
    {
        // Optional: create a pulsing effect on the continue indicator
        if (continueIndicator == null) yield break;
        
        while (animationComplete && continueIndicator.enabled)
        {
            // Fade in and out
            float duration = 0.5f;
            float elapsed = 0f;
            
            while (elapsed < duration && animationComplete)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(0.5f, 1f, Mathf.PingPong(elapsed / duration, 1f));
                Color color = continueIndicator.color;
                color.a = alpha;
                continueIndicator.color = color;
                yield return null;
            }
        }
    }

    void AdvanceToNextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            animationComplete = false;
            isAnimating = true;
            
            if (continueIndicator != null)
                continueIndicator.enabled = false;
            
            DisplayLineImage();
            StartCoroutine(Typeline());
        }
        else
        {
            // All lines completed
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isTutorialActive = false;
        if (darkOverlay != null)
            darkOverlay.enabled = false;
        if (continueIndicator != null)
            continueIndicator.enabled = false;
        
        // Close the dialogue parent if assigned, otherwise close this GameObject
        if (dialogueParent != null)
            dialogueParent.SetActive(false);
        else
            gameObject.SetActive(false);
    }

}