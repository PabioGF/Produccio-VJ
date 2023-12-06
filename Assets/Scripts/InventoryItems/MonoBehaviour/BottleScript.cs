using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleScript : SceneInventoryItem
{
    [SerializeField] private int _damage;
    [SerializeField] private float _throwSpeed;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Rigidbody2D _rigidbody;
    private bool _pickedUp;

    protected override void PickUp()
    {
        if (_pickedUp) return;
        _pickedUp = true;
        _playerController.AddItem(new Bottle(_damage, _throwSpeed, gameObject));
        gameObject.SetActive(false);
    }

    public void SetData(Bottle data)
    {
        _damage = data.Damage;
        _throwSpeed = data.Speed;
    }

    public void Throw()
    {
        gameObject.transform.position = _playerTransform.position + _playerTransform.right;
        gameObject.SetActive(true);
        _rigidbody.velocity = new Vector2(_throwSpeed, 0) * _playerTransform.right;
        _playerController.RemoveItem(InventoryItem.ItemType.Bottle);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (!_pickedUp) base.OnTriggerExit2D (collision);
    }
}
