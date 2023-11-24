using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _collider;
    private bool _isOpen;

    private void Open()
    {
        _isOpen = true;
        _collider.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerController>().HasKey()) { Debug.Log("[DoorScript] Open Door"); Open(); }
        }
    }
}
