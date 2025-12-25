using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;

    [Header("Remaining Time")]
    [SerializeField] float seconds = 60f; // example starting time

    void Update()
    {
        if (seconds > 0)
        {
            seconds -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(seconds / 60);
            int secs = Mathf.FloorToInt(seconds % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, secs);
        }
        else
        {
            // Timer reached zero
            seconds = 0;
            timerText.text = "00:00";

            // Load Win Scene
            SceneManager.LoadScene("WinScene_3"); // Replace with your scene name
        }
    }

}
