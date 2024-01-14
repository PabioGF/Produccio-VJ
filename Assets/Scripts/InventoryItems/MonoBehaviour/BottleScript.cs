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
    private bool _isThrow;

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

    public void Throw(Vector2 direction)
    { 
        gameObject.transform.position = _playerTransform.position;
        gameObject.SetActive(true);
        if (direction == Vector2.zero)
            direction = _playerTransform.right;
        _rigidbody.velocity = _throwSpeed * direction;
        _playerController.RemoveItem(InventoryItem.ItemType.Bottle);
        _isThrow = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isThrow) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.TryGetComponent<LifeComponent>(out var life))
            {
                life.ReceiveHit(_damage);
            }
        }

        Destroy(gameObject, 0.1f);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (!_pickedUp) base.OnTriggerExit2D (collision);
    }
}
