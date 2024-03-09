using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInventoryItem : InteractableObject
{
    protected override void Interact()
    {
        base.Interact();
        PickUp();
        gameObject.SetActive(false); 
    }

    protected virtual void PickUp() { }
}
