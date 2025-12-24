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


    public void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Defender_Object") && isFull == false)
        {
            gameManager.currentContainer = this.gameObject;
            backgroundImage.enabled = true;
        }
    }

    public void OnCollisionExit2D(UnityEngine.Collision2D collision)
    {
        gameManager.currentContainer = null;
        backgroundImage.enabled = false;
    }
}
