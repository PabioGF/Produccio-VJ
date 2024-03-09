using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleScript : SceneInventoryItem
{
    [SerializeField] private int _damage;
    [SerializeField] private float _throwSpeed;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Rigidbody2D _rigidbody;
    private bool _isPickedUp;
    private bool _isThrown;

    protected override void PickUp()
    {
        if (_isPickedUp) return;
        _isPickedUp = true;
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
        // Sets the bottle in the position of the player
        gameObject.transform.position = _playerTransform.position;
        gameObject.SetActive(true);

        // Forces it to have a direction
        if (direction == Vector2.zero)
            direction = _playerTransform.right;

        // Sends the bottle to that direction and removes it from the inventory
        _rigidbody.velocity = _throwSpeed * direction;
        _playerTransform.GetComponent<PlayerController>().RemoveItem(InventoryItem.ItemType.Bottle);
        _isThrown = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isThrown) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.TryGetComponent<LifeComponent>(out var life))
            {
                life.ReceiveHit(_damage);
            }
        }

        Destroy(gameObject, 0.1f);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // Only is interactable when it hasn't been picked up yet
        if (_isPickedUp) return;
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        // Only is interactable when it hasn't been picked up yet
        if (_isThrown) return;
        base.OnTriggerExit2D(collision);
    }
}
