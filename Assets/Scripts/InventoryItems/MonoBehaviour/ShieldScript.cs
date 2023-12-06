using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldScript : SceneInventoryItem
{
    protected override void PickUp()
    {
        _playerController.AddItem(new Shield());
        base.PickUp();
    }
}
