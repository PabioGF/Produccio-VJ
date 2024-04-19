using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleScript : SceneInventoryItem
{
    [SerializeField] private int _damage;
    [SerializeField] private float _throwSpeed;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Rigidbody2D _rigidbody;
    //[SerializeField] private AudioClip _disappearSound;

    private bool _isPickedUp;
    private bool _isThrown;
    private int _bottles;
    //private AudioSource _audioSource;

    /*private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }*/

    protected override void PickUp()
    {
        if (_isPickedUp) return;
        _isPickedUp = true;
        _playerController.AddItem(new Bottle(_damage, _throwSpeed, gameObject));
        _bottles = UIController.Instance.GetBottles();
        _bottles += 1;
        UIController.Instance.SetBottles(_bottles);
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
        _bottles = UIController.Instance.GetBottles();
        _bottles -= 1;
        UIController.Instance.SetBottles(_bottles);
        _isThrown = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isThrown) return;

        if (collision.collider.CompareTag("Enemy"))
        {
            LifeComponent life = collision.gameObject.GetComponentInChildren<LifeComponent>();
            life.ReceiveHit(_damage);
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

    public void SetPlayerRansform(Transform player)
    {
        Debug.Log("x2");
        _playerTransform = player;
    }


}
