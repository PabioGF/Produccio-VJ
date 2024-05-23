using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : SceneInventoryItem
{
    private int _shield;
    protected override void PickUp()
    {
        _playerController.AddItem(new Shield());
        AudioManager.Instance.PlaySFX("Shield Equip", 4);

        _shield = UIController.Instance.GetShield();
        _shield += 1;
       
        UIController.Instance.SetShield(_shield);
    }
}
