using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class PlayerLifeComponent : MonoBehaviour
{
    #region Global Variables
    [Header("Specific fields")]
    [SerializeField] private PlayerCombat _playerCombat;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float _hitStopDuration;
    [SerializeField] int _scoreSubstractByHit;

    [SerializeField] protected GameObject _parent;
    [SerializeField] protected float _maxLife;
    [SerializeField] private SpriteRenderer sprite;

    protected float _currentLife;
    protected bool _isDead;
    private int _shield;

    [SerializeField] private HealthBarComponent _healthBar;

    [Header("Audio")]
    [SerializeField] private AudioClip _healSound;
    [SerializeField] private float _healVolume = 1.0f;
    private AudioSource _audioSource;
    #endregion

    private void Awake()
    {
        _healthBar = _healthBar.GetComponentInChildren<HealthBarComponent>();
    }
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        _currentLife = _maxLife;
        _healthBar.UpdateHealthBar(_currentLife, _maxLife);
        UIController.Instance.UpdateHealthBar(_currentLife, _maxLife);
        UIController.Instance.SetLife(_currentLife);
        
    }

    public void ReceiveHit(float amount, Vector3 damagePosition)
    {
        // If the player is deflecting, counter attacks
        if (_playerCombat.Deflect)
        {
            _playerCombat.OnDeflect();
            return;
        }

        // If the player is invulnerable returns
        if (_playerCombat.IsInvulnerable) return;

        GameController.Instance.StopTime(0f, _hitStopDuration);
        StartCoroutine(CameraShaker.Instance.ShakeCamera(.5f, .8f));

        // If the player has a shield, removes it instead of taking the damage and stops
        if (_playerController.HasItem(InventoryItem.ItemType.Shield))
        {
            Debug.Log("Shield");
            _playerController.RemoveItem(InventoryItem.ItemType.Shield);

            _shield = UIController.Instance.GetShield();
            if (_shield > 0)
            {
                _shield -= 1;
            }

            UIController.Instance.SetShield(_shield);
            return;
        }

        // Recieves the damage of the hit, updates the UI and checks if the player is dead
        _currentLife -= amount;
        _healthBar.UpdateHealthBar(_currentLife, _maxLife);
        UIController.Instance.UpdateHealthBar(_currentLife, _maxLife);
        UIController.Instance.SetLife(_currentLife);
    
        GameController.Instance.SubstractScore(_scoreSubstractByHit);
        if (_currentLife <= 0) _isDead = true;

        // Shows the death screen if the player is dead
        if (_isDead)
        {
            PlayerController playerController = _parent.GetComponent<PlayerController>();
            playerController.Die();
            UIController.Instance.ShowDeathScreen();
        }

        // Visually shows the player has been hit
        _playerCombat.GetHit(damagePosition);
    }

    /// <summary>
    /// Heals the player and updates the UI
    /// </summary>
    /// <param name="healingPoints">Amount to heal</param>
    public void Heal(int healingPoints)
    {
        _currentLife += healingPoints;
        if (_currentLife > _maxLife) _currentLife = _maxLife;
        
        _healthBar.UpdateHealthBar(_currentLife, _maxLife);
        UIController.Instance.UpdateHealthBar(_currentLife, _maxLife);
        UIController.Instance.SetLife(_currentLife);
        
        StartCoroutine(FlashGreen());
        if (_healSound != null)
        {
            _audioSource.PlayOneShot(_healSound, _healVolume);
        }
    }

    public IEnumerator FlashGreen()
    {
        sprite.color = Color.green;
        Debug.Log("Entro aqui" + sprite.color);
        yield return new WaitForSeconds(0.5f);
        sprite.color = Color.white;
    }

}