using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDodge : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    public bool upAttack;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
    }

    void Update()
    {
        rb.velocity = Vector3.left * speed;
        if (transform.position.x < -5)
        {
            transform.position = new Vector3(5, transform.position.y, transform.position.z);
        }
    }

    public bool GetAttackType()
    {
        return upAttack;
    }
}
