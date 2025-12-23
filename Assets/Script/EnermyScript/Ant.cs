using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    public float speed;
    public int health;
    private void FixedUpdate()
    {
        transform.position -= new Vector3(speed, 0,0);
    }
}
