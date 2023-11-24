using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollected : MonoBehaviour
{
    //Funció per detectar la colisió entre usuari-objecte
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") /*&& Input.GetKeyDown(KeyCode.E)*/)
        {
            collision.GetComponent<PlayerController>().setKey(true);
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, 0.5f);
        }
    }
}
