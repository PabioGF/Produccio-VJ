using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : MonoBehaviour
{

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected");
        LifeComponent life = collision.gameObject.GetComponent<LifeComponent>();
        if (life != null)
        {
            life.ApplyDamage(1f);
        }
    }

}
