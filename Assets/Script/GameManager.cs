using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject draggingObject;
    public GameObject currentContainer;
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void PlaceObject()
    {
        
    }
}
