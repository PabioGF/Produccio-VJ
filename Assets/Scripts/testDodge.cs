using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDodge : MonoBehaviour
{
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector3.left * 10f;
        if (transform.position.x < -5)
        {
            transform.position = new Vector3(5, transform.position.y, transform.position.z);
        }
    }


}
