using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class LoadingScreenUI : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private float fakeLoadDelay = 2f;
    [SerializeField] private String sceneName;
    [SerializeField] private float dotAnimationSpeed = 0.5f; // Time between dot updates
    
    private int dotCount = 1; // Current number of dots (1, 2, 3, 2, 1, 2, 3...)
    private bool dotIncreasing = true; // Whether dots are increasing or decreasing


    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(fakeLoadDelay);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        
        float dotTimer = 0f;
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;
            
            // Update dot animation
            dotTimer += Time.deltaTime;
            if (dotTimer >= dotAnimationSpeed)
            {
                dotTimer = 0f;
                UpdateDotCount();
            }

            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    
    void UpdateDotCount()
    {
        if (dotIncreasing)
        {
            dotCount++;
            if (dotCount >= 3)
            {
                dotCount = 3;
                dotIncreasing = false;
            }
        }
        else
        {
            dotCount--;
            if (dotCount <= 1)
            {
                dotCount = 1;
                dotIncreasing = true;
            }
        }
    }
}