using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MandatoryRoom : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private MandatoryDoor[] _doorControllers;

    private List<LifeComponent> _enemiesLife;
    private bool _playerIsInside;

    #region Unity Methods
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
            CheckEnemies();
        }       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerIsInside = true;
            foreach (MandatoryDoor door in _doorControllers)
            {
                door.ToggleDoor();
            }
        }
    }
    #endregion

    private void CheckEnemies()
    {
        bool enemiesAlive = false;

        foreach(LifeComponent enemy in _enemiesLife)
        {
            if (!enemy.IsDead) enemiesAlive = true;
        }

        if (!enemiesAlive)
        {
            // If the enemies are dead, unlocks de doors and disables itself
            UnlockDoors();
            gameObject.SetActive(false);
        }
    }

    private void UnlockDoors()
    {
        foreach (MandatoryDoor door in _doorControllers)
        {
            door.RoomIsCompleted = true;
            door.ToggleDoor();
        }
    }
}
