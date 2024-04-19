using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : SceneInventoryItem
{
    private int _shield;
    protected override void PickUp()
    {
        Debug.Log("New Shield");
        _playerController.AddItem(new Shield());

        _shield = UIController.Instance.GetShield();
        _shield += 1;
       
        UIController.Instance.SetShield(_shield);
    }
}
