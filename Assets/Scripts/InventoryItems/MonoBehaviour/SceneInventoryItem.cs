using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInventoryItem : InteractableObject
{
    protected override void Interact()
    {
        base.Interact();
        gameObject.SetActive(false);
        Destroy(gameObject);
        PickUp();
    }

    protected virtual void PickUp() { }
}
