using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConveyorItem : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public float speed = 1f;    
    public GameObject object_drag;
    public GameObject object_game;
    public Canvas canvas;
    private GameObject objectDragInstance;


    private Rigidbody2D rb;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            Debug.Log("DESTROYED");
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position -= new Vector3(speed, 0,0);        
    }


// Drag Implementation

    public void OnDrag(PointerEventData eventData)
    {
        objectDragInstance.transform.position = Input.mousePosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        objectDragInstance = Instantiate(object_drag, canvas.transform);
        objectDragInstance.transform.position = Input.mousePosition;
        GameManager.instance.draggingObject = objectDragInstance;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameManager.instance.draggingObject = null;
        Destroy(objectDragInstance);
    }



}
