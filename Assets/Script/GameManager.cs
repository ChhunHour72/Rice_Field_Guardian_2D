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
        if (draggingObject == null) return false;

        // Find container under dragged object
        Collider2D hit = Physics2D.OverlapPoint(
            draggingObject.transform.position,
            LayerMask.GetMask("Container")
        );

        if (hit == null) return false;

        ObjectContainer container = hit.GetComponent<ObjectContainer>();
        if (container == null || container.isFull) return false;

        Instantiate(
            draggingObject.GetComponent<ObjectDragging>().card.object_game,
            container.transform.position,
            Quaternion.identity,
            container.transform
        );

        container.isFull = true;
        Debug.Log("Placed");
        return true;
    }
}
