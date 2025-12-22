using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorItem : MonoBehaviour
{
    public float speed = 1f;
    public float DestroyInSecond = 10f;
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
