using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private LockedDoorController _doorController;

    private List<LifeComponent> _enemiesLife;
    private bool _playerIsInside;

    private void Start()
    {
        _enemiesLife = new List<LifeComponent>();
        foreach (var enemy in _enemies)
        {
            _enemiesLife.Add(enemy.GetComponent<LifeComponent>());
        }
    }

    void Update()
    {
        if (_playerIsInside)
        {
            if (CheckEnemies()) UnlockDoor();
        }       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerIsInside = true;
        }
    }

    private bool CheckEnemies()
    {
        foreach(LifeComponent enemy in _enemiesLife)
        {
            if (!enemy.IsDead) return false;
        }
        return true;
    }

    private void UnlockDoor()
    {
        _doorController.UnlockDoor();
        gameObject.SetActive(false);
    }
}
