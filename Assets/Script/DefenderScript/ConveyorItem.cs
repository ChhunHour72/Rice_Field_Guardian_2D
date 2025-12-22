using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConveyorItem : MonoBehaviour, IDragHandler
{
    public float speed = 1f;
    public float DestroyInSecond = 10f;
    public GameObject object_drag;
    public GameObject object_game;

    
    public void OnDrag(PointerEventData eventData)
    {
        
    }

    // Update is called once per frame
    private void Start()
    {
        Destroy(gameObject, DestroyInSecond);
    }
    private void FixedUpdate()
    {
        transform.position -= new Vector3(speed, 0,0);        
    }



}
