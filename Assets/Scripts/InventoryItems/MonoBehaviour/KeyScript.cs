using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyScript : SceneInventoryItem
{
    [SerializeField] private int _id;
    [SerializeField] float _pickupVolume;

    protected override void PickUp()
    {
        _playerController.AddItem(new Key(_id));
        AudioManager.Instance.PlaySFX("ObtainKey", _pickupVolume);
        Destroy(gameObject);
    }
}