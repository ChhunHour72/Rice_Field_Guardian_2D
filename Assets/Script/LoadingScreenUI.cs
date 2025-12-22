using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;

public class LoadingScreenUI : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private Text loadingText;
    [SerializeField] private Image hoverImage;
    [SerializeField] private float fakeLoadDelay = 2f;
    [SerializeField] private String sceneName;


    void Start()
    {
        // Hide hover image initially
        if (hoverImage != null)
            hoverImage.enabled = false;

        // Add event triggers for hover
        EventTrigger trigger = progressBar.gameObject.AddComponent<EventTrigger>();
        
        // On Pointer Enter
        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((data) => { OnHoverEnter(); });
        trigger.triggers.Add(pointerEnter);
        
        // On Pointer Exit
        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((data) => { OnHoverExit(); });
        trigger.triggers.Add(pointerExit);

        StartCoroutine(LoadSceneAsync());
    }

    private void OnHoverEnter()
    {
        if (hoverImage != null)
            hoverImage.enabled = true;
    }

    private void OnHoverExit()
    {
        if (hoverImage != null)
            hoverImage.enabled = false;
    }

    IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(fakeLoadDelay);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;
            loadingText.text = $"Loading...{Mathf.Floor(progress * 100)}%";

            // Move the hover image along with the progress bar
            if (hoverImage != null)
            {
                RectTransform barRect = progressBar.GetComponent<RectTransform>();
                RectTransform imageRect = hoverImage.GetComponent<RectTransform>();
                
                // Calculate the position based on progress
                float barWidth = barRect.rect.width;
                float newXPosition = (progress * barWidth) - (barWidth / 2f);
                
                imageRect.anchoredPosition = new Vector2(newXPosition, imageRect.anchoredPosition.y);
            }

            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}