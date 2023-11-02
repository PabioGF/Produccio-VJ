using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxController : MonoBehaviour
{
    [SerializeField] private GameObject _parent;
    private PlayerCombat _playerCombatScript;
    
    void Awake()
    {
        _playerCombatScript = _parent.GetComponent<PlayerCombat>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerCombatScript.OnHurtboxTriggerEnter2D(collision);
    }
}
