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

    public bool PlaceObject()
    {
        if(draggingObject != null && currentContainer != null)
        {
            Instantiate(draggingObject.GetComponent<ObjectDragging>().card.object_game, currentContainer.transform.position, Quaternion.identity, currentContainer.transform);
            currentContainer.GetComponent<ObjectContainer>().isFull = true;
            Debug.Log("Placed");
            return true;
        }
        else return false;
    }
}
