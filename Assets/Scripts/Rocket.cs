using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    public float speed;

    

    void Start()
    {
        speed = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "plane")
        {
            SimplePool.Despawn(gameObject);
        } 
    }
}
