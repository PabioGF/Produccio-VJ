using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackComponent : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private int _scoreSubstractByContact;

    private IAController _enemyController;

    private void Start()
    {
        _enemyController = GetComponentInParent<IAController>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerLifeComponent>(out var life))
        {
            Debug.Log(collision.gameObject.name);
            life.ReceiveHit(_damage, transform.position);
            GameController.Instance.SubstractScore(_scoreSubstractByContact);

            if (_enemyController != null) _enemyController.PlayHitSound(); 
        }
    }
}
