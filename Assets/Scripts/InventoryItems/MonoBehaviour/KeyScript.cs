using Cinemachine.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyScript : SceneInventoryItem
{
    [SerializeField] private int _id;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void PickUp()
    {
        _playerController.AddItem(new Key(_id));
        base.PickUp();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision); 
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
    }   
}
