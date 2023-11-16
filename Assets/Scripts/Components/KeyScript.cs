using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("[KeyScript] Get Key");
            collision.GetComponent<PlayerController>().GiveKey(true);
            Destroy(gameObject);
        }
    }
}
