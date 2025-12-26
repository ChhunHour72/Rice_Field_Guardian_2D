using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_menu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    void Start()
    {
        InitialState();
    }

    void Update()
    {
        
    }

    void InitialState()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; 
    }
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; 
    }
}
