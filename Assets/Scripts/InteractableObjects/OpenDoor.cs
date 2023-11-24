using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public Sprite spriteOpenedDoor;
    public Collider2D colliderDesapear; 
    private bool doorOpened = false;

    //Función que abre la puerta si se dispone de la llave
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!doorOpened && collision.CompareTag("Player"))
        {
            bool hasKey = collision.GetComponent<PlayerController>().hasKey;

            if (hasKey)
            {
                doorOpened = true;
                collision.GetComponent<PlayerController>().setKey(false);
                GetComponent<SpriteRenderer>().sprite = spriteOpenedDoor;
                GetComponent<Collider2D>().enabled = false;
                colliderDesapear.enabled = false; 
                
            }
            else
            {
                Debug.Log("Necesitas una llave para abrir esta puerta.");
            }
        }
    }
}
