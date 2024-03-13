using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : SceneInventoryItem
{
    protected override void PickUp()
    {
        _playerController.AddItem(new Shield());
    }
}
