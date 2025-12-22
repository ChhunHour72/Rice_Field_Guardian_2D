using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectContainer : MonoBehaviour
{

    public bool isFull;
    public Image backgroundImage;
    public GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameManager.draggingObject != null && isFull == false)
        {
            gameManager.currentContainer = this.gameObject;
            backgroundImage.enabled = false;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        gameManager.currentContainer = null;
        backgroundImage.enabled = false;
    }
}
