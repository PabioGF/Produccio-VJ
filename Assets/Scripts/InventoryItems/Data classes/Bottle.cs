using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : InventoryItem
{
    private int _damage;
    private float _speed;
    private GameObject _gameBottle;
    public Bottle(int damage, float speed, GameObject bottle)
    {
        _type = ItemType.Bottle;
        _damage = damage;
        _speed = speed;
        _gameBottle = bottle;
    }

    public int Damage => _damage;
    public float Speed => _speed;
    public GameObject Object => _gameBottle;
}